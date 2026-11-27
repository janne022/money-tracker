import { createFileRoute } from "@tanstack/react-router";
import { useSubscriptions } from "./-api/subscriptionsQuery";
import { CategoryActions } from "./-components/category-actions";
import { AddCategoryDialog } from "./-components/category-dialog";
import { AddSubscriptionDialog } from "./-components/subscription-dialog";
import { SubscriptionGroup } from "./-components/subscription-group";

export const Route = createFileRoute("/_app/subscriptions/")({
  component: SubscriptionsPage,
});

function SubscriptionsPage() {
  const { data, isPending, isError } = useSubscriptions();

  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-wrap items-center justify-between gap-2">
        <h1 className="text-2xl font-semibold">Subscriptions</h1>
        {data && (
          <div className="flex gap-2">
            <AddCategoryDialog />
            <AddSubscriptionDialog categories={data.categories} />
          </div>
        )}
      </div>

      {isPending && <p className="text-muted-foreground">Loading…</p>}
      {isError && (
        <p role="alert" className="text-destructive">
          Could not load your subscriptions.
        </p>
      )}

      {data && (
        <>
          {data.categories.map((category) => (
            <SubscriptionGroup
              key={category.id}
              title={category.name}
              description={category.description}
              color={category.color}
              subscriptions={category.subscriptions}
              categories={data.categories}
              action={<CategoryActions category={category} />}
            />
          ))}
          {(data.uncategorized.length > 0 || data.categories.length === 0) && (
            <SubscriptionGroup
              title="Uncategorized"
              subscriptions={data.uncategorized}
              categories={data.categories}
            />
          )}
        </>
      )}
    </div>
  );
}
