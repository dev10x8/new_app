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
  // Date integrity. A clock behind the high-water mark by more than the skew
  // tolerance is treated as rolled back; posting allows up to futureTolerance
  // days ahead and flags back-dating beyond backdateTolerance days.
  clockSkewToleranceSeconds: Number(process.env.CLOCK_SKEW_TOLERANCE_SECONDS || 120),
  futureToleranceDays: Number(process.env.FUTURE_DATE_TOLERANCE_DAYS || 0),
  backdateToleranceDays: Number(process.env.BACKDATE_TOLERANCE_DAYS || 1),
};

export default config;
