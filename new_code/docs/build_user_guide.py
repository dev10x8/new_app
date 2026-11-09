#!/usr/bin/env python3
"""Build the Madhushala Pro user guide PDF (Admin and non-admin flows).

Content is written from the application's own sources:
  * new_code/web/menu.js        - navigation tree (mirrors legacy frm_main)
  * new_code/web/screens.js     - every screen and its fields
  * new_code/web/app.js         - login / logout / session
  * new_code/backend/src/*      - services and API behaviour
  * new_code/backend/src/db/*   - schema and seed data (sample codes)
  * new_code/docs/screenshots/  - real screenshots of every screen
  * PRODUCT_FEATURES.md         - business flows, legacy form / table mapping
  * decode/ + dev/db/ddl.sql    - original WinForms forms and SQL Server tables

The screenshots are produced by:
    cd new_code && npm start            # run the app with the demo dataset
    npm run seed-demo                   # load the demo dataset
    npm run screenshots                 # capture docs/screenshots/*.png (CDP)

Run:  python3 build_user_guide.py   ->  Madhushala_Pro_User_Guide.pdf
"""

from __future__ import annotations

import os

from reportlab.lib import colors
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import ParagraphStyle
from reportlab.lib.units import mm
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.platypus import (
    BaseDocTemplate,
    Frame,
    Image,
    KeepTogether,
    NextPageTemplate,
    PageBreak,
    PageTemplate,
    Paragraph,
    Spacer,
    Table,
    TableStyle,
)
from reportlab.platypus.tableofcontents import TableOfContents

from PIL import Image as PILImage

HERE = os.path.dirname(os.path.abspath(__file__))
OUT = os.path.join(HERE, "Madhushala_Pro_User_Guide.pdf")
SHOTS = os.path.join(HERE, "screenshots")

BUILD_DATE = "8 November 2026"
VERSION = "2.0"

# --------------------------------------------------------------------------
# Fonts (DejaVu has the full glyph set we need, incl. the rupee sign).
# --------------------------------------------------------------------------
DEJAVU = "/usr/share/fonts/truetype/dejavu"
pdfmetrics.registerFont(TTFont("Body", f"{DEJAVU}/DejaVuSans.ttf"))
pdfmetrics.registerFont(TTFont("Body-Bold", f"{DEJAVU}/DejaVuSans-Bold.ttf"))
pdfmetrics.registerFont(TTFont("Mono", f"{DEJAVU}/DejaVuSansMono.ttf"))
pdfmetrics.registerFont(TTFont("Mono-Bold", f"{DEJAVU}/DejaVuSansMono-Bold.ttf"))
pdfmetrics.registerFontFamily("Body", normal="Body", bold="Body-Bold",
                              italic="Body", boldItalic="Body-Bold")

# --------------------------------------------------------------------------
# Palette (matches the app design tokens in web/styles.css).
# --------------------------------------------------------------------------
BRAND = colors.HexColor("#0f766e")
BRAND_2 = colors.HexColor("#14b8a6")
INK = colors.HexColor("#0f172a")
INK_2 = colors.HexColor("#475569")
MUTED = colors.HexColor("#94a3b8")
LINE = colors.HexColor("#e2e8f0")
PANEL = colors.HexColor("#f8fafc")
ACCENT = colors.HexColor("#6366f1")

# --------------------------------------------------------------------------
# Styles.
# --------------------------------------------------------------------------
def st(name, **kw):
    base = dict(fontName="Body", fontSize=9.6, leading=14, textColor=INK)
    base.update(kw)
    return ParagraphStyle(name, **base)


S = {
    "h1": st("h1", fontName="Body-Bold", fontSize=20, leading=25, textColor=BRAND,
             spaceBefore=4, spaceAfter=10),
    "body": st("body", spaceAfter=6),
    "muted": st("muted", textColor=INK_2, fontSize=9.2, leading=13),
    "small": st("small", fontSize=8.4, leading=12, textColor=INK_2),
    "step": st("step", fontSize=9.6, leading=13.6, spaceAfter=3),
    "tip": st("tip", fontSize=9, leading=12.8, textColor=colors.HexColor("#1e3a5f")),
    "code": st("code", fontName="Mono", fontSize=8.6, leading=12.6,
               textColor=colors.HexColor("#0b1220")),
    "toc0": st("toc0", fontName="Body-Bold", fontSize=11, leading=16, textColor=BRAND),
    "toc1": st("toc1", fontName="Body-Bold", fontSize=9.8, leading=14, textColor=INK),
    "toc2": st("toc2", fontSize=9.4, leading=13, textColor=INK_2),
    "cover_title": st("cover_title", fontName="Body-Bold", fontSize=34, leading=38,
                      textColor=colors.white),
    "cover_sub": st("cover_sub", fontSize=14, leading=19, textColor=colors.HexColor("#cbd5e1")),
    "cover_meta": st("cover_meta", fontSize=10.5, leading=16, textColor=colors.HexColor("#e2e8f0")),
    "cell": st("cell", fontSize=9.2, leading=13),
    "cellb": st("cellb", fontName="Body-Bold", fontSize=9.2, leading=13),
    "caption": st("caption", fontSize=8.2, leading=11, textColor=MUTED),
}


# --------------------------------------------------------------------------
# Document template with cover + body templates, header/footer, and a TOC.
# --------------------------------------------------------------------------
class GuideDoc(BaseDocTemplate):
    def __init__(self, filename, **kw):
        super().__init__(filename, pagesize=A4,
                         leftMargin=20 * mm, rightMargin=18 * mm,
                         topMargin=20 * mm, bottomMargin=18 * mm,
                         title="Madhushala Pro - User Guide",
                         author="Madhushala Pro", **kw)
        frame_cover = Frame(self.leftMargin, self.bottomMargin, self.width, self.height, id="cover")
        frame_body = Frame(self.leftMargin, self.bottomMargin, self.width, self.height, id="body")
        self.addPageTemplates([
            PageTemplate(id="cover", frames=[frame_cover], onPage=self._cover_page),
            PageTemplate(id="body", frames=[frame_body], onPage=self._body_page),
        ])

    def _cover_page(self, canvas, doc):
        canvas.saveState()
        canvas.setFillColor(colors.HexColor("#0b1220"))
        canvas.rect(0, 0, A4[0], A4[1], stroke=0, fill=1)
        canvas.setFillColor(BRAND)
        canvas.rect(0, A4[1] - 8 * mm, A4[0], 8 * mm, stroke=0, fill=1)
        canvas.setFillColor(BRAND_2)
        canvas.rect(0, 0, A4[0], 6 * mm, stroke=0, fill=1)
        canvas.restoreState()

    def _body_page(self, canvas, doc):
        canvas.saveState()
        canvas.setStrokeColor(LINE)
        canvas.setLineWidth(0.6)
        y = A4[1] - 13 * mm
        canvas.line(self.leftMargin, y, A4[0] - self.rightMargin, y)
        canvas.setFont("Body", 7.8)
        canvas.setFillColor(MUTED)
        canvas.drawString(self.leftMargin, y + 2.4 * mm, "Madhushala Pro - User Guide")
        canvas.drawRightString(A4[0] - self.rightMargin, y + 2.4 * mm, "Admin and User Flows")
        fy = 11 * mm
        canvas.line(self.leftMargin, fy + 4 * mm, A4[0] - self.rightMargin, fy + 4 * mm)
        canvas.setFont("Body", 7.8)
        canvas.setFillColor(MUTED)
        canvas.drawString(self.leftMargin, fy, f"Version {VERSION}  -  {BUILD_DATE}")
        canvas.drawRightString(A4[0] - self.rightMargin, fy, f"Page {canvas.getPageNumber()}")
        canvas.restoreState()

    def afterFlowable(self, flowable):
        if hasattr(flowable, "_toc"):
            level, text = flowable._toc
            self.notify("TOCEntry", (level, text, self.page))


def P(text, style="body"):
    return Paragraph(text, S[style])


def toc_para(text, level):
    style = ["toc0", "toc1", "toc2"][level]
    p = Paragraph(text, S[style])
    p._toc = (level, text)
    return p


