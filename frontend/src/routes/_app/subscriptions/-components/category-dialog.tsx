import { useState } from "react";
import type { SubmitEvent } from "react";
import { z } from "zod";
import type { CategorySubscriptions } from "@/shared/api/generated/model";
import { Button } from "@/shared/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/shared/components/ui/dialog";
import { FieldError, FieldGroup } from "@/shared/components/ui/field";
import { categorySchema } from "../-api/schemas";
import { useCreateCategory } from "../-api/useCreateCategory";
import { useUpdateCategory } from "../-api/useUpdateCategory";
import { FormField } from "./form-field";

function CategoryForm({
  category,
  onSaved,
}: {
  category?: CategorySubscriptions;
  onSaved: () => void;
}) {
  const createCategory = useCreateCategory();
  const updateCategory = useUpdateCategory();
  const { isPending, isError } = category ? updateCategory : createCategory;
  const [errors, setErrors] = useState<Record<string, string[] | undefined>>({});

  function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();
    const result = categorySchema.safeParse(
      Object.fromEntries(new FormData(event.currentTarget)),
    );
    if (!result.success) {
      setErrors(z.flattenError(result.error).fieldErrors);
      return;
    }
    setErrors({});
    const request = { ...result.data, icon: category?.icon ?? "" };
    if (category) {
      updateCategory.mutate(
        { id: category.id, request },
        { onSuccess: onSaved },
      );
    } else {
      createCategory.mutate(request, { onSuccess: onSaved });
    }
  }

  return (
    <form noValidate onSubmit={handleSubmit}>
      <FieldGroup>
        <FormField
          label="Name"
          name="name"
          defaultValue={category?.name}
          errors={errors.name}
        />
        <FormField
          label="Description (optional)"
          name="description"
          defaultValue={category?.description}
        />
        <FormField
          label="Color"
          name="color"
          type="color"
          defaultValue={category?.color || "#2563eb"}
        />
        {isError && (
          <FieldError>Could not save the category. Please try again.</FieldError>
        )}
        <Button type="submit" disabled={isPending}>
          Save category
        </Button>
      </FieldGroup>
    </form>
  );
}

export function AddCategoryDialog() {
  const [open, setOpen] = useState(false);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger render={<Button variant="outline" />}>
        Add category
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Add category</DialogTitle>
        </DialogHeader>
        <CategoryForm onSaved={() => setOpen(false)} />
      </DialogContent>
    </Dialog>
  );
}

export function EditCategoryDialog({
  category,
  open,
  onOpenChange,
}: {
  category: CategorySubscriptions;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Edit category</DialogTitle>
        </DialogHeader>
        <CategoryForm category={category} onSaved={() => onOpenChange(false)} />
      </DialogContent>
    </Dialog>
  );
}
