using System.Security.Claims;
using Calligraphy.Application.DTOs.Order;
using Calligraphy.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/Order
    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdValue == null)
            return Unauthorized();

        if (!int.TryParse(userIdValue, out int userId))
            return Unauthorized();

        var orders =
            await _orderService.GetOrdersByUserIdAsync(userId);

        var result = orders.Select(order => new
        {
            order.Id,
            order.UserId,
            order.TotalAmount,
            order.Status,
            order.ShippingAddress,
            order.CreatedAt,

            OrderItems = order.OrderItems.Select(item => new
            {
                item.Id,
                item.ProductId,
                ProductName = item.Product.Name,
                ImageUrl = item.Product.ImageUrl,
                item.Quantity,
                item.Price
            })
        });

        return Ok(result);
    }

    // GET: api/Order/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var order =
            await _orderService.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound("Order not found.");

        if (order.UserId != userId.Value)
            return NotFound("Order not found.");

        return Ok(new
        {
            order.Id,
            order.UserId,
            order.TotalAmount,
            order.Status,
            order.ShippingAddress,
            order.CreatedAt,

            OrderItems = order.OrderItems.Select(item => new
            {
                item.Id,
                item.ProductId,
                ProductName=item.Product.Name,
                ImageUrl = item.Product.Name,
                item.Quantity,
                item.Price,
                
            })
        });
    }

    // POST: api/Order
    // POST: api/Order
    [HttpPost]
    public async Task<IActionResult> CreateOrder(
    CreateOrderDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        try
        {
            var order =
                await _orderService.CreateOrderAsync(
                    userId.Value,
                    dto);

            if (order == null)
                return BadRequest(
                    "Invalid address or cart is empty.");

            return Ok(new
            {
                order.Id,
                order.UserId,
                order.TotalAmount,
                order.Status,
                order.ShippingAddress,
                order.CreatedAt
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
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