def rule(color=LINE, thickness=0.7, space=4):
    t = Table([[""]], colWidths=[170 * mm], rowHeights=[0.1])
    t.setStyle(TableStyle([
        ("LINEBELOW", (0, 0), (-1, -1), thickness, color),
        ("TOPPADDING", (0, 0), (-1, -1), 0),
        ("BOTTOMPADDING", (0, 0), (-1, -1), space),
    ]))
    return t


def callout(title, lines, accent=BRAND, bg=colors.HexColor("#f0fdfa")):
    inner = [Paragraph(f"<b>{title}</b>", S["tip"])]
    for ln in lines:
        inner.append(Paragraph(ln, S["tip"]))
    t = Table([[inner]], colWidths=[170 * mm])
    t.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, -1), bg),
        ("LINEBEFORE", (0, 0), (0, -1), 2.4, accent),
        ("LEFTPADDING", (0, 0), (-1, -1), 9),
        ("RIGHTPADDING", (0, 0), (-1, -1), 9),
        ("TOPPADDING", (0, 0), (-1, -1), 7),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 7),
    ]))
    return t


def meta_table(rows):
    data = []
    for label, value in rows:
        data.append([Paragraph(label, S["cellb"]), Paragraph(value, S["cell"])])
    t = Table(data, colWidths=[32 * mm, 138 * mm])
    t.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (0, -1), PANEL),
        ("LINEBELOW", (0, 0), (-1, -2), 0.4, LINE),
        ("BOX", (0, 0), (-1, -1), 0.5, LINE),
        ("VALIGN", (0, 0), (-1, -1), "TOP"),
        ("LEFTPADDING", (0, 0), (-1, -1), 7),
        ("RIGHTPADDING", (0, 0), (-1, -1), 7),
        ("TOPPADDING", (0, 0), (-1, -1), 4),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 4),
    ]))
    return t


def steps_table(items):
    data = []
    for i, it in enumerate(items, 1):
        data.append([Paragraph(str(i), S["cellb"]), Paragraph(it, S["step"])])
    t = Table(data, colWidths=[8 * mm, 162 * mm])
    t.setStyle(TableStyle([
        ("VALIGN", (0, 0), (-1, -1), "TOP"),
        ("LEFTPADDING", (0, 0), (0, -1), 2),
        ("RIGHTPADDING", (0, 0), (-1, -1), 4),
        ("TOPPADDING", (0, 0), (-1, -1), 2),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 3),
        ("TEXTCOLOR", (0, 0), (0, -1), BRAND),
    ]))
    return t


# --------------------------------------------------------------------------
# Screenshots. Captured by scripts/capture_screenshots.js into docs/screenshots.
# Very tall screens are split into readable slices so nothing is shrunk to
# illegibility on the A4 page.
# --------------------------------------------------------------------------
CONTENT_W = 170 * mm
MAX_IMG_H = 232 * mm
_SLICE_DIR = None


def _shot_path(stem):
    return os.path.join(SHOTS, f"{stem}.png")


def _slice_tall(path, stem):
    """Return a list of image file paths, slicing very tall images."""
    global _SLICE_DIR
    with PILImage.open(path) as im:
        w, h = im.size
        ratio = h / w
        max_ratio = (MAX_IMG_H / CONTENT_W)
        if ratio <= max_ratio:
            return [(path, w, h)]
        parts = int(ratio / max_ratio) + 1
        step = h // parts
        if _SLICE_DIR is None:
            import tempfile
            _SLICE_DIR = tempfile.mkdtemp(prefix="guide-slices-")
        out = []
        for i in range(parts):
            top = i * step
            bottom = h if i == parts - 1 else min(h, (i + 1) * step)
            crop = im.crop((0, top, w, bottom))
            p = os.path.join(_SLICE_DIR, f"{stem}-{i}.png")
            crop.save(p)
            out.append((p, w, bottom - top))
        return out


def screenshot(stem, caption=None):
    """Screenshot flowables (a list) with an optional caption per slice."""
    path = _shot_path(stem)
    if not os.path.exists(path):
        return []
    out = []
    parts = _slice_tall(path, stem)
    for i, (p, w, h) in enumerate(parts, 1):
        iw = CONTENT_W
        ih = iw * h / w
        if ih > MAX_IMG_H:
            ih = MAX_IMG_H
            iw = ih * w / h
        img = Image(p, width=iw, height=ih)
        img.hAlign = "CENTER"
        out.append(img)
        out.append(Spacer(1, 1.5 * mm))
        if caption:
            label = caption if len(parts) == 1 else f"{caption} (part {i} of {len(parts)})"
            out.append(Paragraph(label, S["caption"]))
    return out


def legacy_table(rows):
    """A small two-column 'legacy mapping' table: legacy form/table -> rebuilt."""
    data = [[Paragraph("Legacy (WinForms / SQL Server)", S["cellb"]),
             Paragraph("Rebuilt in Madhushala Pro", S["cellb"])]]
    for a, b in rows:
        data.append([Paragraph(a, S["cell"]), Paragraph(b, S["cell"])])
    t = Table(data, colWidths=[85 * mm, 85 * mm])
    t.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, 0), PANEL),
        ("LINEBELOW", (0, 0), (-1, -2), 0.4, LINE),
        ("BOX", (0, 0), (-1, -1), 0.5, LINE),
        ("VALIGN", (0, 0), (-1, -1), "TOP"),
        ("LEFTPADDING", (0, 0), (-1, -1), 7),
        ("RIGHTPADDING", (0, 0), (-1, -1), 7),
        ("TOPPADDING", (0, 0), (-1, -1), 4),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 4),
    ]))
    return t



# ==========================================================================
# CONTENT
# ==========================================================================

