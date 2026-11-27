import { useLocation } from "@tanstack/react-router";
import { navItems } from "./nav-items";

export function AppBar() {
  const pathname = useLocation({ select: (location) => location.pathname });

  return (
    <nav className="sticky bottom-0 z-10 border-t border-sidebar-border bg-sidebar text-sidebar-foreground md:hidden">
      <ul className="grid grid-cols-4">
        {navItems.map((item) => (
          <li key={item.title}>
            <a
              href={item.url}
              aria-current={pathname === item.url ? "page" : undefined}
              className="flex h-16 flex-col items-center justify-center gap-1 text-xs text-muted-foreground outline-hidden focus-visible:ring-2 focus-visible:ring-sidebar-ring focus-visible:ring-inset aria-[current=page]:font-medium aria-[current=page]:text-sidebar-primary"
            >
              <item.icon className="size-5" />
              <span className="max-w-full truncate px-1">{item.title}</span>
            </a>
          </li>
        ))}
      </ul>
    </nav>
  );
}
