using Calligraphy.Application.DTOs.Cart;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Services;

public interface ICartService
{
    Task<Cart?> GetCartByUserIdAsync(int userId);

    Task<bool> AddToCartAsync(int userId, AddToCartDto dto);

    Task<bool> UpdateCartItemQuantityAsync(
    int userId,
    int cartItemId,
    int quantity);

    Task<bool> RemoveCartItemAsync(int userId, int cartItemId);

    Task<bool> RemoveCartAsync(int userId);
}