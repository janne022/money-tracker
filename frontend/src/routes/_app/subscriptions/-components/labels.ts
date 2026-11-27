export const billingIntervals = ["Daily", "Weekly", "Monthly", "Yearly"];
export const statuses = ["Active", "Paused", "Canceled"];

const priceFormat = new Intl.NumberFormat(undefined, {
  minimumFractionDigits: 2,
  maximumFractionDigits: 2,
});

export function formatPrice(price: number | string) {
  return priceFormat.format(Number(price));
}
