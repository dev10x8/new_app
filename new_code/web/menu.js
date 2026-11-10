// Navigation tree. Mirrors the legacy frm_main tv_menu structure (and the TUI
// menu) but each item is a route id handled in app.js.

export const NAV = [
  {
    title: 'Main',
    icon: '⌂',
    items: [
      { title: 'Dashboard', route: 'dashboard' },
      { title: 'System Admin', route: 'sysadmin' },
    ],
  },
  {
    title: 'Admin',
    items: [
      { title: 'Company', route: 'master:company' },
      { title: 'Parameters', route: 'params' },
      { title: 'User', route: 'users' },
      { title: 'User Rights', route: 'rights' },
      { title: 'New Year', route: 'new-year' },
      { title: 'Excise Audit', route: 'excise-audit' },
      { title: 'Deleted Bill', route: 'deleted-bills' },
    ],
  },
  {
    title: 'Masters',
    items: [
      { title: 'A/C Main Group', route: 'master:acgroup' },
      { title: 'A/C Sub Group', route: 'master:acount' },
      { title: 'Ledger', route: 'master:ledger' },
      { title: 'Category', route: 'master:category' },
      { title: 'KFL', route: 'master:kfl' },
      { title: 'Manufacturer', route: 'master:manufacturer' },
      { title: 'Strength', route: 'master:strength' },
      { title: 'Measure and Packing', route: 'master:itemgroupml' },
      { title: 'Pack Sizes (Quart/Pint/Nip)', route: 'pack-sizes' },
      { title: 'Item Master', route: 'items' },
      { title: 'Inventory', route: 'inventory' },
      { title: 'State Master List', route: 'state-master' },
      { title: 'Bulk Master Upload', route: 'bulk-import' },
      { title: 'Sales Rate', route: 'master:rateinfo' },
      { title: 'Store Master', route: 'master:storage' },
      { title: 'Counter Master', route: 'master:counter' },
      { title: 'Customer Master', route: 'master:customer' },
      { title: 'Area', route: 'master:area' },
      { title: 'Tax Schemes', route: 'master:taxscheme' },
      { title: 'Scheme / Offer', route: 'schemes' },
      { title: 'Excise Item', route: 'master:exciseitem' },
      { title: 'Excise Report Tagging', route: 'master:reporttagmain' },
      { title: 'NAI Unit', route: 'master:naiunit' },
      { title: 'NAI Item Group', route: 'master:naigroup' },
      { title: 'NAI Item Sub Group', route: 'master:naisubgroup' },
      { title: 'NAI Item Master', route: 'master:naiitem' },
    ],
  },
  {
    title: 'Transactions',
    items: [
      { title: 'Sale – POS', route: 'sale' },
      { title: 'Sales Register', route: 'sales-register' },
      { title: 'Purchase', route: 'purchase' },
      { title: 'Parcha Import (Invoice)', route: 'parcha-import' },
      { title: 'Counter / Transfer Sale', route: 'transfer-sale' },
      { title: 'Transfer Receive', route: 'transfer-receive' },
      { title: 'Payment Entry', route: 'voucher:PAYMENT' },
      { title: 'Receipt Entry', route: 'voucher:RECEIPT' },
      { title: 'Contra Entry', route: 'voucher:CONTRA' },
      { title: 'Journal Entry', route: 'voucher:JOURNAL' },
      { title: 'Debit Note', route: 'note:DEBIT' },
      { title: 'Credit Note', route: 'note:CREDIT' },
      { title: 'On A/C Adjustment', route: 'on-account' },
      { title: 'A/C Replace', route: 'account-replace' },
      { title: 'Excise Fine', route: 'fine' },
      { title: 'Opening Stock', route: 'opening-stock' },
      { title: 'Stock Transfer', route: 'stock-transfer' },
      { title: 'Breakage', route: 'breakage' },
      { title: 'BRS Entry', route: 'brs' },
      { title: 'QRCode / UPI Payment', route: 'upi' },
      { title: 'Home Delivery', route: 'delivery' },
    ],
  },
  {
    title: 'Reports',
    items: [
      { title: 'Sales Reports', route: 'reports:sales' },
      { title: 'Stock Reports', route: 'reports:stock' },
      { title: 'Purchase Reports', route: 'reports:purchase' },
      { title: 'Accounts Reports', route: 'reports:accounts' },
      { title: 'MIS Reports', route: 'reports:mis' },
      { title: 'Breakage Reports', route: 'reports:breakage' },
      { title: 'Stock Transfer Reports', route: 'reports:transfer' },
      { title: 'Excise Reports', route: 'reports:excise' },
      { title: 'GST Reports', route: 'reports:gst' },
    ],
  },
  {
    title: 'Dash Board',
    items: [
      { title: 'Sales Status', route: 'dashboard' },
      { title: 'Stock Status', route: 'dash:stock' },
      { title: 'Purchase Status', route: 'dash:purchase' },
      { title: 'Scheme / Spirit-Beer / CS', route: 'dash:scheme' },
    ],
  },
  {
    title: 'Tools',
    items: [
      { title: 'Calculator', route: 'calc' },
      { title: 'Change Password', route: 'change-password' },
      { title: 'Data Back-Up', route: 'backup' },
      { title: 'Data Restore', route: 'restore' },
      { title: 'Data Migration', route: 'migration' },
      { title: 'Landed Cost / Margin', route: 'landed-cost' },
      { title: 'Export To Tally', route: 'tally' },
      { title: 'Remote Tally Sync', route: 'tally-sync' },
      { title: 'Excise Sync', route: 'excise-sync' },
      { title: 'Form M-22', route: 'form-m22' },
      { title: 'GST / Returns', route: 'gst' },
      { title: 'SMS / WhatsApp', route: 'messages' },
      { title: 'PML Print and Reports', route: 'pml' },
      { title: 'Clean Print Garbage', route: 'clean-print' },
      { title: 'Data Sync', route: 'sync' },
    ],
  },
  {
    title: 'Help',
    items: [
      { title: 'About Madhushala', route: 'about' },
      { title: 'Ask for Help', route: 'help' },
      { title: 'List of Updates', route: 'updates' },
    ],
  },
];

// Flat route -> title map for page titles.
export const ROUTE_TITLES = (() => {
  const m = { dashboard: 'Dashboard' };
  for (const g of NAV) for (const it of g.items) m[it.route] = it.title;
  return m;
})();
