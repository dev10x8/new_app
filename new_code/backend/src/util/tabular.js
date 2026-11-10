import { inflateRawSync } from 'node:zlib';

/**
 * Minimal tabular parsing used by the bulk-import features. Everything here is
 * pure Node (no third-party dependency):
 *
 *   - parseDelimited : CSV / TSV (RFC-4180 quoting)
 *   - rowsToObjects  : header row -> array of plain objects
 *   - readXlsx       : an .xlsx workbook (a ZIP of XML) -> sheet -> rows
 *
 * The XLSX reader handles the common case: shared strings, inline strings,
 * numeric cells and the first worksheet. It is deliberately small - it exists
 * so the shopkeeper can drop in the same Excel file the legacy app accepted,
 * without adding a spreadsheet dependency to the runtime.
 */

// --------------------------------------------------------------------- CSV/TSV

/**
 * Split delimited text into rows of cells, honouring "..." quoting and doubled
 * quotes ("") inside a quoted cell. Blank trailing lines are dropped.
 * @returns {string[][]}
 */
export function parseDelimited(text, delimiter = ',') {
  const src = String(text ?? '').replace(/\r\n?/g, '\n');
  const rows = [];
  let row = [];
  let cell = '';
  let inQuotes = false;
  for (let i = 0; i < src.length; i++) {
    const ch = src[i];
    if (inQuotes) {
      if (ch === '"') {
        if (src[i + 1] === '"') { cell += '"'; i++; } else inQuotes = false;
      } else cell += ch;
      continue;
    }
    if (ch === '"') { inQuotes = true; continue; }
    if (ch === delimiter) { row.push(cell); cell = ''; continue; }
    if (ch === '\n') { row.push(cell); rows.push(row); row = []; cell = ''; continue; }
    cell += ch;
  }
  if (cell !== '' || row.length) { row.push(cell); rows.push(row); }
  return rows.filter((r) => r.some((c) => String(c).trim() !== ''));
}

/** Convert an array-of-arrays with a header row into objects (trimmed keys). */
export function rowsToObjects(rows) {
  if (!rows.length) return [];
  const header = rows[0].map((c) => String(c).trim());
  return rows.slice(1).map((r) => {
    const o = {};
    header.forEach((k, i) => { o[k] = r[i] === undefined ? '' : r[i]; });
    return o;
  });
}

/** Detect the delimiter from the header line (comma / tab / semicolon). */
export function sniffDelimiter(text) {
  const line = String(text ?? '').split('\n')[0] || '';
  const counts = [[',', (line.match(/,/g) || []).length], ['\t', (line.match(/\t/g) || []).length], [';', (line.match(/;/g) || []).length]];
  counts.sort((a, b) => b[1] - a[1]);
  return counts[0][1] > 0 ? counts[0][0] : ',';
}

// ------------------------------------------------------------------------ XLSX

/** Read the local-file entries of a ZIP archive into a name -> Buffer map. */
export function unzip(buffer) {
  const files = new Map();
  let i = 0;
  while (i < buffer.length - 4) {
    // local file header signature 0x04034b50
    if (buffer.readUInt32LE(i) !== 0x04034b50) break;
    const method = buffer.readUInt16LE(i + 8);
    let compSize = buffer.readUInt32LE(i + 18);
    const nameLen = buffer.readUInt16LE(i + 26);
    const extraLen = buffer.readUInt16LE(i + 28);
    const name = buffer.toString('utf8', i + 30, i + 30 + nameLen);
    const dataStart = i + 30 + nameLen + extraLen;
    if (compSize === 0 && method === 8) {
      // sizes live in the data descriptor (general-purpose bit 3): scan for the
      // next local-file signature to bound the compressed payload.
      compSize = findNextLocalHeader(buffer, dataStart) - dataStart;
    }
    const raw = buffer.subarray(dataStart, dataStart + compSize);
    let content;
    if (method === 0) content = Buffer.from(raw);
    else if (method === 8) content = inflateRawSync(raw);
    else throw new Error(`Unsupported zip compression method ${method}`);
    files.set(name, content);
    i = dataStart + compSize;
  }
  return files;
}

