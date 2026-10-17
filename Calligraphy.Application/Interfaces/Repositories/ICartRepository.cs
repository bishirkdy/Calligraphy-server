using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(int userId);

    Task AddCartAsync(Cart cart);

    Task AddCartItemAsync(CartItem cartItem);

    Task<CartItem?> GetCartItemAsync(int cartId, int productId);

    Task<CartItem?> GetCartItemByIdAsync(int id);

    Task<bool> UpdateCartItemQuantityAsync(
    int userId,
    int cartItemId,
    int quantity);

    Task RemoveCartItemAsync(CartItem cartItem);

    Task RemoveCartAsync(Cart cart);

    Task SaveChangesAsync();
}