FLOWS = [
    # ---------------------------------------------------------------- admin
    dict(section="Part A - Getting started", group=None, title="Signing in",
         role="Admin and User",
         route="Start-up screen",
         purpose="Open a session. Everything except the sign-in screen is locked until a user signs in.",
         steps=[
             "Start the application (or open the app URL). The sign-in screen appears.",
             "Type your <b>User Code</b> (for example <font face='Mono'>A00001</font>) and your <b>Password</b> (PIN).",
             "Select <b>Sign in</b>.",
             "On success the shell opens: the sidebar menu on the left, the page area on the right, and the current company and year shown in the top bar.",
         ],
         notes=[
             "The shipped administrator is <font face='Mono'>A00001</font> with password <font face='Mono'>admin</font>. A second demo account, <font face='Mono'>A00002</font> (Cashier), is a non-admin user.",
             "A wrong user code or password shows an inline error and keeps you on the sign-in screen.",
         ]),
    dict(section="Part A - Getting started", group=None, title="Signing out",
         role="Admin and User", route="Sidebar footer - Log out",
         purpose="End the session and return to the sign-in screen.",
         steps=[
             "Select <b>Log out</b> at the bottom of the sidebar.",
             "The session is cleared and the sign-in screen returns.",
         ],
         notes=["Always sign out on a shared counter so the next operator signs in with their own code."]),
    dict(section="Part A - Getting started", group=None, title="Changing your own PIN",
         role="Admin and User", route="Tools - Change Password",
         purpose="Set a new password (PIN) for the account that is currently signed in.",
         steps=[
             "Open <b>Tools - Change Password</b>.",
             "Type the <b>New PIN</b>, then repeat it in <b>Confirm PIN</b>.",
             "Select the save button. A confirmation toast appears.",
         ],
         notes=["The two PIN fields must match, otherwise the change is refused.",
                "If you have lost the administrator PIN entirely, see 'Forgot the admin PIN' in Part F."]),
    dict(section="Part A - Getting started", group=None, title="Understanding roles: Admin vs User",
         role="Reference", route="Admin - User (Access Level)",
         purpose="Understand what each access level can do and how the menu changes.",
         steps=[
             "Every account has an <b>Access Level</b>: <b>Admin</b> or <b>User</b>.",
             "<b>Admin</b> sees the full menu, including the whole <b>Admin</b> group (Company, Parameters, User, User Rights, New Year, Excise Audit, Deleted Bill).",
             "<b>User</b> is intended for counter and back-office staff. The module rights below decide what they may do.",
             "Module rights are set per user in <b>Admin - User Rights</b>: for each module (MASTERS, TRANSACTIONS, REPORTS, ADMIN, TOOLS) there are View, Add and Delete flags.",
         ],
         notes=[
             "<b>Important:</b> in the current build the sidebar shows the same menu to both levels and the server does not yet block an API call by rights. The rights you record are the intended policy and are stored, but enforcement is on the roadmap. Treat Admin-level screens as administrator-only by convention.",
         ]),

    # ------------------------------------------------------------ dashboard
    dict(section="Part B - Daily operations", group=None, title="Dashboard (Sales / Stock status)",
         role="Admin and User", route="Dash Board - Sales Status (default page)",
         purpose="See today's headline numbers at a glance: sales, purchases, stock and quick actions.",
         steps=[
             "Sign in. The Dashboard opens by default.",
             "Read the top cards: <b>Sales Bills</b>, <b>Sales Net</b>, <b>Purchase Bills</b>, <b>Purchase Net</b>.",
             "Under <b>Stock</b> read <b>Items</b>, <b>Stock Lines</b> and <b>Stock Value</b>.",
             "Use the <b>Quick actions</b> card: <b>New Sale</b>, <b>Sales Reports</b> or <b>Item Master</b>.",
             "For other views open <b>Dash Board - Stock Status</b>, <b>Purchase Status</b> or <b>Scheme / Spirit-Beer / CS</b>.",
         ],
         notes=["Status screens show a report table plus buttons for the related detailed reports."]),
    dict(section="Part B - Daily operations", group="Sales", title="Billing a sale at the counter (Sale - POS)",
         role="Admin and User", route="Transactions - Sale - POS",
         purpose="The primary revenue flow: raise a bill, take payment, and update stock and accounts.",
         steps=[
             "Open <b>Transactions - Sale - POS</b>.",
             "Set <b>Store / Counter</b> (for example <font face='Mono'>CNT1</font>).",
             "Choose the <b>Sale Type</b>: FL (foreign liquor), CS (country spirit), NAI (non-alcoholic), CASH, CREDIT or RETURN.",
             "Set the <b>Party Ledger</b> (for a walk-in, the default <font face='Mono'>C00001</font> Walk-in Customer).",
             "Set the <b>Date</b> and, if any, the <b>Bill Discount</b>.",
             "Add one line per item: pick the <b>Item</b>, enter <b>Qty</b> and the <b>Rate</b>. Use <b>+ Add line</b> for more items.",
             "Watch the running <b>Total</b> pill update as you type.",
             "Select <b>Save Sale</b>. A dialog shows the new <b>Bill No</b>, <b>Gross</b>, <b>Discount</b> and <b>Net</b>.",
         ],
         notes=[
             "Bill numbers keep the legacy format: a type prefix (FL / CS / NAI) plus a running number, for example <font face='Mono'>FL00001</font>.",
             "Saving writes the bill header and lines, posts the accounting entries (sale plus receipt against the cash or bank ledger) and updates stock.",
             "The bill is also queued for Remote Tally Sync when that feature is switched on (see Part F).",
         ]),
    dict(section="Part B - Daily operations", group="Sales", title="Viewing the Sales Register",
         role="Admin and User", route="Transactions - Sales Register",
         purpose="List saved bills with type, party, gross and net amounts.",
         steps=[
             "Open <b>Transactions - Sales Register</b>.",
             "Read the table: Bill No, Date, Type, Store, Party, Gross, Net.",
             "Use <b>+ New Sale</b> to jump straight to billing.",
         ]),
    dict(section="Part B - Daily operations", group="Sales", title="Day close (refresh the sale summary)",
         role="Admin", route="Daily routine (backend day-close)",
         purpose="Close the day's sales so totals and stock snapshots are finalised.",
         steps=[
             "Confirm all bills for the day are entered and printed.",
             "Run the day-close action for the company and year.",
             "The system stamps the unclosed bills with the close date and recomputes stock.",
         ],
         notes=["Day close is a routine that marks each bill as closed; run it once per business day."]),

    # -------------------------------------------------------------- purchase
    dict(section="Part B - Daily operations", group="Purchases and stock", title="Recording a purchase (goods receipt)",
         role="Admin and User", route="Transactions - Purchase",
         purpose="Enter a supplier bill so stock, supplier dues and purchase reports are up to date.",
         steps=[
             "Open <b>Transactions - Purchase</b>.",
             "Set the <b>Date</b>, the <b>Supplier Ledger</b> (for example <font face='Mono'>S00001</font>) and the <b>Supplier Invoice No</b>.",
             "Add one line per item: <b>Item</b>, <b>Qty</b>, <b>Rate</b>, <b>MRP</b> and <b>Batch</b>.",
             "Select <b>Save Purchase</b>. A dialog shows the transaction number and the net amount.",
         ],
         notes=["Purchases feed stock, the purchase reports and the supplier outstanding balance."]),
    dict(section="Part B - Daily operations", group="Purchases and stock", title="Opening stock",
         role="Admin and User", route="Transactions - Opening Stock",
         purpose="Enter the opening quantity of an item at the start of a year or when an item is first stocked.",
         steps=[
             "Open <b>Transactions - Opening Stock</b>.",
             "Enter the <b>Store</b>, <b>Item Code</b>, <b>Quantity</b> and <b>Date</b>.",
             "Save. The item's on-hand quantity is set for that store.",
         ]),
    dict(section="Part B - Daily operations", group="Purchases and stock", title="Stock transfer between stores",
         role="Admin and User", route="Transactions - Stock Transfer",
         purpose="Move stock from one store or counter to another.",
         steps=[
             "Open <b>Transactions - Stock Transfer</b>.",
             "Set <b>From</b> and <b>To</b> stores and the <b>Date</b>.",
             "Add the items and quantities to move.",
             "Select <b>Save Transfer</b>.",
         ]),
    dict(section="Part B - Daily operations", group="Purchases and stock", title="Recording breakage",
         role="Admin and User", route="Transactions - Breakage",
         purpose="Write off damaged or broken stock.",
         steps=[
             "Open <b>Transactions - Breakage</b>.",
             "Set the <b>Store</b> and <b>Date</b>.",
             "Add the broken items and quantities.",
             "Select <b>Save Breakage</b>. Stock is reduced and the breakage expense is posted.",
         ]),
    dict(section="Part B - Daily operations", group="Purchases and stock", title="Counter / transfer sale",
         role="Admin and User", route="Transactions - Counter / Transfer Sale",
         purpose="Move goods from the main store to a counter as a sale-style transfer.",
         steps=[
             "Open <b>Transactions - Counter / Transfer Sale</b>.",
             "Set <b>From</b>, <b>To</b> and the <b>Date</b>.",
             "Add items and quantities.",
             "Select <b>Save</b>. The transfer appears in the <b>Recent</b> list.",
         ]),
    dict(section="Part B - Daily operations", group="Purchases and stock", title="Transfer receive",
         role="Admin and User", route="Transactions - Transfer Receive",
         purpose="Acknowledge stock received at a counter from a transfer.",
         steps=[
             "Open <b>Transactions - Transfer Receive</b>.",
             "Set <b>From</b>, <b>To</b>, the <b>Date</b>, the <b>Transfer No</b> and the <b>Amount</b>.",
             "Save. The receipt appears in the <b>Recent</b> list.",
         ]),

    # ------------------------------------------------------------ accounts
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="Payment entry",
         role="Admin and User", route="Transactions - Payment Entry",
         purpose="Pay a supplier or expense from cash or bank.",
         steps=[
             "Open <b>Transactions - Payment Entry</b>.",
             "Set the <b>Date</b>, the <b>Head Account</b> (cash or bank ledger) and the <b>Client Account</b> (the supplier or party).",
             "Enter the <b>Amount</b> and a <b>Narration</b>.",
             "Save. The voucher is posted and listed under <b>Recent</b>.",
         ]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="Receipt entry",
         role="Admin and User", route="Transactions - Receipt Entry",
         purpose="Record money received from a customer or party.",
         steps=[
             "Open <b>Transactions - Receipt Entry</b>.",
             "Set the <b>Date</b>, the <b>Head Account</b> (cash or bank) and the <b>Client Account</b> (the customer).",
             "Enter the <b>Amount</b> and a <b>Narration</b>.",
             "Save.",
         ]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="Contra entry (cash / bank)",
         role="Admin and User", route="Transactions - Contra Entry",
         purpose="Move money between cash and bank (deposit or withdrawal).",
         steps=[
             "Open <b>Transactions - Contra Entry</b>.",
             "Set the <b>Date</b>, the <b>From Ledger</b> and the <b>To Ledger</b>, and the <b>Amount</b>.",
             "Save.",
         ]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="Journal entry",
         role="Admin and User", route="Transactions - Journal Entry",
         purpose="Post a manual double-entry adjustment with any number of lines.",
         steps=[
             "Open <b>Transactions - Journal Entry</b>.",
             "Set the <b>Date</b>.",
             "Add lines: a <b>Ledger</b> with a <b>Debit</b> or a <b>Credit</b> amount for each.",
             "Select <b>Post Journal</b>. The journal number is shown in a toast.",
         ],
         notes=["Debits must equal credits; the accounting service rejects an unbalanced journal."]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="Debit note and Credit note",
         role="Admin and User", route="Transactions - Debit Note / Credit Note",
         purpose="Adjust a party's account with a debit or credit note.",
         steps=[
             "Open <b>Transactions - Debit Note</b> (or <b>Credit Note</b>).",
             "Set the <b>Date</b>, the <b>Party Ledger</b>, the <b>Amount</b> and a <b>Narration</b>.",
             "Save. The note is listed under <b>Recent</b>.",
         ]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="On A/C adjustment",
         role="Admin and User", route="Transactions - On A/C Adjustment",
         purpose="Adjust a ledger balance on account without a specific bill.",
         steps=[
             "Open <b>Transactions - On A/C Adjustment</b>.",
             "Set the <b>Date</b>, the <b>Ledger</b>, the <b>Amount</b> and a <b>Narration</b>.",
             "Save.",
         ]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="A/C replace (ledger substitution)",
         role="Admin and User", route="Transactions - A/C Replace",
         purpose="Move a balance from one ledger to another.",
         steps=[
             "Open <b>Transactions - A/C Replace</b>.",
             "Set the <b>Date</b>, the <b>From Ledger</b>, the <b>To Ledger</b> and the <b>Amount</b>.",
             "Save.",
         ]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="BRS entry (bank reconciliation)",
         role="Admin and User", route="Transactions - BRS Entry",
         purpose="Reconcile bank transactions and clear them against the bank statement.",
         steps=[
             "Open <b>Transactions - BRS Entry</b>.",
             "Add a reconciliation line: <b>Ledger</b>, <b>Amount</b>, <b>Date</b> and <b>Reference</b>.",
             "In the <b>Reconciliation</b> table select <b>Clear</b> on a row to mark it cleared.",
             "The <b>UPI Transactions</b> table below lists QR / UPI payments for matching.",
         ]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="QRCode / UPI payment",
         role="Admin and User", route="Transactions - QRCode / UPI Payment",
         purpose="Record a payment taken by QR code or UPI.",
         steps=[
             "Open <b>Transactions - QRCode / UPI Payment</b>.",
             "Set the <b>Date</b>, the <b>Amount</b>, the <b>UPI Ref</b> and the <b>Party</b>.",
             "Save. The payment appears under <b>Recent</b>.",
         ]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="Excise fine",
         role="Admin and User", route="Transactions - Excise Fine",
         purpose="Record an excise fine or penalty against a shop.",
         steps=[
             "Open <b>Transactions - Excise Fine</b>.",
             "Set the <b>Date</b>, the <b>Shop</b>, the <b>Amount</b> and the <b>Reason</b>.",
             "Save. The fine is listed under <b>Recent</b>.",
         ]),
    dict(section="Part B - Daily operations", group="Accounts and vouchers", title="Home delivery orders",
         role="Admin and User", route="Transactions - Home Delivery",
         purpose="Create a delivery order and track it to delivery.",
         steps=[
             "Open <b>Transactions - Home Delivery</b>.",
             "Set the <b>Date</b>, the <b>Customer</b>, the <b>Address</b> and the <b>Amount</b>.",
             "Save. The order appears in the <b>Orders</b> table with a status.",
             "To progress an order, pick a new status (PENDING, OUT, DELIVERED, CANCELLED) in the row and select <b>Set</b>.",
         ]),

    # ------------------------------------------------------------- masters
    dict(section="Part C - Masters", group="Product and party masters",
         title="Adding an item (Item Master)",
         role="Admin and User", route="Masters - Item Master",
         purpose="Create the liquor and non-alcoholic products that billing uses.",
         steps=[
             "Open <b>Masters - Item Master</b>.",
             "Select <b>+ Add Item</b>.",
             "Fill in the item code and name and the packing details (group, category, KFL, strength, ML, packing, barcode).",
             "Enter the sale and purchase rates.",
             "Save. The item appears in the list with category, ML, pack, sale and purchase rates.",
         ],
         notes=[
             "Sample items: <font face='Mono'>I00001</font> Royal Stag 750ml, <font face='Mono'>I00002</font> Blenders Pride 750ml, <font face='Mono'>I00003</font> Kingfisher Beer 650ml.",
             "Items are used by Sale - POS, Purchase, stock flows and the reports.",
         ]),
    dict(section="Part C - Masters", group="Product and party masters",
         title="Maintaining other masters (generic add / edit / delete)",
         role="Admin and User", route="Masters - all master screens",
         purpose="Every master screen works the same way: a table with Add, Edit and Del.",
         steps=[
             "Open any master, for example <b>Masters - Ledger</b>, <b>Category</b>, <b>KFL</b>, <b>Manufacturer</b>, <b>Strength</b>, <b>Measure and Packing</b>, <b>Sales Rate</b>, <b>Store Master</b>, <b>Counter Master</b>, <b>Customer Master</b>, <b>Area</b>, <b>Tax Schemes</b>, <b>Excise Item</b>, <b>Excise Report Tagging</b>, or an <b>NAI</b> master.",
             "Select <b>+ Add</b> to create a record and fill the form.",
             "Select <b>Edit</b> on a row to change it, or <b>Del</b> to remove it.",
         ],
         notes=[
             "Masters must be set up before the transactions that depend on them: ledgers before vouchers, counters and items before billing.",
         ]),
    dict(section="Part C - Masters", group="Product and party masters",
         title="Schemes and offers",
         role="Admin and User", route="Masters - Scheme / Offer",
         purpose="Set up buy-one-get-one style schemes and percentage offers that POS can apply.",
         steps=[
             "Open <b>Masters - Scheme / Offer</b>.",
             "The screen lists <b>Schemes</b> and <b>Offers</b>.",
             "Maintain them from the master list (add / edit / delete).",
         ],
         notes=["Sample: scheme <font face='Mono'>SC1</font> 'Beer 1+1' and offer <font face='Mono'>OF1</font> 'Whisky 5% off'."]),

    # ------------------------------------------------------------- reports
    dict(section="Part C - Masters", group="Reports", title="Running a report",
         role="Admin and User", route="Reports - any group",
         purpose="Open any of the 113 reports across eight groups.",
         steps=[
             "Open <b>Reports</b> and pick a group: Sales, Stock, Purchase, Accounts, MIS, Breakage, Stock Transfer or Excise.",
             "A group shows a button per report with the number of reports in each group.",
             "Select a report title. The report runs and shows a table with a row count.",
             "Select <b>Back</b> to return to the group list.",
         ],
         notes=[
             "Sales Reports (32), Stock (20), Purchase (10), Accounts (14), MIS (12), Breakage (2), Stock Transfer (2), Excise (21) - 113 in total.",
             "Examples: Sales Statement, Item Wise Sales, P/L Account, Balance Sheet, Trial Balance, Daily Stock Book, GST reports.",
         ]),

    # --------------------------------------------------------- admin screens
    dict(section="Part D - Administration", group="Company setup", title="Company profile",
         role="Admin", route="Admin - Company",
         purpose="Maintain the company name, address and statutory numbers.",
         steps=[
             "Open <b>Admin - Company</b>.",
             "Edit the fields shown (name, address, city, phone, e-mail, TIN / CST / PAN / VAT / ST numbers).",
             "Save.",
         ]),
    dict(section="Part D - Administration", group="Company setup", title="Company parameters",
         role="Admin", route="Admin - Parameters",
         purpose="Set defaults that drive accounting heads, printing, tax and POS behaviour.",
         steps=[
             "Open <b>Admin - Parameters</b>.",
             "Review the parameter list: sale account head, cash / bank ledgers, breakage and sales-return heads, purchase account, discount accounts, rate code, expense and round-off heads, POS mode, print mode, surcharge, language, SMS flags, credit limit.",
             "Change the values you need and select <b>Save Parameters</b>.",
         ],
         notes=["These heads point at ledgers, so set the ledgers up first."]),
    dict(section="Part D - Administration", group="Company setup", title="Creating a new financial year",
         role="Admin", route="Admin - New Year",
         purpose="Open a new year and carry forward opening balances.",
         steps=[
             "Open <b>Admin - New Year</b>.",
             "Enter the <b>Year Range</b> (for example 2025-2026), the <b>Start Date</b>, the <b>End Date</b> and the <b>Year Code</b>.",
             "Save. The new year appears in the <b>Existing Years</b> table.",
         ],
         notes=["The current year is shown as a pill in the top bar."]),
    dict(section="Part D - Administration", group="Users and access", title="Adding a user",
         role="Admin", route="Admin - User",
         purpose="Create an account for a staff member and set its access level.",
         steps=[
             "Open <b>Admin - User</b>.",
             "Select <b>+ Add User</b>.",
             "Enter the <b>User Code</b> and <b>Name</b>, choose the <b>Access Level</b> (User or Admin) and set a <b>PIN</b>.",
             "Save. The user appears in the list with their level badge.",
         ]),
    dict(section="Part D - Administration", group="Users and access", title="Resetting a user's PIN",
         role="Admin", route="Admin - User - Reset PIN",
         purpose="Set a new PIN for a user who has forgotten theirs.",
         steps=[
             "Open <b>Admin - User</b>.",
             "On the user's row select <b>Reset PIN</b>.",
             "Type the <b>New PIN</b> and confirm it, then save.",
         ],
         notes=["A user can also change their own PIN from Tools - Change Password."]),
    dict(section="Part D - Administration", group="Users and access", title="Setting user rights",
         role="Admin", route="Admin - User Rights",
         purpose="Record what each user may view, add and delete per module.",
         steps=[
             "Open <b>Admin - User</b> and select <b>Rights</b> on a user's row.",
             "For each module (MASTERS, TRANSACTIONS, REPORTS, ADMIN, TOOLS) set the <b>View</b>, <b>Add</b> and <b>Delete</b> flags (Y or N).",
             "Select <b>Save Rights</b>.",
         ],
         notes=["See the roles section in Part A for the current enforcement status of these flags."]),

    # -------------------------------------------------------------- tools
    dict(section="Part E - Tools and utilities", group="Everyday tools", title="Calculator",
         role="Admin and User", route="Tools - Calculator",
         purpose="A simple on-screen calculator.",
         steps=["Open <b>Tools - Calculator</b>.", "Type digits, then use +, -, x or / to apply an operation."]),
    dict(section="Part E - Tools and utilities", group="Everyday tools", title="SMS / WhatsApp messages",
         role="Admin and User", route="Tools - SMS / WhatsApp",
         purpose="Queue a text or WhatsApp message and review the outbox.",
         steps=[
             "Open <b>Tools - SMS / WhatsApp</b>.",
             "Enter the <b>To</b> number and the <b>Message</b>.",
             "Save. The message is queued and listed in the <b>Outbox</b>.",
         ]),
    dict(section="Part E - Tools and utilities", group="Everyday tools", title="PML print and reports",
         role="Admin and User", route="Tools - PML Print and Reports",
         purpose="Record PML (bottles and ML) entries and review them.",
         steps=[
             "Open <b>Tools - PML Print and Reports</b>.",
             "Enter the <b>Date</b>, <b>Bottles</b>, <b>ML</b> and <b>Remarks</b>.",
             "Save. The entry appears in the <b>Entries</b> table.",
         ]),
    dict(section="Part E - Tools and utilities", group="Everyday tools", title="Clean print garbage",
         role="Admin and User", route="Tools - Clean Print Garbage",
         purpose="Remove orphaned print records for the current company.",
         steps=["Open <b>Tools - Clean Print Garbage</b>.", "Select <b>Run Cleanup</b>."]),
    dict(section="Part E - Tools and utilities", group="Data and maintenance", title="Data back-up",
         role="Admin", route="Tools - Data Back-Up",
         purpose="Create a compressed backup of the database.",
         steps=[
             "Open <b>Tools - Data Back-Up</b>.",
             "Enter the <b>Destination folder</b>.",
             "Save. A confirmation toast appears when the backup is created.",
         ]),
    dict(section="Part E - Tools and utilities", group="Data and maintenance", title="Data restore",
         role="Admin", route="Tools - Data Restore",
         purpose="Restore the database from a backup file.",
         steps=[
             "Open <b>Tools - Data Restore</b>.",
             "Enter the <b>Backup file path</b>.",
         ],
         notes=["Restore is performed on the server: place the backup on the server and give its path. In this build the guided screen explains the step; the copy itself is done server-side."]),
    dict(section="Part E - Tools and utilities", group="Data and maintenance", title="Data sync (multi-branch / central)",
         role="Admin", route="Tools - Data Sync",
         purpose="Push or pull data to a central server for multi-branch setups.",
         steps=[
             "Open <b>Tools - Data Sync</b>.",
             "Choose the <b>Entity</b> (SALES, PURCHASE, ITEMS, LEDGER) and the <b>Direction</b> (PUSH or PULL).",
             "Select <b>Sync now</b>. The status panel refreshes.",
         ]),
    dict(section="Part E - Tools and utilities", group="Data and maintenance", title="Export to Tally (file export)",
         role="Admin", route="Tools - Export To Tally",
         purpose="Produce a data extract for Tally in one step.",
         steps=[
             "Open <b>Tools - Export To Tally</b>.",
             "Choose the data kind (SALES, PURCHASE or LEDGER).",
             "Select <b>Export</b>. The exported data is shown on screen.",
         ],
         notes=["For the automatic, connectionless exchange with the accountant's Tally Prime, use Remote Tally Sync in Part F instead."]),

    # ---------------------------------------------------------- tally sync
    dict(section="Part F - Remote Tally Sync", group=None, title="What Remote Tally Sync does",
         role="Admin (setup) and User (billing)", route="Tools - Remote Tally Sync",
         purpose="Exchange vouchers and masters with the accountant's Tally Prime without a live connection.",
         steps=[
             "The shop agent queues each sales and purchase voucher as it is saved.",
             "When the connection is available the agent pushes the queued vouchers to a cloud broker.",
             "The accountant's Tally Prime pulls the vouchers from the broker.",
             "Ledger and stock-item masters flow back the other way, so the counter keeps the accountant's chart of accounts.",
             "Billing and printing never depend on the connection: pending vouchers simply wait in the local queue.",
         ]),
    dict(section="Part F - Remote Tally Sync", group=None, title="Setting up the sync agent",
         role="Admin", route="Tools - Remote Tally Sync - Agent settings",
         purpose="Connect this shop to the broker once.",
         steps=[
             "Open <b>Tools - Remote Tally Sync</b>.",
             "In <b>Agent settings</b> enter the <b>Broker URL</b>, the <b>Tally company</b>, the <b>Agent ID</b> and the <b>Company GUID</b>.",
             "Select <b>Generate token</b> to create the agent token, then tick <b>Enabled</b>.",
             "Select <b>Save</b>. The status cards and the <b>Last pull</b> / <b>Last push</b> line update.",
         ],
         notes=["With the feature disabled no vouchers are queued and billing is unaffected."]),
    dict(section="Part F - Remote Tally Sync", group=None, title="Queueing and delivering vouchers",
         role="Admin", route="Tools - Remote Tally Sync - Outbound queue",
         purpose="Send saved vouchers to the broker and track delivery.",
         steps=[
             "Read the status cards: <b>Pending</b>, <b>Sent</b>, <b>Delivered</b>, <b>Failed</b>, <b>Total</b>.",
             "Select <b>Queue pending bills</b> to add any un-queued vouchers.",
             "Select <b>Preview Tally XML</b> to see the exact payload that will be sent.",
             "The <b>Outbound queue</b> table shows each voucher: type, document number and date, amount, tries, status and last error.",
             "Select <b>Retry failed</b> to requeue anything that failed.",
         ],
         notes=["Statuses: PENDING (waiting), SENT (handed to the broker), ACKED (delivered), FAILED (needs a retry)."]),
    dict(section="Part F - Remote Tally Sync", group=None, title="Ledger and tax auto-mapping",
         role="Admin", route="Tools - Remote Tally Sync - Ledger / tax auto-mapping",
         purpose="Tell the sync how POS values map to Tally masters.",
         steps=[
             "In <b>Ledger / tax auto-mapping</b> choose a <b>Type</b> (LEDGER, ITEM, TAX or PAYMENT).",
             "Enter the <b>POS pattern</b> (matched against the POS code or name) and the <b>Tally master</b> to map it to.",
             "Select <b>Save</b>. The rule is listed with its priority and active flag.",
             "Select <b>Delete</b> on a rule to remove it.",
         ]),
    dict(section="Part F - Remote Tally Sync", group=None, title="Applying masters received from Tally",
         role="Admin", route="Tools - Remote Tally Sync - Masters received from Tally",
         purpose="Bring the accountant's ledgers and stock items into the counter.",
         steps=[
             "In <b>Masters received from Tally</b> review each incoming master: type, code, name, status and received time.",
             "For a row with status <b>PENDING</b>, select <b>Apply</b> to upsert it into the local masters.",
             "The status changes to <b>APPLIED</b> and the <b>Sync log</b> records the action.",
         ]),
    dict(section="Part F - Remote Tally Sync", group=None, title="Reading the sync log",
         role="Admin", route="Tools - Remote Tally Sync - Sync log",
         purpose="Audit what the agent sent and received.",
         steps=["Open <b>Tools - Remote Tally Sync</b> and scroll to <b>Sync log</b>.",
               "Read when, the direction, the action, the status and the message for each entry."]),
    dict(section="Part F - Remote Tally Sync", group="State excise", title="State excise compliance sync",
         role="Admin (setup) and User (billing)",
         route="Tools - Excise Sync",
         purpose="Queue every sale line for the state excise portal by its excise identity, upload when "
                 "online, push the daily closing balance, and apply indents / transport permits received "
                 "from the portal. Billing never depends on the network.",
         steps=[
             "Open <b>Tools - Excise Sync</b>.",
             "In <b>State settings</b>, pick the <b>State</b> (for example West Bengal, which uses the direct EODB API, or Odisha, which uses the portal agent), then enter the <b>Licensee ID</b>, <b>Endpoint base</b> and <b>Credential</b>, and save.",
             "The status cards show the queue: <b>Pending</b>, <b>Sent to portal</b>, <b>Uploaded</b>, <b>Failed</b>, plus <b>Unmatched</b> and <b>Total</b>.",
             "Bills are captured automatically as you post them; to backfill, select <b>Capture pending bills</b>.",
             "Select <b>Upload to portal</b> (API states) or <b>Stage for portal agent</b> (portal states such as Odisha).",
             "Fix any <b>Unmatched</b> lines (GTIN + pack size + MRP no longer match the item master) - they are skipped on upload until corrected.",
             "For the closing balance: pick the <b>Date</b>, select <b>Build closing balance</b>, then <b>Upload closing balance</b>.",
             "For portal states, export the batch with <b>Export batch (JSON)</b> and confirm with <b>Confirm all sent as uploaded</b> once the portal shows the lines accepted.",
             "Review <b>Portal documents received</b> (indents / transport permits) and select <b>Apply</b> to post a document into purchase.",
             "Check the <b>Sync log</b> for what was sent and received.",
         ],
         notes=[
             "Offline-first: every sale and return line is queued locally by excise identity at billing, so printing and billing never wait for the network.",
             "The pre-validation gate compares GTIN + pack size + MRP against the item master before upload; mismatches are held back as unmatched.",
             "West Bengal uses the <font face='Mono'>api</font> adapter (token auth + push); Odisha uses the <font face='Mono'>portal</font> adapter (stage a batch + agent confirm).",
         ]),
    dict(section="Part F - Remote Tally Sync", group="Account recovery", title="Forgot the admin PIN",
         role="Admin", route="Tools - Change Password, Admin - User, or the API",
         purpose="Regain access when the administrator PIN is lost.",
         steps=[
             "If you are already signed in: <b>Tools - Change Password</b> sets a new PIN for your own account.",
             "If another admin is signed in: <b>Admin - User - Reset PIN</b> sets a new PIN for any user.",
             "If nobody can sign in, reset it on the server with one command (it uses the same database as the app):",
             "<font face='Mono'>node scripts/reset_admin_pin.js A00001 1234</font>",
         ],
         notes=[
             "The script above resets the PIN to <font face='Mono'>1234</font> for user <font face='Mono'>A00001</font>; pass a different user code and PIN if you need to.",
             "You can also call the API directly: <font face='Mono'>curl -X POST localhost:7100/api/users/A00001/reset-pin -H 'content-type: application/json' -d '{\"pin\":\"1234\"}'</font>",
             "The admin PIN is stored in the <font face='Mono'>users</font> table (plaintext, as in the legacy app), so it can be reset directly if needed.",
         ]),

    # ------------------------------------------------------------- appendix
    dict(section="Part G - Reference", group=None, title="Default accounts and sample data",
         role="Reference", route="Seeded data",
         purpose="Codes you can use to explore every flow.",
         steps=[
             "Sign in as <font face='Mono'>A00001</font> / <font face='Mono'>admin</font> (administrator) or <font face='Mono'>A00002</font> / <font face='Mono'>cashier</font> (user). If the admin PIN has been changed, use the current PIN or reset it - see <b>Forgot the admin PIN</b>.",
             "Items: <font face='Mono'>I00001</font> Royal Stag 750ml, <font face='Mono'>I00002</font> Blenders Pride 750ml, <font face='Mono'>I00003</font> Kingfisher Beer 650ml, <font face='Mono'>I00004</font> Absolut Vodka 750ml, <font face='Mono'>I00005</font> Magic Moments 750ml, <font face='Mono'>I00006</font> Budweiser 650ml.",
             "Ledgers: cash <font face='Mono'>L00001</font>, bank <font face='Mono'>L00002</font>, sales <font face='Mono'>L00003</font>, purchase <font face='Mono'>L00004</font>; supplier <font face='Mono'>S00001</font>; walk-in customer <font face='Mono'>C00001</font>.",
             "Stores: <font face='Mono'>MAIN</font> (main store), <font face='Mono'>CNT1</font> (counter 1) and <font face='Mono'>BAR1</font> (main bar).",
             "Year: 2024-2025. Company 1.",
         ]),
    dict(section="Part G - Reference", group=None, title="Getting help",
         role="Admin and User", route="Help menu",
         purpose="Where to look for help inside the app.",
         steps=[
             "Open <b>Help - Ask for Help</b> for the key flow pointers.",
             "Open <b>Help - About Madhushala</b> for the product and stack summary.",
             "Open <b>Help - List of Updates</b> for the recent change list.",
         ]),
]


