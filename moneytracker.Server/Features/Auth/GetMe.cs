using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using moneytracker.Server.Domain.Entities;

namespace moneytracker.Server.Features.Auth
{
    [Authorize]
    [ApiController]
    [Route("api/me")]
    public class GetMeController(UserManager<Member> userManager) : ControllerBase
    {
        private readonly UserManager<Member> _userManager = userManager;

        [HttpGet]
        public async Task<ActionResult<MeResponse>> GetMe()
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var response = await GetMeHandler.Handle(new GetMeMessage(userClaim), _userManager);
            if (response == null)
            {
                return Unauthorized();
            }
            return Ok(response);
        }
    }
    public static class GetMeHandler
    {
        public static async Task<MeResponse?> Handle(GetMeMessage message, UserManager<Member> userManager)
        {
            var member = await userManager.FindByIdAsync(message.UserId);
            if (member == null)
            {
                return null;
            }
            return new MeResponse(
                member.Id,
                member.Email!,
                member.FirstName,
                member.LastName
            );
        }
    }
    public record MeResponse
    (
        string Id,
        string Email,
        string FirstName,
        string LastName
    );

    public record GetMeMessage
    (
        string UserId
    );
}
