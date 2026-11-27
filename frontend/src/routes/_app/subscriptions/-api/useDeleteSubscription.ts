import { apiClient } from "@/shared/api/api";

import { useMutation, useQueryClient } from "@tanstack/react-query";
import { subscriptionsQuery } from "./subscriptionsQuery";

export function useDeleteSubscription() {
  const query = useQueryClient();

  return useMutation({
    mutationFn: (id: number | string) =>
      apiClient.delete<void>(`/api/DeleteSubscription/${id}`),

    onSuccess: () =>
      query.invalidateQueries({ queryKey: subscriptionsQuery.queryKey }),
  });
}
