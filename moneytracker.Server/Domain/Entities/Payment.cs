using System;

namespace moneytracker.Server.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateOnly PaymentDate { get; set; }
    }
}
