// Portal navigation tree. Each item is a route id handled in screens.js.

export const NAV = [
  {
    title: 'Overview',
    items: [
      { title: 'Dashboard', route: 'dashboard' },
      { title: 'Businesses', route: 'businesses' },
      { title: 'Licences', route: 'licenses' },
    ],
  },
  {
    title: 'Onboard',
    items: [
      { title: 'New Business', route: 'onboard' },
      { title: 'Plans & Pricing', route: 'plans' },
      { title: 'Modules', route: 'modules' },
    ],
  },
  {
    title: 'Licensing',
    items: [
      { title: 'Issue Licence', route: 'issue' },
      { title: 'Activate / Verify', route: 'verify' },
    ],
  },
  {
    title: 'Resources',
    items: [
      { title: 'Download App', route: 'download' },
      { title: 'Licence Key', route: 'key' },
      { title: 'Audit Log', route: 'audit' },
    ],
  },
];

export const ROUTE_TITLES = {
  dashboard: 'Dashboard',
  businesses: 'Businesses',
  business: 'Business Details',
  licenses: 'Licences',
  onboard: 'Onboard a Business',
  plans: 'Plans & Pricing',
  modules: 'Modules & Add-ons',
  issue: 'Issue a Licence',
  verify: 'Activate / Verify a Licence',
  download: 'Download the App',
  key: 'Licence Signing Key',
  audit: 'Audit Log',
};
