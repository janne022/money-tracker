import { useState } from "react";
import type { ReactNode } from "react";
import type {
  CategorySubscriptions,
  GetSubscription,
} from "@/shared/api/generated/model";
import { Badge } from "@/shared/components/ui/badge";
import {
  Card,
  CardAction,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/shared/components/ui/card";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/shared/components/ui/table";
import { billingIntervals, formatPrice, statuses } from "./labels";
import { SubscriptionActions } from "./subscription-actions";
import { SubscriptionDetailsDialog } from "./subscription-details-dialog";

export function SubscriptionGroup({
  title,
  description,
  color,
  subscriptions,
  categories,
  action,
}: {
  title: string;
  description?: string;
  color?: string;
  subscriptions: GetSubscription[];
  categories: CategorySubscriptions[];
  action?: ReactNode;
}) {
  const [selectedId, setSelectedId] = useState<GetSubscription["id"] | null>(
    null,
  );

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          {color && (
            <span
              aria-hidden
              className="size-3 rounded-full"
              style={{ backgroundColor: color }}
            />
          )}
          {title}
        </CardTitle>
        {description && <CardDescription>{description}</CardDescription>}
        {action && <CardAction>{action}</CardAction>}
      </CardHeader>
      <CardContent>
        {subscriptions.length === 0 ? (
          <p className="text-muted-foreground">No subscriptions yet.</p>
        ) : (
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Name</TableHead>
                <TableHead>Price</TableHead>
                <TableHead>Billed</TableHead>
                <TableHead>Next billing</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>
                  <span className="sr-only">Actions</span>
                </TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {subscriptions.map((subscription) => (
                <TableRow
                  key={subscription.id}
                  className="cursor-pointer"
                  onClick={() => setSelectedId(subscription.id)}
                >
                  <TableCell className="font-medium">
                    <button type="button" className="hover:underline">
                      {subscription.name}
                    </button>
                  </TableCell>
                  <TableCell>{formatPrice(subscription.price)}</TableCell>
                  <TableCell>
                    {billingIntervals[subscription.billingInterval]}
                  </TableCell>
                  <TableCell>{subscription.nextBillingDate}</TableCell>
                  <TableCell>
                    <Badge variant="secondary">
                      {statuses[subscription.status]}
                    </Badge>
                  </TableCell>
                  <TableCell
                    className="cursor-default text-right"
                    onClick={(event) => event.stopPropagation()}
                  >
                    <SubscriptionActions
                      categories={categories}
                      subscription={subscription}
                    />
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        )}
      </CardContent>
      <SubscriptionDetailsDialog
        subscription={subscriptions.find(({ id }) => id === selectedId)}
        category={title}
        onClose={() => setSelectedId(null)}
      />
    </Card>
  );
}
