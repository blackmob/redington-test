import { UserConfig } from '@commitlint/types';

const azureWorkItemIdRegex = /#\d+/g;

const config: UserConfig = {
  extends: ['@commitlint/config-conventional'],
  rules: {
    'scope-enum': async () => [2, 'always', ['release']],
    'no-azure-id': [2, 'always'],
  },
  plugins: [
    {
      rules: {
        /**
         * Disallow Azure DevOps work item IDs in the commit subject.
         * To attach work item IDs use the `Refs:` footer field.
         *
         * Example valid commit message:
         *
         * feat: add password reset feature
         *
         * Refs: #123456
         */
        'no-azure-id': (parsed) => {
          const { subject } = parsed;
          const matches = (subject ?? '').match(azureWorkItemIdRegex);
          return [
            !matches,
            `Azure DevOps work item identifier number is not allowed in subject. Use the 'Refs:' footer instead: ${matches}`,
          ];
        },
      },
    },
  ],
};
export default config;
