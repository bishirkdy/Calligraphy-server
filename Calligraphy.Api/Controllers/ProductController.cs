using Calligraphy.Application.DTOs.Common;
using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Application.DTOs.Product;
using Calligraphy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? categoryId,
        [FromQuery] string? style,
        [FromQuery] string? sort)
    {
        var products = await _productService.GetFilteredAsync(
            categoryId,
            style,
            sort);

        return Ok(
            ApiResponse<IEnumerable<Product>>.Ok(
                products,
                "Products retrieved successfully."
            )
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound(
                ApiResponse<Product>.Fail("Product not found.")
            );
        }

        return Ok(
            ApiResponse<Product>.Ok(
                product,
                "Product retrieved successfully."
            )
        );
    }
}