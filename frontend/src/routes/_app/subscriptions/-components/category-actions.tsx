import { useState } from "react";
import type { CategorySubscriptions } from "@/shared/api/generated/model";
import { useDeleteCategory } from "../-api/useDeleteCategory";
import { ActionsMenu } from "./actions-menu";
import { EditCategoryDialog } from "./category-dialog";
import { DeleteDialog } from "./delete-dialog";

export function CategoryActions({
  category,
}: {
  category: CategorySubscriptions;
}) {
  const [dialog, setDialog] = useState<"edit" | "delete" | null>(null);
  const deleteCategory = useDeleteCategory();

  return (
    <>
      <ActionsMenu
        name={category.name}
        onEdit={() => setDialog("edit")}
        onDelete={() => {
          deleteCategory.reset();
          setDialog("delete");
        }}
      />
      <EditCategoryDialog
        category={category}
        open={dialog === "edit"}
        onOpenChange={(open) => setDialog(open ? "edit" : null)}
      />
      <DeleteDialog
        open={dialog === "delete"}
        onOpenChange={(open) => setDialog(open ? "delete" : null)}
        name={category.name}
        description="This deletes the category. Its subscriptions are kept and become uncategorized."
        onConfirm={() => deleteCategory.mutateAsync(category.id)}
        isPending={deleteCategory.isPending}
        isError={deleteCategory.isError}
      />
    </>
  );
}
