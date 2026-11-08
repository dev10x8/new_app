import { all, get, run } from '../db/index.js';
import { now } from './helpers.js';

/**
 * Validate credentials exactly like the legacy frmLoginNew flow: plaintext
 * comparison against users.password, access level "Admin" grants admin rights.
 */
export function login(db, { usercode, password }) {
  const user = get(
    db,
    'SELECT usercode, username, accesslevel, password FROM users WHERE usercode = ?',
    [usercode],
  );
  if (!user) return { ok: false, error: 'Unknown user code' };
  if (String(user.password) !== String(password)) return { ok: false, error: 'Invalid password' };
  const rights = all(
    db,
    'SELECT modulename, perv, pera, pere, perd FROM userrights WHERE usercode = ?',
    [usercode],
  );
  return {
    ok: true,
    user: {
      usercode: user.usercode,
      username: user.username,
      accesslevel: user.accesslevel,
      admin: String(user.accesslevel).toLowerCase() === 'admin',
      rights,
      loginAt: now(),
    },
  };
}

export function listUsers(db) {
  return all(db, 'SELECT usercode, username, accesslevel FROM users ORDER BY usercode');
}

export function createUser(db, { usercode, username, accesslevel = 'User', password = '' }) {
  run(db, 'INSERT INTO users (usercode, username, accesslevel, password) VALUES (?,?,?,?)',
    [usercode, username, accesslevel, password]);
  return { usercode };
}

export function updateUser(db, usercode, { username, accesslevel, password }) {
  const cur = get(db, 'SELECT * FROM users WHERE usercode = ?', [usercode]);
  if (!cur) throw new Error(`User ${usercode} not found`);
  run(db, 'UPDATE users SET username=?, accesslevel=?, password=? WHERE usercode=?',
    [username ?? cur.username, accesslevel ?? cur.accesslevel, password ?? cur.password, usercode]);
  return { usercode };
}

/** Reset a user's PIN/password. Admin PIN lives in users.password. */
export function resetPin(db, usercode, newPin) {
  const cur = get(db, 'SELECT usercode FROM users WHERE usercode = ?', [usercode]);
  if (!cur) throw new Error(`User ${usercode} not found`);
  run(db, 'UPDATE users SET password = ? WHERE usercode = ?', [String(newPin), usercode]);
  return { usercode, reset: true };
}

export function setRights(db, usercode, company, module, rights) {
  const existing = get(
    db,
    'SELECT 1 FROM userrights WHERE usercode=? AND companycode=? AND modulename=?',
    [usercode, company, module],
  );
  if (existing) {
    run(
      db,
      'UPDATE userrights SET perv=?, pera=?, pere=?, perd=? WHERE usercode=? AND companycode=? AND modulename=?',
      [rights.perv || 'N', rights.pera || 'N', rights.pere || 'N', rights.perd || 'N', usercode, company, module],
    );
  } else {
    run(
      db,
      'INSERT INTO userrights (usercode, companycode, accaccess, modulename, perv, pera, pere, perd) VALUES (?,?,?,?,?,?,?,?)',
      [usercode, company, 'Y', module, rights.perv || 'N', rights.pera || 'N', rights.pere || 'N', rights.perd || 'N'],
    );
  }
  return { usercode, module };
}
