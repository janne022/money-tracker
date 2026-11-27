import type { GetSubscription } from "@/shared/api/generated/model";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/shared/components/ui/dialog";
import { billingIntervals, formatPrice, statuses } from "./labels";

export function SubscriptionDetailsDialog({
  subscription,
  category,
  onClose,
}: {
  subscription?: GetSubscription;
  category: string;
  onClose: () => void;
}) {
  const details = subscription
    ? [
        ["Price", formatPrice(subscription.price)],
        ["Billed", billingIntervals[subscription.billingInterval]],
        ["Status", statuses[subscription.status]],
        ["Category", category],
        ["Start date", subscription.startDate],
        ["Next billing date", subscription.nextBillingDate],
        ["End date", subscription.endDate ?? "None"],
      ]
    : [];

  return (
    <Dialog open={!!subscription} onOpenChange={(open) => !open && onClose()}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{subscription?.name}</DialogTitle>
        </DialogHeader>
        <dl className="grid grid-cols-[auto_1fr] gap-x-6 gap-y-2">
          {details.map(([label, value]) => (
            <div key={label} className="contents">
              <dt className="text-muted-foreground">{label}</dt>
              <dd className="text-right font-medium">{value}</dd>
            </div>
          ))}
        </dl>
      </DialogContent>
    </Dialog>
  );
}
