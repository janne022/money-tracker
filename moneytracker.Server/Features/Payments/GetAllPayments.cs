using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moneytracker.Server.Infrastructure.Persistence;

namespace moneytracker.Server.Features.Payments
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GetAllPaymentsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPayment>>> GetAll()
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var response = await GetAllPaymentsHandler.Handle(new GetAllPaymentsMessage(userClaim), _context);
            return Ok(response);
        }
    }
    public static class GetAllPaymentsHandler
    {
        public static async Task<IEnumerable<GetPayment>> Handle(GetAllPaymentsMessage message, AppDbContext context)
        {
            return await context.Payments
                .AsNoTracking()
                .Where(p => p.Subscription.MemberId == message.UserId)
                .OrderByDescending(p => p.PaymentDate)
                .Select(p => new GetPayment
                (
                    p.Id,
                    p.SubscriptionId,
                    p.Amount,
                    p.PaymentDate
                ))
                .ToListAsync();
        }
    }

    public record GetPayment
    (
        int Id,
        int SubscriptionId,
        decimal Amount,
        DateOnly PaymentDate
    );

    public record GetAllPaymentsMessage
    (
        string UserId
    );
}
