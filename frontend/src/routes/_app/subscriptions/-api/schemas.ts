import { z } from "zod";

const name = z.string().trim().min(1, "Enter a name.");
const date = z.iso.date("Pick a date.");

export const subscriptionSchema = z.object({
  name,
  price: z.coerce.number().positive("Enter a price above 0."),
  billingInterval: z.coerce.number().int(),
  status: z.coerce.number().int(),
  startDate: date,
  nextBillingDate: date,
  categoryId: z
    .string()
    .transform((value) => (value === "none" ? null : Number(value))),
});

export const categorySchema = z.object({
  name,
  description: z.string().trim(),
  color: z.string().max(10),
});
