using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moneytracker.Server.Domain;
using moneytracker.Server.Domain.DTOs;
using moneytracker.Server.Domain.Entities;
using moneytracker.Server.Infrastructure.Persistence;

namespace moneytracker.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SubscriptionController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<GetSubscription>> GetAll()
        {
            var subscriptions = _context.Subscriptions
            .AsNoTracking()
            .Select(s => new GetSubscription
            (
                s.Name,
                s.Amount,
                s.StartDate,
                s.EndDate,
                s.CategoryId,
                s.Price,
                s.BillingInterval,
                s.NextBillingDate,
                s.Status
            )).ToList();
            return Ok(subscriptions);
        }

        [HttpPost]
        public ActionResult CreateSubscription(CreateSubscriptionRequest request)
        {
            var subscription = new Subscription
            {
                Name = request.Name,
                Amount = request.Amount,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CategoryId = request.CategoryId,
                Price = request.Price,
                BillingInterval = request.BillingInterval,
                NextBillingDate = request.NextBillingDate,
                Status = request.Status
            };
            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();
            return Ok();
        }
    }
}