import { FlatCompat } from '@eslint/eslintrc';
import jsxa11y from 'eslint-plugin-jsx-a11y';
import prettier from 'eslint-plugin-prettier';
import simpleImportSort from 'eslint-plugin-simple-import-sort';
import tailwindCss from 'eslint-plugin-tailwindcss';
import testingLibrary from 'eslint-plugin-testing-library';
import unusedImports from 'eslint-plugin-unused-imports';
import { dirname } from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const compat = new FlatCompat({ baseDirectory: __dirname });

const eslintConfig = [
  ...compat.extends('next/core-web-vitals', 'next/typescript'),
  { ignores: ['**/.next/**', '**/node_modules/**', '**/public/**'] },
  {
    plugins: {
      'jsx-a11y': jsxa11y,
      tailwindcss: tailwindCss,
      'unused-imports': unusedImports,
      'simple-import-sort': simpleImportSort,
      'testing-library': testingLibrary,
      prettier: prettier,
    },
    rules: {
      'unused-imports/no-unused-imports': 'error',
      'unused-imports/no-unused-vars': 'error',
      'simple-import-sort/imports': 'warn',
      'simple-import-sort/exports': 'warn',
      '@typescript-eslint/no-unused-vars': [
        'warn',
        { ignoreRestSiblings: true },
      ],
      'testing-library/no-node-access': 'off',
      'jsx-a11y/no-onchange': 'off',
      'jsx-a11y/anchor-is-valid': [
        'error',
        { aspects: ['invalidHref', 'preferButton'] },
      ],
      'prettier/prettier': 'error',
      'import/no-duplicates': 'off',
    },
  },
];

export default eslintConfig;
