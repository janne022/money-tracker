import { Outlet, createRootRoute } from "@tanstack/react-router";
import {
  SidebarProvider,
  SidebarTrigger,
  SidebarInset,
} from "../../components/ui/sidebar";
import { AppSidebar } from "../../components/app-sidebar";

export const Route = createRootRoute({
  component: RootComponent,
});

function RootComponent() {
  return (
    <SidebarProvider>
      <AppSidebar />
      <SidebarInset>
        <main className="flex-1 p-6">
          <SidebarTrigger />
          <Outlet />
        </main>
      </SidebarInset>
    </SidebarProvider>
  );
}
