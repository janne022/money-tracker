using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moneytracker.Server.Infrastructure.Persistence;

namespace moneytracker.Server.Features.Subscriptions
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DeleteSubscriptionController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var deleted = await DeleteSubscriptionHandler.Handle(new DeleteSubscriptionMessage(userClaim, id), _context);
            return deleted ? NoContent() : NotFound();
        }
    }
    public static class DeleteSubscriptionHandler
    {
        public static async Task<bool> Handle(DeleteSubscriptionMessage message, AppDbContext context)
        {
            var subscription = await context.Subscriptions
                .FirstOrDefaultAsync(s => s.Id == message.Id && s.MemberId == message.UserId);
            if (subscription == null)
            {
                return false;
            }
            context.Subscriptions.Remove(subscription);
            await context.SaveChangesAsync();
            return true;
        }
    }
    public record DeleteSubscriptionMessage
    (
        string UserId,
        int Id
    );
}
