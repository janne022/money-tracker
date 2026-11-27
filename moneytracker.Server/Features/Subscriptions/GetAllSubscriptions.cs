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
    public class GetAllSubscriptionsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<GetAllSubscriptionsResponse>> GetAll()
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var response = await GetAllSubscriptionsHandler.Handle(new GetAllSubscriptionsMessage(userClaim), _context);
            return Ok(response);
        }
    }
    public static class GetAllSubscriptionsHandler
    {
        public static async Task<GetAllSubscriptionsResponse> Handle(GetAllSubscriptionsMessage message, AppDbContext context)
        {
            var subscriptions = await context.Subscriptions
                .AsNoTracking()
                .Where(s => s.MemberId == message.UserId)
                .OrderBy(s => s.Name)
                .Select(s => new GetSubscription
                (
                    s.Id,
                    s.Name,
                    s.Amount,
                    s.StartDate,
                    s.EndDate,
                    s.CategoryId,
                    s.Price,
                    s.BillingInterval,
                    s.NextBillingDate,
                    s.Status
                ))
                .ToListAsync();

            var categories = await context.Categories
                .AsNoTracking()
                .Where(c => c.MemberId == message.UserId)
                .OrderBy(c => c.Name)
                .ToListAsync();

            // Lookup to prevent throwing, then group by category and uncategorized
            var byCategory = subscriptions.ToLookup(s => s.CategoryId);

            return new GetAllSubscriptionsResponse(
                [.. categories.Select(c => new CategorySubscriptions
                (
                    c.Id,
                    c.Name,
                    c.Description,
                    c.Icon,
                    c.Color,
                    [.. byCategory[c.Id]]
                ))],
                [.. byCategory[null]]
            );
        }
    }

    public record GetSubscription
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

    public record CategorySubscriptions
    (
        int Id,
        string Name,
        string Description,
        string Icon,
        string Color,
        IReadOnlyList<GetSubscription> Subscriptions
    );

    public record GetAllSubscriptionsResponse
    (
        IReadOnlyList<CategorySubscriptions> Categories,
        IReadOnlyList<GetSubscription> Uncategorized
    );

    public record GetAllSubscriptionsMessage
    (
        string UserId
    );
}
