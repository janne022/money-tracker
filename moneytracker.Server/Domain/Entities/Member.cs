using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace moneytracker.Server.Domain.Entities
{
    public class Member : IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}