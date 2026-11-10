#!/usr/bin/env python3
"""Build the VanijLiquor Customer Portal user guide PDF.

One guide is produced, illustrated with real screenshots of every portal screen
and a per-feature "where this lives" table (portal screen -> API routes):

  * VanijLiquor_Customer_Portal_User_Guide.pdf

Content is written from the application's own sources:
  * new_code_portal/web/menu.js       - navigation tree
  * new_code_portal/web/screens.js    - every screen and its fields
  * new_code_portal/web/app.js        - routing / modal / toast
  * new_code_portal/backend/src/*     - services and API behaviour
  * new_code_portal/backend/src/db/*  - schema and seed data (plans, modules)
  * new_code_portal/docs/screenshots/ - real screenshots of every screen

The screenshots are produced by:
    cd new_code_portal && npm start       # run the portal (backend 7200, web 8081)
    npm run seed-demo                     # load the demo dataset
    npm run screenshots                   # capture docs/screenshots/*.png (CDP)

Run:  python3 build_user_guide.py   ->  the PDF
"""

from __future__ import annotations

import os
import sys

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
OUT = os.path.join(HERE, "VanijLiquor_Customer_Portal_User_Guide.pdf")
SHOTS = os.path.join(HERE, "screenshots")

BUILD_DATE = "8 November 2026"
VERSION = "1.0"

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
# Palette (matches the portal design tokens in web/styles.css).
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
                         title="VanijLiquor Customer Portal - User Guide",
                         author="VanijLiquor", **kw)
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
        canvas.drawString(self.leftMargin, y + 2.4 * mm, "VanijLiquor - Customer Portal User Guide")
        canvas.drawRightString(A4[0] - self.rightMargin, y + 2.4 * mm, "Onboarding & Licensing")
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
    data = [[Paragraph(label, S["cellb"]), Paragraph(value, S["cell"])] for label, value in rows]
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
    data = [[Paragraph(str(i), S["cellb"]), Paragraph(it, S["step"])] for i, it in enumerate(items, 1)]
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


def api_table(rows):
    """Two-column table: portal screen/action -> API route(s)."""
    data = [[Paragraph("Portal screen / action", S["cellb"]),
             Paragraph("API route(s)", S["cellb"])]]
    for a, b in rows:
        data.append([Paragraph(a, S["cell"]), Paragraph(b, S["cell"])])
    t = Table(data, colWidths=[62 * mm, 108 * mm])
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


def data_table(header, rows, widths):
    data = [[Paragraph(c, S["cellb"]) for c in header]]
    for r in rows:
        data.append([Paragraph(c, S["cell"]) for c in r])
    t = Table(data, colWidths=widths)
    t.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, 0), PANEL),
        ("LINEBELOW", (0, 0), (-1, -2), 0.4, LINE),
        ("BOX", (0, 0), (-1, -1), 0.5, LINE),
        ("VALIGN", (0, 0), (-1, -1), "TOP"),
        ("LEFTPADDING", (0, 0), (-1, -1), 6),
        ("RIGHTPADDING", (0, 0), (-1, -1), 6),
        ("TOPPADDING", (0, 0), (-1, -1), 4),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 4),
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
    global _SLICE_DIR
    with PILImage.open(path) as im:
        w, h = im.size
        ratio = h / w
        max_ratio = MAX_IMG_H / CONTENT_W
        if ratio <= max_ratio:
            return [(path, w, h)]
        parts = int(ratio / max_ratio) + 1
        step = h // parts
        if _SLICE_DIR is None:
            import tempfile
            _SLICE_DIR = tempfile.mkdtemp(prefix="portal-guide-slices-")
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
        return [Paragraph(f"[screenshot {stem} missing]", S["caption"])]
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


# ==========================================================================
# CONTENT
# ==========================================================================

