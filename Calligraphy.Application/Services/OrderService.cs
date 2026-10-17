using Calligraphy.Application.DTOs.Order;
using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IAddressRepository _addressRepository;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IAddressRepository addressRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _addressRepository = addressRepository;
    }

    public async Task<Order?> CreateOrderAsync(
        int userId,
        CreateOrderDto dto)
    {
     
        var address = await _addressRepository
            .GetByIdAsync(dto.AddressId, userId);

        if (address == null)
            return null;

        var cart = await _cartRepository
            .GetCartByUserIdAsync(userId);

        if (cart == null || !cart.CartItems.Any())
            return null;

        var order = new Order
        {
            UserId = userId,
            ShippingAddress = address.AddressLine,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            TotalAmount = 0
        };

        foreach (var cartItem in cart.CartItems)
        {
            var orderItem = new OrderItem
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Product.Price
            };

            order.OrderItems.Add(orderItem);

            order.TotalAmount +=
                cartItem.Product.Price * cartItem.Quantity;
        }

        await _orderRepository.AddOrderAsync(order);
        await _orderRepository.SaveChangesAsync();

        await _cartRepository.RemoveCartAsync(cart);
        await _cartRepository.SaveChangesAsync();

        return order;
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _orderRepository
            .GetOrderByIdAsync(id);
    }

    public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _orderRepository
            .GetOrdersByUserIdAsync(userId);
    }
    public async Task<List<Order>> GetAllOrdersAsync(string? status = null)
    {
        return await _orderRepository
            .GetAllOrdersAsync(status);
    }

    public async Task<(List<Order> Items, int TotalCount)> GetAdminPagedAsync(
        string? search, string? status, int page, int pageSize)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var orders = await _orderRepository.GetAllOrdersAsync(status);
        IEnumerable<Order> query = orders;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(o =>
                o.Id.ToString().Contains(term, StringComparison.OrdinalIgnoreCase) ||
                (o.User != null &&
                    ((o.User.Name?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                     (o.User.Email?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false))));
        }

        var totalCount = query.Count();
        var items = query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    public async Task<bool> UpdateOrderStatusAsync(int id,string status)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null)
            return false;

        order.Status = status;
        await _orderRepository.SaveChangesAsync();
        return true;

    }

}