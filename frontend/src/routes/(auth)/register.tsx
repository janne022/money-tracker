import { Link, createFileRoute, redirect } from "@tanstack/react-router";
import { AuthForm } from "./-components/auth-form";
import { currentUserQuery } from "@/shared/api/currentUser";
import { validateAuthSearch } from "./-api/search";

export const Route = createFileRoute("/(auth)/register")({
  validateSearch: validateAuthSearch,
  beforeLoad: async ({ context }) => {
    const user = await context.queryClient.query({
      ...currentUserQuery,
      staleTime: "static",
    });
    if (user) throw redirect({ to: "/" });
  },
  component: RegisterPage,
});

function RegisterPage() {
  const search = Route.useSearch();

  return (
    <AuthForm
      mode="register"
      redirect={search.redirect}
      footer={
        <>
          Already have an account?{" "}
          <Link to="/login" search={search} className="text-primary underline">
            Log in
          </Link>
        </>
      }
    />
  );
}