FEATURES = [
    # ------------------------------------------------------------- overview
    dict(section="Part A - Getting started", group=None, title="What the Customer Portal is",
         who="Everyone",
         where="Open the portal URL in a browser",
         purpose="The portal is the self-service front door for VanijLiquor: a shop onboards "
                 "itself, picks a plan and modules, gets a price quote, and receives a "
                 "machine-bound licence it can activate on the counter PC.",
         steps=[
             "The portal is a web app. Open it in any modern browser (Chrome, Edge, Firefox).",
             "It runs as two small services: a backend API and a web front end that serves the "
             "UI and proxies <font face='Mono'>/api</font> to the backend.",
             "Start everything with <font face='Mono'>npm start</font> from the "
             "<font face='Mono'>new_code_portal</font> folder.",
             "The portal opens on the <b>Dashboard</b>; use the left sidebar to move between screens.",
         ],
         notes=[
             "There is no login on the portal in this build — it is an operator/vendor console. "
             "Put it behind your own authentication (reverse proxy or VPN) before exposing it.",
             "Backend listens on <font face='Mono'>127.0.0.1:7200</font>; the web UI on "
             "<font face='Mono'>0.0.0.0:8081</font>.",
         ],
         api=[("Start the portal", "npm start  (backend :7200 + web :8081)")],
         shots=[("shell-dashboard", "The portal shell: the sidebar menu on the left, the page on the right.")]),

    dict(section="Part A - Getting started", group=None, title="The layout: sidebar, top bar and toasts",
         who="Everyone", where="Any screen",
         purpose="Learn the common furniture so every screen is easy to read.",
         steps=[
             "The <b>sidebar</b> groups the screens: <b>Overview</b> (Dashboard, Businesses, "
             "Licences), <b>Onboard</b> (New Business, Plans &amp; Pricing, Modules), "
             "<b>Licensing</b> (Issue Licence, Activate / Verify) and <b>Resources</b> "
             "(Download App, Licence Key, Audit Log).",
             "The <b>top bar</b> shows the current screen name and a reminder of the three plans.",
             "A <b>toast</b> (a small message, bottom of the screen) confirms or refuses every "
             "action, for example <i>Issued VL-LIC-XXXXXX</i> or an error.",
             "On a narrow window the sidebar collapses behind the <b>&#9776;</b> menu button.",
         ],
         notes=["Selecting a sidebar item changes the URL hash (for example <font face='Mono'>#plans</font>), so a screen can be linked or bookmarked."],
         api=[("Navigation", "client-side routing in web/app.js (hash routes)")],
         shots=[]),

    # ------------------------------------------------------------ dashboard
    dict(section="Part B - Overview", group=None, title="Dashboard",
         who="Everyone", where="Overview - Dashboard",
         purpose="See the health of the whole book of business at a glance: how many shops, "
                 "how many licences, activations and recurring revenue, plus the latest activity.",
         steps=[
             "Open <b>Overview - Dashboard</b>. It is the default screen.",
             "Read the four headline cards: <b>Businesses</b> (with how many are active), "
             "<b>Licences</b> (active / issued), <b>Activations</b> and <b>MRR</b> "
             "(monthly recurring revenue).",
             "Use <b>Quick actions</b> to jump straight to <b>Onboard a business</b>, "
             "<b>Issue a licence</b>, <b>Activate / verify</b> or <b>Download the app</b>.",
             "Read <b>Licence health</b>: issued, active, revoked, expiring within 30 days, "
             "and whether the signing key is production or development.",
             "Scan <b>Businesses by plan</b> for the spread across Basic / Standard / Premium.",
             "Read <b>Recent activity</b> — the last few audit events (business created, "
             "licence issued, licence activated, and so on).",
         ],
         notes=[
             "<b>MRR</b> sums the monthly plan price of every active business, so a yearly "
             "customer still contributes their monthly-equivalent.",
             "If <b>Signing key</b> shows <i>development</i>, the licences are signed with the "
             "built-in dev key — set <font face='Mono'>PORTAL_LICENSE_KEY</font> before "
             "issuing real licences (see Part E).",
         ],
         api=[("Dashboard cards, licence health, activity",
               "GET /api/dashboard")],
         shots=[("dashboard", "Dashboard: businesses, licences, activations, MRR and recent activity.")]),

    # ----------------------------------------------------------- businesses
    dict(section="Part B - Overview", group=None, title="Businesses (search and open)",
         who="Everyone", where="Overview - Businesses",
         purpose="Find a shop by name, code or e-mail, filter by status, and open its record.",
         steps=[
             "Open <b>Overview - Businesses</b>.",
             "Type in <b>Search</b> (matches legal name, code or e-mail) and/or pick a "
             "<b>Status</b>: draft, onboarding, active or suspended.",
             "Select <b>Search</b>. The list shows one card per business with its status badge.",
             "Select <b>Open</b> on a card to see the full record (see the next topic).",
             "Select <b>＋ New business</b> to start the onboarding wizard.",
         ],
         notes=["Every business gets a code in the legacy style <font face='Mono'>VL-YYYY-NNNN</font>, for example <font face='Mono'>VL-2026-0001</font>."],
         api=[("List / search", "GET /api/businesses?q=&status="),
              ("Open one business", "GET /api/businesses/:id")],
         shots=[("businesses", "Businesses: search, status filter and one card per shop.")]),

    dict(section="Part B - Overview", group=None, title="Business details (record, plan and licences)",
         who="Everyone", where="Overview - Businesses - Open",
         purpose="See a single shop's details, its plan and entitled modules, and every licence "
                 "issued to it — and issue a new licence or edit its modules from here.",
         steps=[
             "Open a business from the Businesses list.",
             "Read the <b>Business</b> card: code, legal and trade name, owner, e-mail, phone, "
             "GSTIN, city/state, business type, outlets and status.",
             "Read the <b>Plan &amp; entitlement</b> card: the plan, billing period and the "
             "module chips the shop is entitled to.",
             "Select <b>Issue licence</b> to mint a licence for this shop, or "
             "<b>Edit modules</b> to change its entitlements.",
             "Under <b>Licences</b> read every licence for the shop: id, plan, status, valid-until "
             "date and activation count.",
         ],
         notes=["Entitlements are the plan's bundled modules plus any extra modules you selected. They are copied into every licence you issue."],
         api=[("Business record", "GET /api/businesses/:id"),
              ("Change modules", "POST /api/businesses/:id/modules"),
              ("Licences for this shop", "GET /api/licenses?business_id=:id")],
         shots=[("business-detail", "Business details: record, plan and entitlement, and its licences.")]),

    dict(section="Part B - Overview", group=None, title="Licences (all issued licences)",
         who="Everyone", where="Overview - Licences",
         purpose="See every licence across all shops in one table, open one, or download its "
                 "licence file.",
         steps=[
             "Open <b>Overview - Licences</b>.",
             "Read the table: Licence id, Business, Plan, Status, Machine (first 12 hex digits of "
             "the machine hash), Valid until and Activations.",
             "Each licence also appears as a card below with <b>View</b> (opens the licence "
             "detail) and <b>Download</b> (saves the <font face='Mono'>.vlic</font> file).",
             "Use <b>＋ Issue licence</b> or <b>✓ Activate / verify</b> to jump to those screens.",
         ],
         notes=["Statuses are <b>issued</b> (minted, not yet activated), <b>active</b> (activated on its machine) and <b>revoked</b> (withdrawn)."],
         api=[("List licences", "GET /api/licenses"),
              ("Licence detail", "GET /api/licenses/:id"),
              ("Download .vlic", "GET /api/licenses/:id/file")],
         shots=[("licences", "Licences: every licence, with View and Download on each card.")]),

    # ------------------------------------------------------------- onboard
    dict(section="Part C - Onboard & pricing", group=None, title="Onboard a business (the wizard)",
         who="Everyone", where="Onboard - New Business",
         purpose="Create a new customer in four steps: business details, plan and term, modules, "
                 "then a review with the live price.",
         steps=[
             "<b>Step 1 - Business.</b> Enter the legal name (required), trade name, owner, "
             "e-mail (required), phone (required), GSTIN, PAN, business type, address, city, "
             "district, state, state code, PIN and number of outlets. Select <b>Continue</b>.",
             "<b>Step 2 - Plan.</b> Pick a plan (Basic / Standard / Premium) and a billing term "
             "(Month, 1 Year, 2 Years, 3 Years). The quote box updates as you choose. "
             "Select <b>Continue</b>.",
             "<b>Step 3 - Modules.</b> Tick the modules the shop should have. Modules already in "
             "the plan are pre-marked <i>included</i>; extra modules are quoted as add-ons. "
             "Select <b>Continue</b>.",
             "<b>Step 4 - Review.</b> Check the business summary and the subscription breakdown "
             "(base, add-ons, GST 18%, total), then select <b>Create business</b>.",
             "On success the portal shows <i>Created VL-YYYY-NNNN</i> and opens the new "
             "business's detail page.",
         ],
         notes=[
             "The legal name is required; the wizard will refuse to create without it.",
             "You can go <b>Back</b> at any step; your entries are kept.",
             "The price is a quote only — the portal does not take payment in this build.",
         ],
         api=[("Create the business", "POST /api/businesses"),
              ("Set its modules", "POST /api/businesses/:id/modules"),
              ("Price a quote", "POST /api/quote  (or computed in the wizard)")],
         shots=[
             ("onboard-step1-business", "Step 1 - Business details."),
             ("onboard-step2-plan", "Step 2 - Plan and billing term, with the live quote."),
             ("onboard-step3-modules", "Step 3 - Modules; bundled ones are pre-marked."),
             ("onboard-step4-review", "Step 4 - Review and Create business."),
         ]),

    dict(section="Part C - Onboard & pricing", group=None, title="Plans & Pricing",
         who="Everyone", where="Onboard - Plans & Pricing",
         purpose="Show the three plans, their per-term prices, the feature comparison and the "
                 "stand-alone add-ons.",
         steps=[
             "Open <b>Onboard - Plans &amp; Pricing</b>.",
             "Use the term selector (Month / 1 Year / 2 Years / 3 Years) to re-price the cards.",
             "Read each plan card: name, tagline, price for the chosen term and its highlights.",
             "Read <b>Compare plans</b> — a feature matrix grouped by category, with a tick per plan.",
             "Read <b>Add-ons</b> — the stand-alone extras and their prices.",
             "Select <b>Choose &lt;plan&gt;</b> to start onboarding with that plan in mind.",
         ],
         notes=["Prices mirror the vendor's public subscriptions page and are stored in the database (the <font face='Mono'>plans</font>, <font face='Mono'>plan_prices</font> and <font face='Mono'>comparison</font> tables), so they can be re-priced without code changes."],
         api=[("Plans, prices, comparison, add-ons", "GET /api/plans, /api/periods, /api/comparison, /api/addons")],
         shots=[("plans", "Plans & Pricing: cards, the comparison matrix and add-ons.")]),

    dict(section="Part C - Onboard & pricing", group=None, title="Modules & Add-ons catalogue",
         who="Everyone", where="Onboard - Modules",
         purpose="See the full module catalogue grouped by category, and edit the modules a "
                 "particular business is entitled to.",
         steps=[
             "Open <b>Onboard - Modules</b> to see every module grouped by category "
             "(Software &amp; Usage, Support &amp; Assistance, Cloud &amp; Connected Access, "
             "Integrations &amp; Advanced Tools).",
             "A module with a price is a stand-alone add-on; a module marked <i>Bundled</i> comes "
             "with a plan.",
             "To change what a shop gets, open the business and select <b>Edit modules</b> "
             "(or use the business-detail page). Tick/untick modules and select "
             "<b>Save modules</b>.",
         ],
         notes=["Editing modules changes the shop's entitlements; the next licence you issue for it will carry the new set."],
         api=[("Module catalogue", "GET /api/modules"),
              ("Add one module", "POST /api/businesses/:id/modules/:code"),
              ("Remove one module", "DELETE /api/businesses/:id/modules/:code"),
              ("Replace the set", "POST /api/businesses/:id/modules")],
         shots=[("modules", "Modules & Add-ons: the full catalogue grouped by category.")]),

    # ------------------------------------------------------------ licensing
    dict(section="Part D - Licensing", group=None, title="Issue a licence",
         who="Everyone", where="Licensing - Issue Licence",
         purpose="Mint a signed <font face='Mono'>.vlic</font> licence bound to one machine's "
                 "fingerprint, for a chosen business and term.",
         steps=[
             "Open <b>Licensing - Issue Licence</b>.",
             "Pick the <b>Business</b>. Its entitled modules are listed below the form.",
             "Optionally set <b>Term (months)</b>; leave blank to use the plan's default term.",
             "Under <b>Machine fingerprint</b>, paste the <font face='Mono'>machine-fingerprint.json</font> "
             "from the counter PC (or type the MAC, CPU id, disk serial, hostname and OS).",
             "Select <b>Issue licence</b>. A dialog shows the new licence id, business, plan, "
             "valid dates, machine hash and entitlements.",
             "Select <b>Download licence file</b> to save the <font face='Mono'>.vlic</font>, or "
             "<b>Download payload only</b> for the unsigned payload JSON.",
         ],
         notes=[
             "The fingerprint must include at least a MAC, CPU id or disk id, otherwise the issue "
             "is refused.",
             "The licence binds to the machine hash, so the same file will not activate on a "
             "different PC (anti-clone).",
             "The licence id looks like <font face='Mono'>VL-LIC-XXXXXX</font>.",
         ],
         api=[("Issue a licence", "POST /api/licenses"),
              ("Download the .vlic", "GET /api/licenses/:id/file")],
         shots=[
             ("issue", "Issue Licence: pick the business, paste the fingerprint, then Issue licence."),
             ("issue-modal", "The issued-licence dialog with the download buttons."),
         ]),

    dict(section="Part D - Licensing", group=None, title="Activate / Verify a licence",
         who="Everyone", where="Licensing - Activate / Verify",
         purpose="Redeem a licence on the machine it is bound to, or check a licence's validity "
                 "and machine match.",
         steps=[
             "Open <b>Licensing - Activate / Verify</b>.",
             "Enter the <b>Licence id</b> (for example <font face='Mono'>VL-LIC-XXXXXX</font>).",
             "Enter <b>This machine</b>'s fingerprint (paste the JSON or type the fields).",
             "Select <b>Activate on this machine</b> to redeem it, or <b>Verify only</b> to check "
             "it without activating.",
             "Read the <b>Result</b>: a green <i>Licence valid</i> / <i>Activated</i> panel, or a "
             "red refusal.",
         ],
         notes=[
             "<b>Activation is refused on a different machine</b> — the message is "
             "<i>Licence is bound to a different machine</i>. This is the anti-clone gate.",
             "Verifying checks the Ed25519 signature, the expiry date and (when a fingerprint is "
             "given) the machine match. The result shows days remaining and entitlements.",
             "An expired licence, or a revoked one, is refused.",
         ],
         api=[("Activate", "POST /api/licenses/:id/activate"),
              ("Verify", "POST /api/licenses/:id/verify"),
              ("Activation history", "GET /api/licenses/:id/activations"),
              ("Revoke / re-sign", "POST /api/licenses/:id/revoke, /api/licenses/:id/resign")],
         shots=[
             ("verify", "Activate / Verify: enter the licence id and this machine's fingerprint."),
             ("verify-success", "Verify only: a valid licence on the correct machine."),
             ("verify-denied", "Activation refused on a different machine (anti-clone)."),
         ]),

    dict(section="Part D - Licensing", group=None, title="Download the App",
         who="Everyone", where="Resources - Download App",
         purpose="Give the counter PC the installer and the fingerprint helper, and explain the "
                 "end-to-end onboarding flow.",
         steps=[
             "Open <b>Resources - Download App</b>.",
             "Download the <b>Windows installer</b> (<font face='Mono'>.exe</font> or "
             "<font face='Mono'>.msi</font>) to run on the counter PC.",
             "Download the <b>fingerprint helper</b> (PowerShell or shell) if you want to collect "
             "the machine fingerprint without the installer.",
             "Follow the <b>How onboarding works</b> steps on the right of the screen.",
         ],
         notes=["In this build the installer files are placeholders — drop your signed installers into <font face='Mono'>web/downloads/</font> to serve them."],
         api=[("Static files", "web/downloads/*  (served by the web front end)")],
         shots=[("download", "Download the App: installers, fingerprint helper and the onboarding steps.")]),

    dict(section="Part D - Licensing", group=None, title="Licence signing key",
         who="Administrator", where="Resources - Licence Key",
         purpose="Show the public key the POS app pins, and whether a production signing key is "
                 "in use.",
         steps=[
             "Open <b>Resources - Licence Key</b>.",
             "Read the badge: <b>production key in use</b>, or a warning that a development key "
             "is in use.",
             "Copy the <b>Public key (SPKI, base64)</b> and pin it in the POS app's build so it "
             "can verify licences offline.",
         ],
         notes=[
             "Set <font face='Mono'>PORTAL_LICENSE_KEY</font> (the private Ed25519 seed) in the "
             "backend environment for production. Keep it secret and server-side only.",
             "Without it, the portal signs with a development key and flags licences as "
             "non-production.",
         ],
         api=[("Public key + production flag", "GET /api/license-key")],
         shots=[("licence-key", "Licence Key: the pinned public key and the key-mode badge.")]),

    dict(section="Part D - Licensing", group=None, title="Audit Log",
         who="Administrator", where="Resources - Audit Log",
         purpose="A tamper-evident trail of every portal action: who did what, to which entity, "
                 "and when.",
         steps=[
             "Open <b>Resources - Audit Log</b>.",
             "Read the table: When, Actor, Action, Entity and Detail.",
             "Use it to trace a licence: <font face='Mono'>license.issue</font>, "
             "<font face='Mono'>license.activate</font> and, when someone tries the wrong PC, "
             "<font face='Mono'>license.activate.denied</font> (with reason "
             "<font face='Mono'>machine-mismatch</font>).",
         ],
         notes=["The same recent events appear on the Dashboard. The portal shows the latest 200; the API accepts a <font face='Mono'>limit</font>."],
         api=[("Recent audit events", "GET /api/audit?limit=200")],
         shots=[("audit", "Audit Log: the full action trail.")]),
]


