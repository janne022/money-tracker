using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moneytracker.Server.Domain.Entities;
using moneytracker.Server.Infrastructure.Persistence;

namespace moneytracker.Server.Features.Categories
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CreateCategoryController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<CreateCategoryResponse>> CreateCategory(CreateCategoryRequest request)
        {
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized();
            }
            var response = await CreateCategoryHandler.Handle(new CreateCategoryMessage(
                userClaim,
                request.Name,
                request.Description,
                request.Icon,
                request.Color
            ), _context);
            return Ok(response);
        }
    }
    public static class CreateCategoryHandler
    {
        public static async Task<CreateCategoryResponse> Handle(CreateCategoryMessage message, AppDbContext context)
        {
            var category = new Category
            {
                MemberId = message.UserId,
                Name = message.Name,
                Description = message.Description,
                Icon = message.Icon,
                Color = message.Color
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return new CreateCategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                category.Icon,
                category.Color
            );
        }
    }
    public record CreateCategoryRequest
    (
        string Name,
        string Description,
        [MaxLength(15)] string Icon,
        [MaxLength(10)] string Color
    );

    public record CreateCategoryResponse
    (
        int Id,
        string Name,
        string Description,
        string Icon,
        string Color
    );

    public record CreateCategoryMessage
    (
        string UserId,
        string Name,
        string Description,
        string Icon,
        string Color
    );
}
