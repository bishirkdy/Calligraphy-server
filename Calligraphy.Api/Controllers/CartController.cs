using System.Security.Claims;
using Calligraphy.Application.DTOs.Cart;
using Calligraphy.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var cart = await _cartService.GetCartByUserIdAsync(userId.Value);

        if (cart == null)
            return NotFound("Cart not found.");

        return Ok(new
        {
            Id = cart.Id,
            UserId = cart.UserId,

            CartItems = cart.CartItems.Select(item => new
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,

                Product = new
                {
                    Id = item.Product.Id,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    ImageUrl = item.Product.ImageUrl,
                    StockQuantity = item.Product.StockQuantity
                }
            })
        });
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart(AddToCartDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        if (dto.Quantity <= 0)
            return BadRequest("Quantity must be greater than 0.");

        var result = await _cartService.AddToCartAsync(
            userId.Value,
            dto);

        if (!result)
            return NotFound("Product not found.");

        return Ok("Product added to cart.");
    }
    [HttpPut("items/{id}")]
    public async Task<IActionResult> UpdateQuantity(
        int id,
        int quantity)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        if (quantity <= 0)
            return BadRequest("Quantity must be greater than 0.");

        var result = await _cartService.UpdateCartItemQuantityAsync(
            userId.Value,
            id,
            quantity);

        if (!result)
            return NotFound("Cart item not found.");

        return Ok("Cart item quantity updated.");
    }

    [HttpDelete("items/{id}")]
    public async Task<IActionResult> RemoveItem(int id)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var result = await _cartService.RemoveCartItemAsync(
            userId.Value,
            id);

        if (!result)
            return NotFound("Cart item not found.");

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAll()
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var result = await _cartService.RemoveCartAsync(userId.Value);

        if (!result)
            return NotFound("Cart not found.");

        return NoContent();
    }

    private int? GetUserId()
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdValue == null)
            return null;

        if (!int.TryParse(userIdValue, out int userId))
            return null;

        return userId;
    }
}