# ==========================================================================
# STORY
# ==========================================================================
def cover_story():
    return [
        Spacer(1, 58 * mm),
        P("VanijLiquor", "cover_title"),
        Spacer(1, 3 * mm),
        P("Customer Portal - User Guide", "cover_sub"),
        Spacer(1, 8 * mm),
        Table([[Paragraph(
            "The self-service portal for VanijLiquor customers. Onboard a business, choose a "
            "plan, modules and billing term, get a live price quote, then issue a signed, "
            "machine-bound licence and activate it on the counter PC. Every feature is "
            "illustrated with a real screenshot of the running portal.", S["cover_meta"])]],
            colWidths=[150 * mm],
            style=TableStyle([("LEFTPADDING", (0, 0), (-1, -1), 0),
                              ("RIGHTPADDING", (0, 0), (-1, -1), 0),
                              ("TOPPADDING", (0, 0), (-1, -1), 0),
                              ("BOTTOMPADDING", (0, 0), (-1, -1), 0)])),
        Spacer(1, 12 * mm),
        Paragraph(f"Version {VERSION} &nbsp;&bull;&nbsp; {BUILD_DATE}", S["cover_meta"]),
        Spacer(1, 2 * mm),
        Paragraph("Prepared for: shop owners, onboarding staff and the licensing team", S["cover_meta"]),
    ]


