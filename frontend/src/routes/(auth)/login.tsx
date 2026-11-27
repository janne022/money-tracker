import { Link, createFileRoute, redirect } from "@tanstack/react-router";
import { AuthForm } from "./-components/auth-form";
import { currentUserQuery } from "@/shared/api/currentUser";
import { validateAuthSearch } from "./-api/search";

export const Route = createFileRoute("/(auth)/login")({
  validateSearch: validateAuthSearch,
  beforeLoad: async ({ context }) => {
    const user = await context.queryClient.query({
      ...currentUserQuery,
      staleTime: "static",
    });
    if (user) throw redirect({ to: "/" });
  },
  component: LoginPage,
});

function LoginPage() {
  const search = Route.useSearch();

  return (
    <AuthForm
      mode="login"
      redirect={search.redirect}
      footer={
        <>
          No account yet?{" "}
          <Link
            to="/register"
            search={search}
            className="text-primary underline"
          >
            Create one
          </Link>
        </>
      }
    />
  );
}
