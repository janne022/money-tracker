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
    public class GetAllCategoriesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategory>>> GetAll()
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var response = await GetAllCategoriesHandler.Handle(new GetAllCategoriesMessage(userClaim), _context);
            return Ok(response);
        }
    }
    public static class GetAllCategoriesHandler
    {
        public static async Task<IEnumerable<GetCategory>> Handle(GetAllCategoriesMessage message, AppDbContext context)
        {
            return await context.Categories
                .AsNoTracking()
                .Where(c => c.MemberId == message.UserId)
                .OrderBy(c => c.Name)
                .Select(c => new GetCategory
                (
                    c.Id,
                    c.Name,
                    c.Description,
                    c.Icon,
                    c.Color
                ))
                .ToListAsync();
        }
    }

    public record GetCategory
    (
        int Id,
        string Name,
        string Description,
        string Icon,
        string Color
    );

    public record GetAllCategoriesMessage
    (
        string UserId
    );
}