def intro_story():
    story = [
        toc_para("How to use this guide", 0),
        rule(BRAND, 1.0, 6),
        P("Every feature is written the same way: a small table of <b>who</b> it is for, "
          "<b>where</b> it lives in the menu, and <b>what</b> it is for, followed by numbered "
          "steps, any notes, and a <b>where this lives</b> table that names the API routes "
          "behind the screen. Each feature ends with one or more screenshots of the running "
          "portal."),
        Spacer(1, 3 * mm),
        callout("Conventions", [
            "Menu paths use a dash, for example <b>Onboard - Plans &amp; Pricing</b>.",
            "Values you type or pick are shown in monospace, for example "
            "<font face='Mono'>VL-LIC-XXXXXX</font>.",
            "Money is shown in rupees; the portal adds <b>GST 18%</b> on top of the plan price.",
        ]),
        Spacer(1, 4 * mm),
        toc_para("Run the portal and load the demo data", 0),
        rule(BRAND, 1.0, 6),
        P("Start the portal and, optionally, reset it to the demo dataset before you explore:"),
        Paragraph("npm start", S["code"]),
        Paragraph("npm run seed-demo", S["code"]),
        Spacer(1, 2 * mm),
        callout("What the demo dataset contains", [
            "Three businesses: <font face='Mono'>Shree Wines &amp; Co</font> (Premium, 3 years), "
            "<font face='Mono'>Madhushala Bar &amp; Restaurant</font> (Standard, 1 year) and "
            "<font face='Mono'>Ganesh Liquor Mart</font> (Basic, monthly).",
            "One licence per business, each activated on its own machine fingerprint.",
            "Codes in the legacy style: <font face='Mono'>VL-2026-0001</font> and up.",
            "MRR of <font face='Mono'>Rs 3,197</font> from the three active businesses.",
        ]),
        Spacer(1, 3 * mm),
        callout("Ports and commands", [
            "Backend: <font face='Mono'>127.0.0.1:7200</font>. Web UI: "
            "<font face='Mono'>http://127.0.0.1:8081</font>.",
            "Other scripts: <font face='Mono'>npm test</font> (33 tests), "
            "<font face='Mono'>npm run reset-db</font>, "
            "<font face='Mono'>npm run gen-license-key</font>, "
            "<font face='Mono'>npm run screenshots</font> (rebuild this guide's images).",
        ], accent=ACCENT, bg=colors.HexColor("#eef2ff")),
    ]
    return story


