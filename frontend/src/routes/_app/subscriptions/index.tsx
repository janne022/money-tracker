import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_app/subscriptions/")({
  component: SubscriptionsPage,
});

function SubscriptionsPage() {
  return <h1 className="text-2xl font-semibold">Subscriptions</h1>;
}
