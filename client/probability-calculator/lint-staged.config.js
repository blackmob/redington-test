module.exports = {
  '!(*.{js,jsx,ts,tsx})': ['pnpm validate:staged:format'],
  '*.{js,jsx,ts,tsx}': [
    'pnpm validate:staged:format',
    'pnpm validate:staged:lint',
  ],
  '*.{ts,tsx}': [() => 'pnpm validate:staged:types'],
};