function findNextLocalHeader(buffer, from) {
  for (let j = from; j < buffer.length - 4; j++) {
    if (buffer.readUInt32LE(j) === 0x04034b50) return j;
  }
  return buffer.length;
}

const XML_ENTITIES = { '&amp;': '&', '&lt;': '<', '&gt;': '>', '&quot;': '"', '&apos;': "'" };
const decodeXml = (s) => String(s).replace(/&(amp|lt|gt|quot|apos);/g, (m) => XML_ENTITIES[m]);

/** Extract the ordered shared-string table from xl/sharedStrings.xml. */
function sharedStrings(xml) {
  if (!xml) return [];
  const out = [];
  for (const si of xml.match(/<si>[\s\S]*?<\/si>/g) || []) {
    const parts = [...si.matchAll(/<t[^>]*>([\s\S]*?)<\/t>/g)].map((m) => decodeXml(m[1]));
    out.push(parts.join(''));
  }
  return out;
}

/** Convert a column reference like "AB12" to a 0-based column index. */
function colIndex(ref) {
  const letters = (ref.match(/^[A-Z]+/) || [''])[0];
  let n = 0;
  for (const ch of letters) n = n * 26 + (ch.charCodeAt(0) - 64);
  return n - 1;
}

/** Parse one worksheet XML into an array of cell-value rows. */
function parseSheet(xml, strings) {
  const rows = [];
  for (const rowXml of xml.match(/<row[^>]*>[\s\S]*?<\/row>/g) || []) {
    const cells = [];
    for (const c of rowXml.match(/<c[^>]*\/>|<c[^>]*>[\s\S]*?<\/c>/g) || []) {
      const refMatch = c.match(/r="([A-Z]+\d+)"/);
      const idx = refMatch ? colIndex(refMatch[1]) : cells.length;
      const t = (c.match(/t="([^"]+)"/) || [])[1];
      let value = '';
      if (t === 'inlineStr') {
        value = [...c.matchAll(/<t[^>]*>([\s\S]*?)<\/t>/g)].map((m) => decodeXml(m[1])).join('');
      } else {
        const v = (c.match(/<v>([\s\S]*?)<\/v>/) || [])[1];
        if (v !== undefined) value = t === 's' ? (strings[Number(v)] ?? '') : decodeXml(v);
      }
      cells[idx] = value;
    }
    rows.push(cells.map((x) => (x === undefined ? '' : x)));
  }
  return rows;
}

/**
 * Read an .xlsx workbook (Buffer/Uint8Array) into a list of
 * { name, rows: string[][] } - one entry per worksheet, in workbook order.
 */
export function readXlsx(buffer) {
  const files = unzip(Buffer.isBuffer(buffer) ? buffer : Buffer.from(buffer));
  const strings = sharedStrings(files.get('xl/sharedStrings.xml')?.toString('utf8'));
  const workbook = files.get('xl/workbook.xml')?.toString('utf8') || '';
  const sheetNames = [...workbook.matchAll(/<sheet[^>]*name="([^"]*)"/g)].map((m) => decodeXml(m[1]));

  const sheetFiles = [...files.keys()]
    .filter((k) => /^xl\/worksheets\/sheet\d+\.xml$/.test(k))
    .sort((a, b) => Number(a.match(/(\d+)/)[1]) - Number(b.match(/(\d+)/)[1]));

  return sheetFiles.map((key, i) => ({
    name: sheetNames[i] || `Sheet${i + 1}`,
    rows: parseSheet(files.get(key).toString('utf8'), strings),
  }));
}

/** Read any supported tabular buffer: .xlsx, or delimited text. */
export function readTable(buffer, filename = '') {
  const ext = String(filename).toLowerCase().split('.').pop();
  const buf = Buffer.isBuffer(buffer) ? buffer : Buffer.from(buffer);
  if (ext === 'xlsx' || (buf.length > 2 && buf[0] === 0x50 && buf[1] === 0x4b)) {
    const sheets = readXlsx(buf);
    const first = sheets[0] || { name: '', rows: [] };
    return { format: 'xlsx', sheets, rows: first.rows };
  }
  const text = buf.toString('utf8');
  return { format: 'text', sheets: [], rows: parseDelimited(text, sniffDelimiter(text)) };
}
