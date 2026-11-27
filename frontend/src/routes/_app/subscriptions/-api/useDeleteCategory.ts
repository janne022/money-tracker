import { apiClient } from "@/shared/api/api";

import { useMutation, useQueryClient } from "@tanstack/react-query";
import { subscriptionsQuery } from "./subscriptionsQuery";

export function useDeleteCategory() {
  const query = useQueryClient();

  return useMutation({
    mutationFn: (id: number | string) =>
      apiClient.delete<void>(`/api/DeleteCategory/${id}`),

    onSuccess: () =>
      query.invalidateQueries({ queryKey: subscriptionsQuery.queryKey }),
  });
}