# ==========================================================================
# SCREENSHOT MAP - one entry per flow title, in the order shown.
#   title -> list of (screenshot stem, caption)
# Captured by: node scripts/capture_screenshots.js
# ==========================================================================
SHOTS_FOR = {
    "Signing in": [("__login__", "The sign-in screen. Enter your User Code and PIN, then Sign in.")],
    "Signing out": [("shell-dashboard", "The dashboard shell; Log out sits at the bottom of the sidebar.")],
    "Changing your own PIN": [("tool-change-password", "Tools - Change Password.")],
    "Dashboard (Sales / Stock status)": [
        ("dashboard", "Dashboard: today's sales, purchases, stock value and top items."),
        ("dash-stock", "Dashboard - Stock status."),
        ("dash-purchase", "Dashboard - Purchase status."),
    ],
    "Billing a sale at the counter (Sale - POS)": [
        ("sale-pos", "Sale - POS. Pick items on the left; the bill totals update on the right."),
        ("sales-register", "Sales Register lists every bill you just posted."),
    ],
    "Viewing the Sales Register": [("sales-register", "Sales Register.")],
    "Day close (refresh the sale summary)": [("sales-register", "Day close operates over the bills shown in the register.")],
    "Recording a purchase (goods receipt)": [("purchase", "Purchase entry (goods receipt).")],
    "Opening stock": [("opening-stock", "Opening stock for the financial year.")],
    "Stock transfer between stores": [("stock-transfer", "Stock transfer from one store to another.")],
    "Recording breakage": [("breakage", "Breakage entry.")],
    "Counter / transfer sale": [("transfer-sale", "Counter / transfer sale.")],
    "Transfer receive": [("transfer-receive", "Transfer receive.")],
    "Payment entry": [("voucher-payment", "Payment entry.")],
    "Receipt entry": [("voucher-receipt", "Receipt entry.")],
    "Contra entry (cash / bank)": [("voucher-contra", "Contra entry (cash / bank).")],
    "Journal entry": [("voucher-journal", "Journal entry.")],
    "Debit note and Credit note": [("note-debit", "Debit note."), ("note-credit", "Credit note.")],
    "On A/C adjustment": [("on-account", "On A/C adjustment.")],
    "A/C replace (ledger substitution)": [("account-replace", "A/C replace.")],
    "BRS entry (bank reconciliation)": [("brs", "Bank reconciliation statement (BRS).")],
    "QRCode / UPI payment": [("upi", "QRCode / UPI payment status.")],
    "Excise fine": [("fine", "Excise fine entry.")],
    "Home delivery orders": [("delivery", "Home delivery orders.")],
    "Adding an item (Item Master)": [("items", "Item Master with the demo catalogue.")],
    "Maintaining other masters (generic add / edit / delete)": [
        ("master-ledger", "Ledger master."),
        ("master-category", "Category master."),
        ("master-customer", "Customer master."),
    ],
    "Schemes and offers": [("schemes", "Schemes and offers.")],
    "Running a report": [
        ("reports-sales", "Reports - Sales group."),
        ("reports-stock", "Reports - Stock group."),
    ],
    "Company profile": [("master-company", "Company profile.")],
    "Company parameters": [("admin-parameters", "Company parameters.")],
    "Creating a new financial year": [("admin-new-year", "Create a new financial year.")],
    "Adding a user": [("admin-users", "Admin - User.")],
    "Resetting a user's PIN": [("admin-users", "Reset PIN from Admin - User.")],
    "Setting user rights": [("admin-user-rights", "User rights per module.")],
    "Calculator": [("tool-calculator", "Calculator.")],
    "SMS / WhatsApp messages": [("tool-messages", "SMS / WhatsApp messages.")],
    "PML print and reports": [("tool-pml", "PML print and reports.")],
    "Clean print garbage": [("tool-clean-print", "Clean print garbage.")],
    "Data back-up": [("tool-backup", "Data back-up.")],
    "Data restore": [("tool-restore", "Data restore.")],
    "Data sync (multi-branch / central)": [("tool-data-sync", "Data sync.")],
    "Export to Tally (file export)": [("tool-tally-export", "Export to Tally (file export).")],
    "What Remote Tally Sync does": [("tool-tally-sync", "Remote Tally Sync - connectionless twin-agent sync.")],
    "Setting up the sync agent": [("tool-tally-sync", "Agent settings, broker URL and company GUID.")],
    "Queueing and delivering vouchers": [("tool-tally-sync", "Outbound queue and delivery.")],
    "Ledger and tax auto-mapping": [("tool-tally-sync", "Ledger and tax auto-mapping.")],
    "Applying masters received from Tally": [("tool-tally-sync", "Masters received from Tally.")],
    "Reading the sync log": [("tool-tally-sync", "Sync log.")],
    "Excise compliance sync (state portal)": [("tool-excise-sync", "State excise compliance sync.")],
    "Forgot the admin PIN": [("tool-change-password", "Change Password, or reset the PIN for any user from Admin - User.")],
    "Getting help": [("help-ask", "Help - Ask for Help."), ("help-about", "Help - About Madhushala Pro.")],
}

