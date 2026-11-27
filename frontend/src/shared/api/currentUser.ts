import { apiClient, ApiError } from "@/shared/api/api";

import { useQuery } from "@tanstack/react-query";
import type { MeResponse } from "@/shared/api/generated/model";

export const currentUserQuery = {
  queryKey: ["currentUser"] as const,

  queryFn: async ({
    signal,
  }: {
    signal: AbortSignal;
  }): Promise<MeResponse | null> => {
    try {
      const response = await apiClient.get<MeResponse>("/api/me", {
        signal,
      });

      return response;
    } catch (error) {
      if (error instanceof ApiError && error.status === 401) {
        return null;
      }

      throw error;
    }
  },
};

// Hook to get the current user info. Returns null if not logged in. Stale time: 5 minutes
export function useCurrentUser() {
  return useQuery({ ...currentUserQuery, retry: false, staleTime: 300_000 });
}
