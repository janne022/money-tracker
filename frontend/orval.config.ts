import { defineConfig } from "orval";

export default defineConfig({
  moneytracker: {
    input: "../moneytracker.Server/openapi.json",
    output: {
      mode: "split",
      target: "src/shared/api/generated/endpoints.ts",
      schemas: "src/shared/api/generated/model",
      client: "react-query",
      httpClient: "fetch",
      clean: true,
      override: {
        mutator: { path: "src/shared/api/api.ts", name: "apiFetch" },
        fetch: { includeHttpResponseReturnType: false },
      },
    },
  },
});
