using Calligraphy.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUserController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminUserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers(
    [FromQuery] string? search = null,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 5)
    {
        // Validate pagination parameters
        if (page < 1)
            return BadRequest("Page must be greater than 0.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("Page size must be between 1 and 100.");

        // Fetch filtered and paginated users from the repository
        var (users, totalCount) = await _userService.GetUsersPagedAsync(
            search,
            page,
            pageSize);

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        var result = new
        {
            Users = users.Select(user => new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Role,
                user.IsBlocked
            }),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound("User not found.");

        if (user.Role == "Admin")
            return NotFound("User not found.");

        return Ok(new
        {
            user.Id,
            user.Name,
            user.Email,
            user.Role
        });
    }
    [HttpPut("{id}/block")]
    public async Task<IActionResult> BlockUser(int id)
    {
        var blocked = await _userService.BlockAsync(id);

        if (!blocked)
            return NotFound("User not found.");

        return Ok(new
        {
            message = "User blocked successfully."
        });
    }
    [HttpPut("{id}/unblock")]
    public async Task<IActionResult> UnblockUser(int id)
    {
        var unblocked = await _userService.UnblockAsync(id);
        if (!unblocked)
            return NotFound("user not found");
        return Ok(new
        {
            message = "user unblocked succesfully."
        });
    }
}