import { z } from "zod";
import type {
  LoginRequest,
  RegisterRequest,
} from "@/shared/api/generated/model";

const email = z.email("Enter a valid email address.");

export const loginSchema = z.object({
  email,
  password: z.string().min(1, "Enter your password."),
}) satisfies z.ZodType<LoginRequest>;

const newPassword = z
  .string()
  .min(8, "Use at least 8 characters.")
  .regex(/[A-Z]/, "Add an uppercase letter.")
  .regex(/[a-z]/, "Add a lowercase letter.")
  .regex(/[0-9]/, "Add a number.")
  .regex(/[^a-zA-Z0-9]/, "Add a symbol.");

export const registerSchema = z
  .object({
    email,
    password: newPassword,
    confirmPassword: z.string().min(1, "Confirm your password."),
  })
  .refine((values) => values.password === values.confirmPassword, {
    message: "The passwords do not match.",
    path: ["confirmPassword"],
    when: () => true,
  }) satisfies z.ZodType<RegisterRequest>;
