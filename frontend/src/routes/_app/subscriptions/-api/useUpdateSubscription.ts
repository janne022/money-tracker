import { apiClient } from "@/shared/api/api";

import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { UpdateSubscriptionRequest } from "@/shared/api/generated/model";
import { subscriptionsQuery } from "./subscriptionsQuery";

export function useUpdateSubscription() {
  const query = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: {
      id: number | string;
      request: UpdateSubscriptionRequest;
    }) => apiClient.put<void>(`/api/UpdateSubscription/${id}`, request),

    onSuccess: () =>
      query.invalidateQueries({ queryKey: subscriptionsQuery.queryKey }),
  });
}
