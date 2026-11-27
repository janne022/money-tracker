using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moneytracker.Server.Domain;
using moneytracker.Server.Infrastructure.Persistence;

namespace moneytracker.Server.Features.Subscriptions
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateSubscriptionController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UpdateSubscriptionResponse>> UpdateSubscription(int id, UpdateSubscriptionRequest request)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var result = await UpdateSubscriptionHandler.Handle(new UpdateSubscriptionMessage(
                userClaim,
                id,
                request.Name,
                request.Amount,
                request.StartDate,
                request.EndDate,
                request.CategoryId,
                request.Price,
                request.BillingInterval,
                request.NextBillingDate,
                request.Status!.Value
            ), _context);
            switch (result.Outcome)
            {
                case UpdateSubscriptionOutcome.NotFound:
                    return NotFound();
                case UpdateSubscriptionOutcome.CategoryNotFound:
                    ModelState.AddModelError(nameof(request.CategoryId), "Category not found.");
                    return ValidationProblem(ModelState);
                default:
                    return Ok(result.Response);
            }
        }
    }
    public static class UpdateSubscriptionHandler
    {
        public static async Task<UpdateSubscriptionResult> Handle(UpdateSubscriptionMessage message, AppDbContext context)
        {
            var subscription = await context.Subscriptions
                .FirstOrDefaultAsync(s => s.Id == message.Id && s.MemberId == message.UserId);
            if (subscription == null)
            {
                return new UpdateSubscriptionResult(UpdateSubscriptionOutcome.NotFound);
            }
            if (message.CategoryId != null && !await context.Categories
                .AnyAsync(c => c.Id == message.CategoryId && c.MemberId == message.UserId))
            {
                return new UpdateSubscriptionResult(UpdateSubscriptionOutcome.CategoryNotFound);
            }
            subscription.Name = message.Name;
            subscription.Amount = message.Amount;
            subscription.StartDate = message.StartDate;
            subscription.EndDate = message.EndDate;
            subscription.CategoryId = message.CategoryId;
            subscription.Price = message.Price;
            subscription.BillingInterval = message.BillingInterval;
            subscription.NextBillingDate = message.NextBillingDate;
            subscription.Status = message.Status;
            await context.SaveChangesAsync();
            return new UpdateSubscriptionResult(UpdateSubscriptionOutcome.Updated, new UpdateSubscriptionResponse(
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
            ));
        }
    }
    public record UpdateSubscriptionRequest
    (
        string Name,
        decimal Amount,
        DateOnly StartDate,
        DateOnly? EndDate,
        int? CategoryId,
        decimal Price,
        BillingInterval BillingInterval,
        DateOnly NextBillingDate,
        [Required][EnumDataType(typeof(Status))] Status? Status
    );

    public record UpdateSubscriptionResponse
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

    public record UpdateSubscriptionMessage
    (
        string UserId,
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

    public record UpdateSubscriptionResult
    (
        UpdateSubscriptionOutcome Outcome,
        UpdateSubscriptionResponse? Response = null
    );

    public enum UpdateSubscriptionOutcome
    {
        Updated,
        NotFound,
        CategoryNotFound
    }
}
