import { useState } from "react";
import type {
  CategorySubscriptions,
  GetSubscription,
} from "@/shared/api/generated/model";
import { useDeleteSubscription } from "../-api/useDeleteSubscription";
import { ActionsMenu } from "./actions-menu";
import { DeleteDialog } from "./delete-dialog";
import { EditSubscriptionDialog } from "./subscription-dialog";

export function SubscriptionActions({
  categories,
  subscription,
}: {
  categories: CategorySubscriptions[];
  subscription: GetSubscription;
}) {
  const [dialog, setDialog] = useState<"edit" | "delete" | null>(null);
  const deleteSubscription = useDeleteSubscription();

  return (
    <>
      <ActionsMenu
        name={subscription.name}
        onEdit={() => setDialog("edit")}
        onDelete={() => {
          deleteSubscription.reset();
          setDialog("delete");
        }}
      />
      <EditSubscriptionDialog
        categories={categories}
        subscription={subscription}
        open={dialog === "edit"}
        onOpenChange={(open) => setDialog(open ? "edit" : null)}
      />
      <DeleteDialog
        open={dialog === "delete"}
        onOpenChange={(open) => setDialog(open ? "delete" : null)}
        name={subscription.name}
        description="This permanently deletes the subscription and its payment history."
        onConfirm={() => deleteSubscription.mutateAsync(subscription.id)}
        isPending={deleteSubscription.isPending}
        isError={deleteSubscription.isError}
      />
    </>
  );
}
