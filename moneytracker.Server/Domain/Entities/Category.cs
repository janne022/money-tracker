using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace moneytracker.Server.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [MaxLength(15)]
        public string Icon { get; set; } = string.Empty;
        [MaxLength(10)]
        public string Color { get; set; } = string.Empty;
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
    }
}