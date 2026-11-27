import type { ComponentProps } from "react";
import { Field, FieldError, FieldLabel } from "@/shared/components/ui/field";
import { Input } from "@/shared/components/ui/input";

export function FormField({
  label,
  errors,
  ...props
}: ComponentProps<"input"> & {
  label: string;
  name: string;
  errors?: string[];
}) {
  return (
    <Field data-invalid={errors ? true : undefined}>
      <FieldLabel htmlFor={props.name}>{label}</FieldLabel>
      <Input
        id={props.name}
        aria-invalid={errors ? true : undefined}
        {...props}
      />
      {errors && <FieldError>{errors.join(" ")}</FieldError>}
    </Field>
  );
}
