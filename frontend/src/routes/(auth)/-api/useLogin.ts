import { apiClient } from "@/shared/api/api";

import { useQueryClient, useMutation } from "@tanstack/react-query";
import { currentUserQuery } from "@/shared/api/currentUser";
import type { LoginRequest } from "@/shared/api/generated/model";

// Hook to log in a user. On success, the current user query is invalidated and refetched.
export function useLogin() {
  const query = useQueryClient();

  return useMutation({
    mutationFn: (credentials: LoginRequest) =>
      apiClient.post<void>("/api/login?useCookies=true", credentials),

    onSuccess: async () => {
      await query.cancelQueries();

      query.clear();

      await query.query(currentUserQuery);
    },
  });
}
