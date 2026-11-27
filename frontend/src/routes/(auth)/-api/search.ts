export type AuthSearch = { redirect?: string };

// Handles redirect after login/register
export function validateAuthSearch(
  search: Record<string, unknown>,
): AuthSearch {
  return typeof search.redirect === "string"
    ? { redirect: search.redirect }
    : {};
}

export function safeRedirect(target: string | undefined) {
  if (!target) return "/";
  const url = new URL(target, window.location.origin);
  return url.origin === window.location.origin
    ? url.pathname + url.search + url.hash
    : "/";
}
