import { useState } from "react";
import type { SubmitEvent } from "react";
import { z } from "zod";
import type {
  CategorySubscriptions,
  GetSubscription,
} from "@/shared/api/generated/model";
import { Button } from "@/shared/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/shared/components/ui/dialog";
import {
  Field,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/shared/components/ui/field";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/shared/components/ui/select";
import { subscriptionSchema } from "../-api/schemas";
import { useCreateSubscription } from "../-api/useCreateSubscription";
import { useUpdateSubscription } from "../-api/useUpdateSubscription";
import { FormField } from "./form-field";
import { billingIntervals, statuses } from "./labels";

type Option = { value: string; label: string };

function SelectField({
  label,
  name,
  defaultValue,
  options,
}: {
  label: string;
  name: string;
  defaultValue: string;
  options: Option[];
}) {
  return (
    <Field>
      <FieldLabel htmlFor={name}>{label}</FieldLabel>
      <Select name={name} defaultValue={defaultValue} items={options}>
        <SelectTrigger id={name} className="w-full">
          <SelectValue />
        </SelectTrigger>
        <SelectContent>
          {options.map((option) => (
            <SelectItem key={option.value} value={option.value}>
              {option.label}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
    </Field>
  );
}

function SubscriptionForm({
  categories,
  subscription,
  onSaved,
}: {
  categories: CategorySubscriptions[];
  subscription?: GetSubscription;
  onSaved: () => void;
}) {
  const createSubscription = useCreateSubscription();
  const updateSubscription = useUpdateSubscription();
  const { isPending, isError } = subscription
    ? updateSubscription
    : createSubscription;
  const [errors, setErrors] = useState<Record<string, string[] | undefined>>({});
  const today = new Date().toLocaleDateString("en-CA");

  function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();
    const result = subscriptionSchema.safeParse(
      Object.fromEntries(new FormData(event.currentTarget)),
    );
    if (!result.success) {
      setErrors(z.flattenError(result.error).fieldErrors);
      return;
    }
    setErrors({});
    const request = {
      ...result.data,
      amount: result.data.price,
      endDate: subscription?.endDate ?? null,
    };
    if (subscription) {
      updateSubscription.mutate(
        { id: subscription.id, request },
        { onSuccess: onSaved },
      );
    } else {
      createSubscription.mutate(request, { onSuccess: onSaved });
    }
  }

  return (
    <form noValidate onSubmit={handleSubmit}>
      <FieldGroup>
        <FormField
          label="Name"
          name="name"
          defaultValue={subscription?.name}
          errors={errors.name}
        />
        <FormField
          label="Price"
          name="price"
          type="number"
          inputMode="decimal"
          min="0"
          step="0.01"
          defaultValue={subscription?.price}
          errors={errors.price}
        />
        <SelectField
          label="Billed"
          name="billingInterval"
          defaultValue={String(subscription?.billingInterval ?? 2)}
          options={billingIntervals.map((label, index) => ({
            value: String(index),
            label,
          }))}
        />
        <FormField
          label="Start date"
          name="startDate"
          type="date"
          defaultValue={subscription?.startDate ?? today}
          errors={errors.startDate}
        />
        <FormField
          label="Next billing date"
          name="nextBillingDate"
          type="date"
          defaultValue={subscription?.nextBillingDate ?? today}
          errors={errors.nextBillingDate}
        />
        <SelectField
          label="Category"
          name="categoryId"
          defaultValue={String(subscription?.categoryId ?? "none")}
          options={[
            { value: "none", label: "Uncategorized" },
            ...categories.map((category) => ({
              value: String(category.id),
              label: category.name,
            })),
          ]}
        />
        <SelectField
          label="Status"
          name="status"
          defaultValue={String(subscription?.status ?? 0)}
          options={statuses.map((label, index) => ({
            value: String(index),
            label,
          }))}
        />
        {isError && (
          <FieldError>
            Could not save the subscription. Please try again.
          </FieldError>
        )}
        <Button type="submit" disabled={isPending}>
          Save subscription
        </Button>
      </FieldGroup>
    </form>
  );
}

export function AddSubscriptionDialog({
  categories,
}: {
  categories: CategorySubscriptions[];
}) {
  const [open, setOpen] = useState(false);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger render={<Button />}>Add subscription</DialogTrigger>
      <DialogContent className="max-h-[90svh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Add subscription</DialogTitle>
        </DialogHeader>
        <SubscriptionForm
          categories={categories}
          onSaved={() => setOpen(false)}
        />
      </DialogContent>
    </Dialog>
  );
}

export function EditSubscriptionDialog({
  categories,
  subscription,
  open,
  onOpenChange,
}: {
  categories: CategorySubscriptions[];
  subscription: GetSubscription;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-h-[90svh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Edit subscription</DialogTitle>
        </DialogHeader>
        <SubscriptionForm
          categories={categories}
          subscription={subscription}
          onSaved={() => onOpenChange(false)}
        />
      </DialogContent>
    </Dialog>
  );
}
