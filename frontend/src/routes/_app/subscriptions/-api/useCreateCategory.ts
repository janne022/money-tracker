import { apiClient } from "@/shared/api/api";

import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { CreateCategoryRequest } from "@/shared/api/generated/model";
import { subscriptionsQuery } from "./subscriptionsQuery";

export function useCreateCategory() {
  const query = useQueryClient();

  return useMutation({
    mutationFn: (request: CreateCategoryRequest) =>
      apiClient.post<void>("/api/CreateCategory", request),

    onSuccess: () =>
      query.invalidateQueries({ queryKey: subscriptionsQuery.queryKey }),
  });
}
