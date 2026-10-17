using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/admin/orders")]
[Authorize(Roles = "Admin")]
public class AdminOrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public AdminOrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 8)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var (items, totalCount) = await _orderService.GetAdminPagedAsync(
            search, status, page, pageSize);

        return Ok(new
        {
            orders = items.Select(ToAdminDto),
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound("Order not found.");

        return Ok(ToAdminDto(order));
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(
        int id,
        [FromQuery] string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("Status is required.");

        try
        {
            var updated = await _orderService.UpdateOrderStatusAsync(id, status);

            if (!updated)
                return NotFound("Order not found.");

            return Ok(new { message = "Order status updated successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private static object ToAdminDto(Order order) => new
    {
        order.Id,
        order.UserId,
        CustomerName = order.User?.Name,
        CustomerEmail = order.User?.Email,
        order.TotalAmount,
        order.Status,
        order.ShippingAddress,
        order.CreatedAt,
        PaymentMethod = order.Payment?.PaymentMethod,
        PaymentStatus = order.Payment?.Status,
        TotalQuantity = order.OrderItems.Sum(i => i.Quantity),
        OrderItems = order.OrderItems.Select(item => new
        {
            item.Id,
            item.ProductId,
            ProductName = item.Product?.Name,
            ImageUrl = item.Product?.ImageUrl,
            item.Quantity,
            item.Price
        })
    };
}