# ==========================================================================
# LEGACY MAPPING - the original WinForms form / SQL Server table behind each
# flow, and where it lives in the rebuild. Sources: PRODUCT_FEATURES.md,
# decode/ (decompiled sources) and dev/db/ddl.sql.
#   title -> list of (legacy, rebuilt)
# ==========================================================================
LEGACY_FOR = {
    "Signing in": [("frm_login / frm_sign_in (users table)", "Login screen -> POST /api/login; users table")],
    "Signing out": [("frm_main session close", "Sidebar footer - Log out")],
    "Changing your own PIN": [("frm_userinfo self-edit", "Tools - Change Password")],
    "Dashboard (Sales / Stock status)": [("frm_main status panels", "Dashboard route; GET /api/dashboard")],
    "Billing a sale at the counter (Sale - POS)": [
        ("frm_countersales", "Sale - POS"),
        ("salesbillmain + salesbilldetail", "sales.createSale() -> salesbillmain / salesbilldetail"),
    ],
    "Viewing the Sales Register": [("frm_bedit / sales grid", "Sales Register (GET /api/sales)")],
    "Day close (refresh the sale summary)": [("frm_manualdayclose", "POST /api/day-close")],
    "Recording a purchase (goods receipt)": [
        ("frm_purchase", "Purchase"),
        ("purchasemain + purchasedetail", "purchase.createPurchase()"),
    ],
    "Opening stock": [("openingstockmst", "Opening Stock")],
    "Stock transfer between stores": [("frm_stocktransfer / stk_transfer_main", "Stock Transfer")],
    "Recording breakage": [("frm_breakageentry / breakagemain", "Breakage")],
    "Counter / transfer sale": [("frm_countersalesTransfer / frm_TransferSale", "Counter / Transfer Sale")],
    "Transfer receive": [("frm_TransferReceive", "Transfer Receive")],
    "Payment entry": [("frm_conpayrec_vch / payment_main", "Payment Entry")],
    "Receipt entry": [("frm_receipt_voucher / receipt_main", "Receipt Entry")],
    "Contra entry (cash / bank)": [("frm_contra_entry", "Contra Entry")],
    "Journal entry": [("frm_journal_vch / journaldetail", "Journal Entry")],
    "Debit note and Credit note": [("notes_main / notes_detail", "Debit / Credit Note")],
    "On A/C adjustment": [("frm_OnAccountAdjustment", "On A/C Adjustment")],
    "A/C replace (ledger substitution)": [("frm_AccountTransfer", "A/C Replace")],
    "BRS entry (bank reconciliation)": [("frm_reconciliation / TransactionReconciliation", "BRS Entry")],
    "QRCode / UPI payment": [("frm_qrorder / tbl_UPIRespose", "QRCode / UPI Payment")],
    "Excise fine": [("frm_fine / fine_details", "Excise Fine")],
    "Home delivery orders": [("delivery_orders", "Home Delivery")],
    "Adding an item (Item Master)": [("frm_itemmstadd / itemmst", "Item Master")],
    "Maintaining other masters (generic add / edit / delete)": [("frm_masters / frm_ledger / frm_unit / Frm_ManufacturerMaster", "Generic master screens")],
    "Schemes and offers": [("frm_scheme / tbl_scheme, offermst", "Schemes and offers")],
    "Running a report": [("frm_ReportForm / frm_ReportsPreview", "Reports (113 declarative reports)")],
    "Company profile": [("frm_companyinfo / companymast", "Company")],
    "Company parameters": [("frm_companyparameter / companyparamtr", "Parameters")],
    "Creating a new financial year": [("frm_yearcreation / yearmst", "New Year")],
    "Adding a user": [("frm_userinfo / users", "Admin - User")],
    "Resetting a user's PIN": [("users.password (plaintext)", "Admin - User - Reset PIN / POST /api/users/:code/reset-pin")],
    "Setting user rights": [("frm_userrights / userrights", "User Rights")],
    "Calculator": [("tools.calculate", "Calculator")],
    "SMS / WhatsApp messages": [("frm_message / tbl_MessageLog", "SMS / WhatsApp")],
    "PML print and reports": [("tbl_PML / tbl_PMLCAL", "PML Print and Reports")],
    "Clean print garbage": [("legacy print-clean utility", "Clean Print Garbage")],
    "Data back-up": [("backup_upload company parameter", "Data Back-Up")],
    "Data restore": [("frm_restore", "Data Restore")],
    "Data sync (multi-branch / central)": [("AutoDataSync.exe / tbl_UploadDownloadStatus", "Data Sync")],
    "Export to Tally (file export)": [("frm_tally_export", "Export to Tally")],
    "What Remote Tally Sync does": [("MadhushalaDataSync.dll", "Remote Tally Sync")],
    "Setting up the sync agent": [("MadhushalaDataSync.dll config", "Remote Tally Sync - Agent settings")],
    "Queueing and delivering vouchers": [("tbl_TallySyncQueue", "Remote Tally Sync - Outbound queue")],
    "Ledger and tax auto-mapping": [("tbl_TallyMapping", "Remote Tally Sync - Ledger / tax auto-mapping")],
    "Applying masters received from Tally": [("inbound masters", "Remote Tally Sync - Masters received")],
    "Reading the sync log": [("sync log", "Remote Tally Sync - Sync log")],
    "Excise compliance sync (state portal)": [("frm_exciseform / ExciseSalesUpload.exe", "Excise Sync (WB api mode + Odisha portal mode)")],
    "Forgot the admin PIN": [("users.password (plaintext)", "Change Password / Reset PIN / scripts/reset_admin_pin.js")],
    "Getting help": [("frm_ask4help / frm_AboutBox1", "Help menu")],
}


