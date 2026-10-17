using Calligraphy.Application.DTOs.Common;
using Calligraphy.Application.DTOs.Product;
using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = "Admin")]
public class AdminProductController : ControllerBase
{
    private readonly IProductService _productService;

    public AdminProductController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] string? search,
    [FromQuery] bool showDeleted = false,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 5)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var (items, totalCount) = await _productService.GetAdminPagedAsync(
            search, showDeleted, page, pageSize);

        var result = new
        {
            items,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return Ok(
            ApiResponse<object>.Ok(
                result,
                "Admin products retrieved successfully."
            )
        );
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
    {
        try
        {
            var product = await _productService.CreateAsync(dto);

            return Ok(ApiResponse<Product>.Ok(product, "Product created successfully."));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
     int id,
     [FromForm] ProductUpdateDto dto)
    {
        var updatedProduct =
            await _productService.UpdateAsync(id, dto);

        if (updatedProduct == null)
        {
            return NotFound(
                ApiResponse<Product>.Fail(
                    "Product not found."
                )
            );
        }

        return Ok(
            ApiResponse<Product>.Ok(
                updatedProduct,
                "Product updated successfully."
            )
        );
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(
                ApiResponse<object>.Fail(
                    "Product notfound")
                );
        }
        return Ok(
            ApiResponse<object>.Ok(
                null,
                "Product soft deleted")
            );
    }
    [HttpDelete("{id}/hard")]
    public async Task<IActionResult> HardDelete(int id)
    {
        var deleted = await _productService.HardDeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                ApiResponse<object>.Fail(
                    "Product notfound")
                );
        }
        return Ok(
            ApiResponse<object>.Ok(
                null,
                "Product permenently deleted")
            );
    }
    [HttpPut("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var restored = await _productService.RestoreAsync(id);

        if (!restored)
        {
            return NotFound(
                ApiResponse<object>.Fail(
                    "Product not found."
                )
            );
        }

        return Ok(
            ApiResponse<object>.Ok(
                null,
                "Product restored successfully."
            )
        );
    }


}