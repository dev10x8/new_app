import { run } from '../db/index.js';
import { now } from './helpers.js';

/** Append an audit entry. Never throws into the caller's flow. */
export function log(db, actor, action, entity = '', detail = '') {
  try {
    run(db, 'INSERT INTO audit_log (at, actor, action, entity, detail) VALUES (?,?,?,?,?)',
      [now(), String(actor || ''), String(action), String(entity), typeof detail === 'string' ? detail : JSON.stringify(detail)]);
  } catch { /* auditing must not break the request */ }
}

/** Recent audit entries, newest first. */
export function recent(db, limit = 100) {
  return db.prepare('SELECT id, at, actor, action, entity, detail FROM audit_log ORDER BY id DESC LIMIT ?').all(Number(limit) || 100);
}
