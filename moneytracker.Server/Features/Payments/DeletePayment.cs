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
    public class DeletePaymentController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var deleted = await DeletePaymentHandler.Handle(new DeletePaymentMessage(userClaim, id), _context);
            return deleted ? NoContent() : NotFound();
        }
    }
    public static class DeletePaymentHandler
    {
        public static async Task<bool> Handle(DeletePaymentMessage message, AppDbContext context)
        {
            var payment = await context.Payments
                .FirstOrDefaultAsync(p => p.Id == message.Id && p.Subscription.MemberId == message.UserId);
            if (payment == null)
            {
                return false;
            }
            context.Payments.Remove(payment);
            await context.SaveChangesAsync();
            return true;
        }
    }
    public record DeletePaymentMessage
    (
        string UserId,
        int Id
    );
}
