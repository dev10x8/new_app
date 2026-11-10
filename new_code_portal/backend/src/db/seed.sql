-- Seed data for the VanijLiquor Customer Portal.
--
-- Plans, prices, billing periods, modules, add-ons and the comparison matrix
-- mirror the vendor's public subscriptions page (madhushalasoftware.com/
-- subscriptions) so the portal is accurate on first run. Prices are whole
-- rupees. Edit here (or in the DB) to re-price.

-- ------------------------------------------------------------- billing periods
INSERT OR IGNORE INTO billing_periods (code, label, months, sort) VALUES
  ('monthly',    'Month',    1,  1),
  ('oneYear',    '1 Year',  12,  2),
  ('twoYears',   '2 Years', 24,  3),
  ('threeYears', '3 Years', 36,  4);

-- ------------------------------------------------------------- plans
INSERT OR IGNORE INTO plans (code, name, tagline, monthly, popular, sort, highlights) VALUES
  ('basic', 'Basic', 'Essential features for your business.', 499, 0, 1,
   '["Essential billing & stock control","Excise management","Offline usage","Single counter"]'),
  ('standard', 'Standard', 'Advanced features with better control.', 1199, 1, 2,
   '["Everything in Basic","AMC & WhatsApp support","Cloud data, backup & web reporting","One LAN PC"]'),
  ('premium', 'Premium', 'Complete solution with all features.', 1499, 0, 3,
   '["Everything in Standard","Tally integration","Attendance & antivirus","Advanced reports & AI assistant"]');

-- ------------------------------------------------------------- plan prices
INSERT OR IGNORE INTO plan_prices (plan_code, period_code, amount) VALUES
  ('basic',    'monthly',    499), ('basic',    'oneYear',   5599),
  ('basic',    'twoYears',  8999), ('basic',    'threeYears', 12999),
  ('standard', 'monthly',   1199), ('standard', 'oneYear',  12999),
  ('standard', 'twoYears', 21999), ('standard', 'threeYears', 29999),
  ('premium',  'monthly',   1499), ('premium',  'oneYear',  15999),
  ('premium',  'twoYears', 29999), ('premium',  'threeYears', 39999);

-- ------------------------------------------------------------- modules
-- addon_price > 0 means the module can also be bought standalone as an add-on.
INSERT OR IGNORE INTO modules (code, name, category, description, addon_price, sort) VALUES
  ('SOFTWARE',        'Software Subscription', 'Software & Usage', 'Core licence to run the app.', 0, 1),
  ('OFFLINE',         'Offline Billing',       'Software & Usage', 'Bill and print with no internet.', 0, 2),
  ('SALES_POS',       'Sales / POS Billing',   'Software & Usage', 'Counter billing, KOT and printing.', 0, 3),
  ('STOCK',           'Stock & Inventory',     'Software & Usage', 'Live stock, low-stock alerts, requisitions.', 0, 4),
  ('BREAKAGE',        'Breakage & Leakage',    'Software & Usage', 'Record breakage against stock.', 0, 5),
  ('STOCK_TRANSFER',  'Stock Transfer',        'Software & Usage', 'Move stock between counters / stores.', 0, 6),
  ('EXCISE',          'Excise Sync',           'Software & Usage', 'State excise registers, audit and Form M-22.', 0, 7),
  ('REPORTS',         'Core Reports',          'Software & Usage', 'Sales, stock, purchase and MIS reports.', 0, 8),
  ('AMC',             'AMC Support',           'Support & Assistance', 'Annual maintenance contract support.', 0, 9),
  ('WHATSAPP_SUPPORT','WhatsApp Support',      'Support & Assistance', 'Support over WhatsApp.', 3999, 10),
  ('LIVE_CHAT',       'Live Chat',             'Support & Assistance', 'In-app live chat with support.', 0, 11),
  ('AUTO_BACKUP',     'Auto Backup',           'Cloud & Connected Access', 'Scheduled automatic backups.', 0, 12),
  ('MOBILE_APP',      'Mobile App',            'Cloud & Connected Access', 'Owner mobile app access.', 0, 13),
  ('CLOUD',           'Cloud Data',            'Cloud & Connected Access', 'Cloud data and connected services.', 7999, 14),
  ('WEB_REPORTING',   'Web Reporting',         'Cloud & Connected Access', 'Reports over the web.', 0, 15),
  ('LAN_PC',          'One LAN PC',            'Cloud & Connected Access', 'One extra LAN-connected computer.', 5999, 16),
  ('TALLY',           'Tally Integration',     'Integrations & Advanced Tools', 'Twin-agent sync to Tally Prime.', 5999, 17),
  ('ATTENDANCE',      'Attendance System',     'Integrations & Advanced Tools', 'Staff attendance capture.', 0, 18),
  ('ANTIVIRUS',       'Antivirus',             'Integrations & Advanced Tools', 'Managed endpoint antivirus.', 0, 19),
  ('LIFETIME_UPDATES','Lifetime Updates',      'Integrations & Advanced Tools', 'Free feature updates for life.', 0, 20),
  ('ADVANCED_REPORTS','Advanced Reports',      'Integrations & Advanced Tools', 'Deep MIS and analytics reports.', 0, 21),
  ('CAMERA_AI',       'Camera AI',             'Integrations & Advanced Tools', 'AI camera monitoring at the counter.', 0, 22),
  ('PARCHA',          'Parch Load (AI Purchase Import)', 'Integrations & Advanced Tools', 'Import wholesale invoices / parchas into purchases.', 5999, 23),
  ('AI_ASSISTANT',    'AI Assistant',          'Integrations & Advanced Tools', 'Assistant for entries, queries and insights.', 0, 24),
  ('EPOS',            'ePOS',                  'Integrations & Advanced Tools', 'Electronic POS integration.', 3999, 25);

