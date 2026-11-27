using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace moneytracker.Server.Domain.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public required string MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public decimal Price { get; set; }
        public BillingInterval BillingInterval { get; set; }
        public DateOnly NextBillingDate { get; set; }
        public Status Status { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}