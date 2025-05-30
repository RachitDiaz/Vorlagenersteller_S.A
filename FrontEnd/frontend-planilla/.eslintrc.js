/* eslint-env node */
module.exports = {
  root: true,
  env: {
    browser: true,
    es2021: true,
  },
  extends: [
    "plugin:vue/vue3-essential",
    "google"
  ],
  parserOptions: {
    ecmaVersion: 12,
    sourceType: "module"
  },
  overrides: [
    {
      files: ["*.config.js"],
      env: {
        node: true,
      }
    }
  ],
  rules: {
  }
};
