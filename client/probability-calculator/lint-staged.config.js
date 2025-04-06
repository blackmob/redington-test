module.exports = {
  '!(*.{js,jsx,ts,tsx})': ['npm validate:staged:format'],
  '*.{js,jsx,ts,tsx}': [
    'npm validate:staged:format',
    'npm validate:staged:lint',
  ],
  '*.{ts,tsx}': [() => 'npm validate:staged:types'],
};