# ==========================================================================
# STORY
# ==========================================================================
def cover_story():
    return [
        Spacer(1, 62 * mm),
        P("Madhushala Pro", "cover_title"),
        Spacer(1, 3 * mm),
        P("User Guide - Admin and User Flows", "cover_sub"),
        Spacer(1, 8 * mm),
        Table([[Paragraph(
            "Bar and Liquor POS and management system. This guide walks through every "
            "screen and business flow for both the administrator and the counter / "
            "back-office user, from signing in and billing a sale to masters, reports, "
            "administration, state excise compliance and the connectionless Remote "
            "Tally Sync. Every flow is illustrated with a screenshot of the rebuilt "
            "application and a map back to the original WinForms form and SQL Server "
            "table.",
            S["cover_meta"])]], colWidths=[150 * mm],
            style=TableStyle([("LEFTPADDING", (0, 0), (-1, -1), 0),
                              ("RIGHTPADDING", (0, 0), (-1, -1), 0),
                              ("TOPPADDING", (0, 0), (-1, -1), 0),
                              ("BOTTOMPADDING", (0, 0), (-1, -1), 0)])),
        Spacer(1, 12 * mm),
        Paragraph(f"Version {VERSION} &nbsp;&bull;&nbsp; {BUILD_DATE}", S["cover_meta"]),
        Spacer(1, 2 * mm),
        Paragraph("Prepared for: shop owners, counter operators and accountants",
                  S["cover_meta"]),
    ]


