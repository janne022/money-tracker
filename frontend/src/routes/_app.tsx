import { Outlet, createFileRoute, redirect } from "@tanstack/react-router";
import {
  SidebarProvider,
  SidebarTrigger,
  SidebarInset,
} from "@/shared/components/ui/sidebar";
import { AppSidebar } from "@/shared/components/layout/app-sidebar";
import { AppBar } from "@/shared/components/layout/app-bar";
import { currentUserQuery } from "@/shared/api/currentUser";

export const Route = createFileRoute("/_app")({
  beforeLoad: async ({ context, location }) => {
    const user = await context.queryClient.query({
      ...currentUserQuery,
      staleTime: "static",
    });
    if (!user) {
      throw redirect({ to: "/login", search: { redirect: location.href } });
    }
  },
  component: AppLayout,
});

function AppLayout() {
  return (
    <SidebarProvider>
      <AppSidebar />
      <SidebarInset>
        <main className="flex-1 p-6">
          <SidebarTrigger className="hidden md:inline-flex" />
          <Outlet />
        </main>
        <AppBar />
      </SidebarInset>
    </SidebarProvider>
  );
}
