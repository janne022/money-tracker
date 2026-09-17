using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace moneytracker.Server.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
    }
}