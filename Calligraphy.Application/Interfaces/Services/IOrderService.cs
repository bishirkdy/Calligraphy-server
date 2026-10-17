using Calligraphy.Application.DTOs.Order;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Services;

public interface IOrderService
{
    Task<Order?> CreateOrderAsync(int userId, CreateOrderDto dto);

    Task<Order?> GetOrderByIdAsync(int id);

    Task<List<Order>> GetOrdersByUserIdAsync(int userId);

    Task<List<Order>> GetAllOrdersAsync(string? status = null);

    Task<(List<Order> Items, int TotalCount)> GetAdminPagedAsync(
        string? search, string? status, int page, int pageSize);

    Task<bool> UpdateOrderStatusAsync(int id, string status);
}