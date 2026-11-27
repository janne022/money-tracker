using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moneytracker.Server.Infrastructure.Persistence;

namespace moneytracker.Server.Features.Categories
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DeleteCategoryController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var deleted = await DeleteCategoryHandler.Handle(new DeleteCategoryMessage(userClaim, id), _context);
            return deleted ? NoContent() : NotFound();
        }
    }
    public static class DeleteCategoryHandler
    {
        public static async Task<bool> Handle(DeleteCategoryMessage message, AppDbContext context)
        {
            var category = await context.Categories
                .FirstOrDefaultAsync(c => c.Id == message.Id && c.MemberId == message.UserId);
            if (category == null)
            {
                return false;
            }
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return true;
        }
    }
    public record DeleteCategoryMessage
    (
        string UserId,
        int Id
    );
}
