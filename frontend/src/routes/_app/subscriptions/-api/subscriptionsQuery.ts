import { apiClient } from "@/shared/api/api";

import { useQuery } from "@tanstack/react-query";
import type { GetAllSubscriptionsResponse } from "@/shared/api/generated/model";

export const subscriptionsQuery = {
  queryKey: ["subscriptions"] as const,

  queryFn: ({ signal }: { signal: AbortSignal }) =>
    apiClient.get<GetAllSubscriptionsResponse>("/api/GetAllSubscriptions", {
      signal,
    }),
};

export function useSubscriptions() {
  return useQuery(subscriptionsQuery);
}
