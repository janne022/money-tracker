import { useState } from "react";
import type { ComponentProps, ReactNode, SubmitEvent } from "react";
import { useRouter } from "@tanstack/react-router";
import { z } from "zod";
import { Button } from "@/shared/components/ui/button";
import { Input } from "@/shared/components/ui/input";
import { loginSchema, registerSchema } from "../-api/schemas";
import { safeRedirect } from "../-api/search";
import { toFormError } from "../-api/toFormError";
import { useLogin } from "../-api/useLogin";
import { useRegister } from "../-api/useRegister";

function Field({
  label,
  hint,
  errors,
  ...props
}: ComponentProps<"input"> & {
  label: string;
  hint?: string;
  errors?: string[];
  name: string;
}) {
  const hintId = hint ? `${props.name}-hint` : undefined;
  const errorId = errors ? `${props.name}-error` : undefined;

  return (
    <div className="flex flex-col gap-1.5">
      <label htmlFor={props.name} className="text-sm font-medium">
        {label}
      </label>
      <Input
        id={props.name}
        aria-describedby={[hintId, errorId].filter(Boolean).join(" ") || undefined}
        aria-invalid={errors ? true : undefined}
        className="h-10"
        {...props}
      />
      {hint && (
        <p id={hintId} className="text-xs text-muted-foreground">
          {hint}
        </p>
      )}
      {errors && (
        <p id={errorId} className="text-xs text-destructive">
          {errors.join(" ")}
        </p>
      )}
    </div>
  );
}

export function AuthForm({
  mode,
  redirect,
  footer,
}: {
  mode: "login" | "register";
  redirect?: string;
  footer: ReactNode;
}) {
  const router = useRouter();
  const login = useLogin();
  const register = useRegister();
  const isRegister = mode === "register";
  const [errors, setErrors] = useState<Record<string, string[] | undefined>>({});

  const error = register.error ?? login.error;
  const isPending = register.isPending || login.isPending;

  async function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();
    const schema = isRegister ? registerSchema : loginSchema;
    login.reset();
    register.reset();
    const result = schema.safeParse(
      Object.fromEntries(new FormData(event.currentTarget)),
    );
    if (!result.success) {
      setErrors(z.flattenError(result.error).fieldErrors);
      return;
    }
    setErrors({});
    const { email, password } = result.data;
    try {
      if (isRegister) await register.mutateAsync({ email, password });
      await login.mutateAsync({ email, password });
      router.history.push(safeRedirect(redirect));
    } catch {
      return;
    }
  }

  const title = isRegister ? "Create account" : "Log in";

  return (
    <main className="flex min-h-svh items-center justify-center p-6">
      <div className="flex w-full max-w-sm flex-col gap-6">
        <h1 className="text-2xl font-semibold">{title}</h1>
        <form
          noValidate
          onSubmit={handleSubmit}
          className="flex flex-col gap-4"
        >
          <Field
            label="Email"
            name="email"
            type="email"
            autoComplete="email"
            errors={errors.email}
          />
          <Field
            label="Password"
            name="password"
            type="password"
            autoComplete={isRegister ? "new-password" : "current-password"}
            errors={errors.password}
            hint={
              isRegister
                ? "At least 8 characters, with an uppercase letter, a lowercase letter, a number and a symbol."
                : undefined
            }
          />
          {isRegister && (
            <Field
              label="Confirm password"
              name="confirmPassword"
              type="password"
              autoComplete="new-password"
              errors={errors.confirmPassword}
            />
          )}
          {error && (
            <p role="alert" className="text-sm whitespace-pre-line text-destructive">
              {toFormError(error).message}
            </p>
          )}
          <Button type="submit" className="h-10" disabled={isPending}>
            {isPending ? "Please wait…" : title}
          </Button>
        </form>
        <p className="text-sm text-muted-foreground">{footer}</p>
      </div>
    </main>
  );
}