-- ------------------------------------------------------------- plan modules (bundled)
INSERT OR IGNORE INTO plan_modules (plan_code, module_code, included) VALUES
  -- Basic
  ('basic','SOFTWARE',1),('basic','OFFLINE',1),('basic','SALES_POS',1),('basic','STOCK',1),
  ('basic','BREAKAGE',1),('basic','STOCK_TRANSFER',1),('basic','EXCISE',1),('basic','REPORTS',1),
  -- Standard (adds support + cloud + LAN)
  ('standard','SOFTWARE',1),('standard','OFFLINE',1),('standard','SALES_POS',1),('standard','STOCK',1),
  ('standard','BREAKAGE',1),('standard','STOCK_TRANSFER',1),('standard','EXCISE',1),('standard','REPORTS',1),
  ('standard','AMC',1),('standard','WHATSAPP_SUPPORT',1),('standard','LIVE_CHAT',1),
  ('standard','AUTO_BACKUP',1),('standard','MOBILE_APP',1),('standard','CLOUD',1),
  ('standard','WEB_REPORTING',1),('standard','LAN_PC',1),
  -- Premium (adds integrations + AI)
  ('premium','SOFTWARE',1),('premium','OFFLINE',1),('premium','SALES_POS',1),('premium','STOCK',1),
  ('premium','BREAKAGE',1),('premium','STOCK_TRANSFER',1),('premium','EXCISE',1),('premium','REPORTS',1),
  ('premium','AMC',1),('premium','WHATSAPP_SUPPORT',1),('premium','LIVE_CHAT',1),
  ('premium','AUTO_BACKUP',1),('premium','MOBILE_APP',1),('premium','CLOUD',1),
  ('premium','WEB_REPORTING',1),('premium','LAN_PC',1),
  ('premium','TALLY',1),('premium','ATTENDANCE',1),('premium','ANTIVIRUS',1),
  ('premium','LIFETIME_UPDATES',1),('premium','ADVANCED_REPORTS',1),('premium','CAMERA_AI',1),
  ('premium','AI_ASSISTANT',1);

-- ------------------------------------------------------------- add-ons
INSERT OR IGNORE INTO addons (code, name, detail, price, period, sort) VALUES
  ('cloud',           'Cloud',           'Cloud access and connected data services.', 7999, 'Yearly',   1),
  ('whatsapp',        'WhatsApp',        'WhatsApp-related integration and support.', 3999, 'Yearly',   2),
  ('tally',           'Tally',           'Automatic Tally accounting synchronisation.', 5999, 'Yearly',   3),
  ('purchase-import', 'Purchase Import', 'Purchase import functionality (one-time).', 5999, 'One-time', 4),
  ('lan-pc',          'LAN PC',          'Additional LAN-connected computer.', 5999, 'One-time', 5),
  ('epos',            'ePOS',            'Electronic POS functionality.', 3999, 'One-time', 6);

-- ------------------------------------------------------------- comparison matrix
INSERT OR IGNORE INTO comparison (category, feature, basic, standard, premium, sort) VALUES
  ('Software & Usage', 'Software Subscription', 1,1,1, 1),
  ('Software & Usage', 'Offline Usage',         1,1,1, 2),
  ('Support & Assistance', 'AMC Support',       0,1,1, 1),
  ('Support & Assistance', 'WhatsApp Support',  0,1,1, 2),
  ('Support & Assistance', 'Live Chat',         0,1,1, 3),
  ('Cloud & Connected Access', 'Auto Backup',   0,1,1, 1),
  ('Cloud & Connected Access', 'Mobile App',    0,1,1, 2),
  ('Cloud & Connected Access', 'Cloud Data',    0,1,1, 3),
  ('Cloud & Connected Access', 'Web Reporting', 0,1,1, 4),
  ('Cloud & Connected Access', 'One LAN PC',    0,1,1, 5),
  ('Integrations & Advanced Tools', 'Tally Integration', 0,0,1, 1),
  ('Integrations & Advanced Tools', 'Attendance System', 0,0,1, 2),
  ('Integrations & Advanced Tools', 'Antivirus',         0,0,1, 3),
  ('Integrations & Advanced Tools', 'Lifetime Updates',  0,0,1, 4),
  ('Integrations & Advanced Tools', 'Advanced Reports',  0,0,1, 5);

INSERT OR IGNORE INTO portal_meta (key, value) VALUES ('schema', '1');
