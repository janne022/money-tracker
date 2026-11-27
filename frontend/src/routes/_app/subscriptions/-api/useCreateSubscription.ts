import { apiClient } from "@/shared/api/api";

import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { CreateSubscriptionRequest } from "@/shared/api/generated/model";
import { subscriptionsQuery } from "./subscriptionsQuery";

export function useCreateSubscription() {
  const query = useQueryClient();

  return useMutation({
    mutationFn: (request: CreateSubscriptionRequest) =>
      apiClient.post<void>("/api/CreateSubscription", request),

    onSuccess: () =>
      query.invalidateQueries({ queryKey: subscriptionsQuery.queryKey }),
  });
}
