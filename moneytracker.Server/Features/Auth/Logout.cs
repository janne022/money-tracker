using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using moneytracker.Server.Domain.Entities;

namespace moneytracker.Server.Features.Auth
{
    [Authorize]
    [ApiController]
    [Route("api/auth/logout")]
    public class LogoutController(SignInManager<Member> signInManager) : ControllerBase
    {
        private readonly SignInManager<Member> _signInManager = signInManager;

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await LogoutHandler.Handle(new LogoutMessage(), _signInManager);
            return NoContent();
        }
    }
    public static class LogoutHandler
    {
        public static async Task Handle(LogoutMessage message, SignInManager<Member> signInManager)
        {
            await signInManager.SignOutAsync();
        }
    }
    public record LogoutMessage;
}
