using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moneytracker.Server.Domain;
using moneytracker.Server.Domain.Entities;
using moneytracker.Server.Infrastructure.Persistence;

namespace moneytracker.Server.Features.Subscriptions
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CreateSubscriptionController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<CreateSubscriptionResponse>> CreateSubscription(CreateSubscriptionRequest request)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var response = await CreateSubscriptionHandler.Handle(new CreateSubscriptionMessage(
                userClaim,
                request.Name,
                request.Amount,
                request.StartDate,
                request.EndDate,
                request.CategoryId,
                request.Price,
                request.BillingInterval,
                request.NextBillingDate,
                request.Status
            ), _context);
            if (response == null)
            {
                ModelState.AddModelError(nameof(request.CategoryId), "Category not found.");
                return ValidationProblem(ModelState);
            }
            return Ok(response);
        }
    }
    public static class CreateSubscriptionHandler
    {
        public static async Task<CreateSubscriptionResponse?> Handle(CreateSubscriptionMessage message, AppDbContext context)
        {
            if (message.CategoryId != null && !await context.Categories
                .AnyAsync(c => c.Id == message.CategoryId && c.MemberId == message.UserId))
            {
                return null;
            }
            var subscription = new Subscription
            {
                MemberId = message.UserId,
                Name = message.Name,
                Amount = message.Amount,
                StartDate = message.StartDate,
                EndDate = message.EndDate,
                CategoryId = message.CategoryId,
                Price = message.Price,
                BillingInterval = message.BillingInterval,
                NextBillingDate = message.NextBillingDate,
                Status = message.Status
            };
            context.Subscriptions.Add(subscription);
            await context.SaveChangesAsync();
            return new CreateSubscriptionResponse(
                subscription.Id,
                subscription.Name,
                subscription.Amount,
                subscription.StartDate,
                subscription.EndDate,
                subscription.CategoryId,
                subscription.Price,
                subscription.BillingInterval,
                subscription.NextBillingDate,
                subscription.Status
            );
        }
    }
    public record CreateSubscriptionRequest
    (
        string Name,
        decimal Amount,
        DateOnly StartDate,
        DateOnly? EndDate,
        int? CategoryId,
        decimal Price,
        BillingInterval BillingInterval,
        DateOnly NextBillingDate,
        Status Status
    );

    public record CreateSubscriptionResponse
    (
        int Id,
        string Name,
        decimal Amount,
        DateOnly StartDate,
        DateOnly? EndDate,
        int? CategoryId,
        decimal Price,
        BillingInterval BillingInterval,
        DateOnly NextBillingDate,
        Status Status
    );

    public record CreateSubscriptionMessage
    (
        string UserId,
        string Name,
        decimal Amount,
        DateOnly StartDate,
        DateOnly? EndDate,
        int? CategoryId,
        decimal Price,
        BillingInterval BillingInterval,
        DateOnly NextBillingDate,
        Status Status
    );
}