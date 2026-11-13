// Light/dark theme. Dark is the default; the choice persists in localStorage and
// is applied before first paint (the inline snippet in index.html) so there is no
// flash of the wrong theme.

const KEY = 'theme';
export const getTheme = () => (localStorage.getItem(KEY) === 'light' ? 'light' : 'dark');

export function applyTheme(theme) {
  document.documentElement.dataset.theme = theme === 'light' ? 'light' : 'dark';
}

export function toggleTheme() {
  const next = getTheme() === 'light' ? 'dark' : 'light';
  localStorage.setItem(KEY, next);
  applyTheme(next);
  return next;
}
