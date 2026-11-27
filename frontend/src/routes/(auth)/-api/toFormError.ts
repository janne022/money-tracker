import { ApiError } from "@/shared/api/api";

type Problem = { detail?: string; errors?: Record<string, string[]> };

// Used to convert an ApiError into a form error
export function toFormError(error: Error) {
  if (!(error instanceof ApiError)) return error;
  const problem = (error.details ?? {}) as Problem;

  if (problem.detail === "LockedOut")
    return new Error("Too many failed attempts. Try again in a few minutes.");
  if (problem.detail === "Failed")
    return new Error("Wrong email or password.");

  const messages = Object.values(problem.errors ?? {}).flat();
  return new Error(
    messages.join("\n") || "Something went wrong. Please try again.",
  );
}
