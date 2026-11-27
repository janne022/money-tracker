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
        public async Task<ActionResult<IEnumerable<GetSubscription>>> GetAll()
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
        public static async Task<IEnumerable<GetSubscription>> Handle(GetAllSubscriptionsMessage message, AppDbContext context)
        {
            return await context.Subscriptions
                .AsNoTracking()
                .Where(s => s.MemberId == message.UserId)
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

    public record GetAllSubscriptionsMessage
    (
        string UserId
    );
}
