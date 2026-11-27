import { apiClient } from "@/shared/api/api";

import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { UpdateCategoryRequest } from "@/shared/api/generated/model";
import { subscriptionsQuery } from "./subscriptionsQuery";

export function useUpdateCategory() {
  const query = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: {
      id: number | string;
      request: UpdateCategoryRequest;
    }) => apiClient.put<void>(`/api/UpdateCategory/${id}`, request),

    onSuccess: () =>
      query.invalidateQueries({ queryKey: subscriptionsQuery.queryKey }),
  });
}
