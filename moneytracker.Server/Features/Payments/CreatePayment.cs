using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moneytracker.Server.Domain.Entities;
using moneytracker.Server.Infrastructure.Persistence;

namespace moneytracker.Server.Features.Payments
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CreatePaymentController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<CreatePaymentResponse>> CreatePayment(CreatePaymentRequest request)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var response = await CreatePaymentHandler.Handle(new CreatePaymentMessage(
                userClaim,
                request.SubscriptionId,
                request.Amount,
                request.PaymentDate
            ), _context);
            if (response == null)
            {
                ModelState.AddModelError(nameof(request.SubscriptionId), "Subscription not found.");
                return ValidationProblem(ModelState);
            }
            return Ok(response);
        }
    }
    public static class CreatePaymentHandler
    {
        public static async Task<CreatePaymentResponse?> Handle(CreatePaymentMessage message, AppDbContext context)
        {
            var ownsSubscription = await context.Subscriptions
                .AnyAsync(s => s.Id == message.SubscriptionId && s.MemberId == message.UserId);
            if (!ownsSubscription)
            {
                return null;
            }
            var payment = new Payment
            {
                SubscriptionId = message.SubscriptionId,
                Amount = message.Amount,
                PaymentDate = message.PaymentDate
            };
            context.Payments.Add(payment);
            await context.SaveChangesAsync();
            return new CreatePaymentResponse(
                payment.Id,
                payment.SubscriptionId,
                payment.Amount,
                payment.PaymentDate
            );
        }
    }
    public record CreatePaymentRequest
    (
        int SubscriptionId,
        decimal Amount,
        DateOnly PaymentDate
    );

    public record CreatePaymentResponse
    (
        int Id,
        int SubscriptionId,
        decimal Amount,
        DateOnly PaymentDate
    );

    public record CreatePaymentMessage
    (
        string UserId,
        int SubscriptionId,
        decimal Amount,
        DateOnly PaymentDate
    );
}