def reference_story():
    story = [
        PageBreak(),
        toc_para("Part E - Reference", 0),
        rule(BRAND, 1.0, 6),

        toc_para("Sample data (demo dataset)", 1),
        P("Load it with <font face='Mono'>npm run seed-demo</font>. The three demo businesses "
          "are listed below; each has a licence activated on its own fingerprint."),
        Spacer(1, 2 * mm),
        data_table(
            ["Business", "Code", "Plan / term", "Type", "City"],
            [["Shree Wines &amp; Co", "VL-2026-0001", "Premium / 3 years", "retail", "Pune"],
             ["Madhushala Bar &amp; Restaurant", "VL-2026-0002", "Standard / 1 year", "bar", "Bengaluru"],
             ["Ganesh Liquor Mart", "VL-2026-0003", "Basic / monthly", "wholesale", "Kolkata"]],
            [52 * mm, 30 * mm, 36 * mm, 22 * mm, 30 * mm]),
        Spacer(1, 5 * mm),

        toc_para("Plans, terms and modules", 1),
        P("Three plans, four billing terms. A plan bundles modules; other modules are "
          "stand-alone add-ons."),
        Spacer(1, 2 * mm),
        data_table(
            ["Plan", "Monthly", "1 year", "2 years", "3 years"],
            [["Basic", "Rs 499", "Rs 5,599", "Rs 8,999", "Rs 12,999"],
             ["Standard", "Rs 1,199", "Rs 12,999", "Rs 21,999", "Rs 29,999"],
             ["Premium", "Rs 1,499", "Rs 15,999", "Rs 29,999", "Rs 39,999"]],
            [34 * mm, 34 * mm, 34 * mm, 34 * mm, 34 * mm]),
        Spacer(1, 3 * mm),
        data_table(
            ["Add-on", "Detail", "Price", "Period"],
            [["Cloud", "Cloud access and connected data services.", "Rs 7,999", "Yearly"],
             ["WhatsApp", "WhatsApp integration and support.", "Rs 3,999", "Yearly"],
             ["Tally", "Automatic Tally accounting synchronisation.", "Rs 5,999", "Yearly"],
             ["Purchase Import", "Purchase import functionality.", "Rs 5,999", "One-time"],
             ["LAN PC", "Additional LAN-connected computer.", "Rs 5,999", "One-time"],
             ["ePOS", "Electronic POS functionality.", "Rs 3,999", "One-time"]],
            [34 * mm, 78 * mm, 26 * mm, 32 * mm]),
        Spacer(1, 5 * mm),

        toc_para("Licensing model and anti-clone", 1),
        P("A licence is a JSON payload plus a detached <b>Ed25519</b> signature. The payload "
          "carries the licence id, business, plan, entitlements, the machine hash and the valid "
          "dates. The POS app pins the public key and verifies the licence offline."),
        Spacer(1, 2 * mm),
        callout("How the machine binding works", [
            "The installer collects stable identifiers (primary MAC, CPU id, system-disk serial, "
            "hostname) and the portal hashes the normalised set with SHA-256 to a "
            "<font face='Mono'>machine_hash</font>.",
            "The licence is bound to that hash. Activation on any other machine is refused: "
            "<i>Licence is bound to a different machine</i>.",
            "The hash is one-way, so the licence file leaks nothing reusable.",
        ]),
        Spacer(1, 4 * mm),

        toc_para("API reference", 1),
        P("The web front end proxies <font face='Mono'>/api</font> to the backend. All routes "
          "return <font face='Mono'>{ ok: true, data: ... }</font> on success."),
        Spacer(1, 2 * mm),
        api_table([
            ("Catalogue", "GET /api/health, /api/meta, /api/catalogue, /api/plans, /api/periods, "
                          "/api/modules, /api/addons, /api/comparison"),
            ("Quoting", "POST /api/quote"),
            ("Businesses", "GET|POST /api/businesses, GET|PUT /api/businesses/:id, "
                           "POST|DELETE /api/businesses/:id/modules[/:code]"),
            ("Licences", "POST|GET /api/licenses, GET /api/licenses/:id, "
                         "GET /api/licenses/:id/file"),
            ("Activation", "POST /api/licenses/:id/activate, /verify, /revoke, /resign, "
                           "GET /api/licenses/:id/activations"),
            ("Signing key", "GET /api/license-key"),
            ("Portal", "GET /api/dashboard, GET /api/audit?limit="),
        ]),
        Spacer(1, 5 * mm),

        toc_para("Troubleshooting", 1),
        data_table(
            ["Symptom", "What to do"],
            [["<i>Licence is bound to a different machine</i>",
              "The fingerprint does not match the one the licence was issued for. Re-issue the "
              "licence with this PC's fingerprint, or activate on the correct PC."],
             ["<i>Licence has expired</i>",
              "Issue a fresh licence with a new term from Issue Licence."],
             ["Backend unreachable (toast on load)",
              "Start the portal with <font face='Mono'>npm start</font>; check the backend is "
              "listening on <font face='Mono'>127.0.0.1:7200</font>."],
             ["Signing key shows <i>development</i>",
              "Set <font face='Mono'>PORTAL_LICENSE_KEY</font> to your Ed25519 seed and restart "
              "the backend before issuing real licences."],
             ["Installers are placeholders",
              "Drop your signed installers into <font face='Mono'>web/downloads/</font>."]],
            [62 * mm, 108 * mm]),
        Spacer(1, 5 * mm),

        toc_para("Where to find things in the repository", 1),
        data_table(
            ["Path", "Contents"],
            [["backend/src/server.js", "HTTP server and routes"],
             ["backend/src/services/", "onboarding, catalogue, licensing, license (Ed25519), "
                                       "fingerprint, audit, helpers"],
             ["backend/src/db/", "schema.sql, seed.sql (plans, modules, add-ons, comparison)"],
             ["backend/test/", "Test suite (33 tests)"],
             ["web/", "SPA: menu.js (routes), screens.js, api.js, ui.js, app.js, styles.css"],
             ["web/downloads/", "Machine-fingerprint helper scripts (PowerShell + shell)"],
             ["scripts/", "start.js, seed_demo.js, capture_screenshots.js"],
             ["docs/", "This guide and docs/screenshots/"]],
            [56 * mm, 114 * mm]),
    ]
    return story


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

    story += intro_story()

    seen_section = None
    seen_group = None
    for f in FEATURES:
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
        head.append(meta_table([("Who", f["who"]), ("Where", f["where"]), ("What for", f["purpose"])]))
        head.append(Spacer(1, 3 * mm))
        block.append(KeepTogether(head))
        block.append(steps_table(f["steps"]))
        if f.get("notes"):
            block.append(Spacer(1, 2 * mm))
            block.append(callout("Notes", f["notes"]))
        if f.get("api"):
            block.append(Spacer(1, 3 * mm))
            block.append(Paragraph("Where this lives: portal screen &rarr; API routes", S["cellb"]))
            block.append(Spacer(1, 1 * mm))
            block.append(api_table(f["api"]))
        for stem, cap in f.get("shots", []):
            block.append(Spacer(1, 3 * mm))
            block += screenshot(stem, cap)
        block.append(Spacer(1, 7 * mm))
        story += block

    story += reference_story()
    return story


def main():
    doc = GuideDoc(OUT)
    doc.multiBuild(build_story(doc))
    print(f"Wrote {os.path.relpath(OUT, HERE)}")


if __name__ == "__main__":
    main()
