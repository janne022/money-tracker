using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace moneytracker.Server.Domain.DTOs
{
    public record GetSubscription
    (
        string Name,
        decimal Amount,
        DateTime StartDate,
        DateTime? EndDate,
        int CategoryId,
        decimal Price,
        BillingInterval BillingInterval,
        DateTime NextBillingDate,
        Status Status
    );
}