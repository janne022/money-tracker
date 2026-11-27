import type { QueryClient } from "@tanstack/react-query";
import { Link, Outlet, createRootRouteWithContext } from "@tanstack/react-router";

export const Route = createRootRouteWithContext<{
  queryClient: QueryClient;
}>()({
  component: Outlet,
  notFoundComponent: NotFound,
});

function NotFound() {
  return (
    <div className="p-6">
      <p>Page not found.</p>
      <Link to="/" className="text-primary underline">
        Back to overview
      </Link>
    </div>
  );
}
