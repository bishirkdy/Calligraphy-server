using Calligraphy.Application.DTOs.Common;
using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();

        return Ok(
            ApiResponse<IEnumerable<Category>>.Ok(
                categories,
                "Categories retrieved successfully."
            )
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound(
                ApiResponse<Category>.Fail(
                    "Category not found."
                )
            );
        }

        return Ok(
            ApiResponse<Category>.Ok(
                category,
                "Category retrieved successfully."
            )
        );
    }
}

