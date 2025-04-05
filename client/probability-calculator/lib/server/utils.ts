'use server';
/* v8 ignore start */
import { unstable_noStore as noStore } from 'next/cache';

import { Settings } from '../models/Settings';

export async function getSettings(): Promise<Settings> {
  noStore();
  return { apiBaseUrl: process.env.CALCULATOR_API_URL ?? '' };
}

/* v8 ignore end */
