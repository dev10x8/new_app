import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));

export const config = {
  // new_code/data/vanijliquor.db
  dbFile: process.env.VANIJLIQUOR_DB || resolve(here, '..', '..', 'data', 'vanijliquor.db'),
  host: process.env.HOST || '127.0.0.1',
  port: Number(process.env.PORT || 7100),
  company: process.env.COMPANY || '1',
  year: process.env.YEAR || '2024',
};

export default config;
