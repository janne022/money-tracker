using System.ComponentModel.DataAnnotations;
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
    public class UpdateCategoryController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UpdateCategoryResponse>> UpdateCategory(int id, UpdateCategoryRequest request)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var response = await UpdateCategoryHandler.Handle(new UpdateCategoryMessage(
                userClaim,
                id,
                request.Name,
                request.Description,
                request.Icon,
                request.Color
            ), _context);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
    public static class UpdateCategoryHandler
    {
        public static async Task<UpdateCategoryResponse?> Handle(UpdateCategoryMessage message, AppDbContext context)
        {
            var category = await context.Categories
                .FirstOrDefaultAsync(c => c.Id == message.Id && c.MemberId == message.UserId);
            if (category == null)
            {
                return null;
            }
            category.Name = message.Name;
            category.Description = message.Description;
            category.Icon = message.Icon;
            category.Color = message.Color;
            await context.SaveChangesAsync();
            return new UpdateCategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                category.Icon,
                category.Color
            );
        }
    }
    public record UpdateCategoryRequest
    (
        string Name,
        string Description,
        [MaxLength(15)] string Icon,
        [MaxLength(10)] string Color
    );

    public record UpdateCategoryResponse
    (
        int Id,
        string Name,
        string Description,
        string Icon,
        string Color
    );

    public record UpdateCategoryMessage
    (
        string UserId,
        int Id,
        string Name,
        string Description,
        string Icon,
        string Color
    );
}