def build_story(doc):
    story = []
    story += cover_story()
    story.append(NextPageTemplate("body"))
    story.append(PageBreak())

    story.append(P("Contents", "h1"))
    story.append(rule(BRAND, 1.0, 6))
    toc = TableOfContents()
    toc.levelStyles = [S["toc0"], S["toc1"], S["toc2"]]
    toc.dotsMinLevel = 0
    story.append(toc)
    story.append(PageBreak())

    story.append(toc_para("How to use this guide", 0))
    story.append(rule(BRAND, 1.0, 6))
    story.append(P("Every flow is written the same way: a small table of <b>who</b> can do it, "
                   "<b>where</b> it lives in the menu, and <b>what</b> it is for, followed by "
                   "numbered steps and any notes."))
    story.append(Spacer(1, 3 * mm))
    story.append(callout("Roles in this guide", [
        "<b>Admin</b> - full menu, including company setup, users, rights and all tools.",
        "<b>User</b> (non-admin) - counter and back-office work: billing, purchases, stock, vouchers, reports and everyday tools.",
        "Screens are grouped into Parts A to G; the menu path is shown for each flow.",
    ], accent=ACCENT, bg=colors.HexColor("#eef2ff")))
    story.append(Spacer(1, 3 * mm))
    story.append(callout("Conventions", [
        "Menu paths use a dash, for example <b>Tools - Change Password</b>.",
        "Values you type or pick are shown in monospace, for example <font face='Mono'>CNT1</font>.",
        "Money is shown in rupees.",
    ]))
    story.append(Spacer(1, 4 * mm))
    story.append(toc_para("Screenshots and the demo dataset", 0))
    story.append(rule(BRAND, 1.0, 6))
    story.append(P("Every flow below is shown with a real screenshot of the rebuilt application, "
                   "captured from a running copy with the demo dataset loaded. Each flow also "
                   "carries a small <b>legacy mapping</b> table that names the original WinForms "
                   "form and SQL Server table it came from, so anyone who knows the old "
                   "Madhushala Pro product can find their way immediately."))
    story.append(Spacer(1, 3 * mm))
    story.append(P("Load the demo dataset before you explore (it resets the database):", "body"))
    story.append(Paragraph("node scripts/seed_demo.js", S["code"]))
    story.append(Spacer(1, 2 * mm))
    story.append(callout("What the demo dataset contains", [
        "Company <font face='Mono'>1</font>, year <font face='Mono'>2024</font> (2024-2025).",
        "Users: <font face='Mono'>A00001</font> / <font face='Mono'>admin</font> (Admin), "
        "<font face='Mono'>A00002</font> / <font face='Mono'>cashier</font> (User), "
        "<font face='Mono'>A00003</font> / <font face='Mono'>store</font> (User).",
        "Six items (Royal Stag, Blenders Pride, Kingfisher Beer, Absolut Vodka, Magic Moments, "
        "Budweiser), three stores (<font face='Mono'>MAIN</font>, <font face='Mono'>CNT1</font>, "
        "<font face='Mono'>BAR1</font>).",
        "3 purchases, 7 sales bills (net <font face='Mono'>Rs 39,300</font>), a stock transfer, "
        "breakage, payment / receipt / contra / journal vouchers, notes, fine, BRS, UPI and a "
        "delivery order.",
        "Remote Tally Sync configured with 10 queued vouchers (9 delivered, 1 failed); state "
        "excise sync configured for West Bengal with 11 captured bills and a closing balance.",
    ]))
    story.append(Spacer(1, 3 * mm))
    story.append(callout("Forgot the admin PIN?", [
        "Reset it in one command against the running database:",
        "<font face='Mono'>node scripts/reset_admin_pin.js A00001 1234</font>",
        "See the flow <b>Forgot the admin PIN</b> in Part F for the signed-in and server options.",
    ], accent=ACCENT, bg=colors.HexColor("#eef2ff")))

    seen_section = None
    seen_group = None
    for f in FLOWS:
        block = []
        if f["section"] != seen_section:
            seen_section = f["section"]
            seen_group = None
            block.append(PageBreak())
            block.append(toc_para(f["section"], 0))
            block.append(rule(BRAND, 1.0, 6))
        if f["group"] and f["group"] != seen_group:
            seen_group = f["group"]
            block.append(toc_para(f["group"], 1))
            block.append(Spacer(1, 1 * mm))
        head = [toc_para(f["title"], 2), Spacer(1, 1.5 * mm)]
        meta = meta_table([("Who", f["role"]), ("Where", f["route"]), ("What for", f["purpose"])])
        head.append(meta)
        head.append(Spacer(1, 3 * mm))
        block.append(KeepTogether(head))
        block.append(steps_table(f["steps"]))
        if f.get("notes"):
            block.append(Spacer(1, 2 * mm))
            block.append(callout("Notes", f["notes"]))
        legacy = f.get("legacy") or LEGACY_FOR.get(f["title"])
        if legacy:
            block.append(Spacer(1, 3 * mm))
            block.append(Paragraph("Where this lives: legacy form / table &rarr; rebuilt screen",
                                   S["cellb"]))
            block.append(Spacer(1, 1 * mm))
            block.append(legacy_table(legacy))
        shots = SHOTS_FOR.get(f["title"])
        if shots:
            for stem, cap in shots:
                block.append(Spacer(1, 3 * mm))
                block += screenshot(stem, cap)
        block.append(Spacer(1, 7 * mm))
        story += block
    return story


def main():
    doc = GuideDoc(OUT)
    story = build_story(doc)
    doc.multiBuild(story)
    print(f"Wrote {OUT}")


if __name__ == "__main__":
    main()
