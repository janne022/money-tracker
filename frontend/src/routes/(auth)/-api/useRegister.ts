import { apiClient } from "@/shared/api/api";

import { useMutation } from "@tanstack/react-query";
import type { RegisterRequest } from "@/shared/api/generated/model";

export function useRegister() {
  return useMutation({
    mutationFn: (credentials: RegisterRequest) =>
      apiClient.post<void>("/api/register", credentials),
  });
}
