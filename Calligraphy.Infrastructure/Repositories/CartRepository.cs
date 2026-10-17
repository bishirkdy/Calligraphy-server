using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Domain.Entities;
using Calligraphy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calligraphy.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetCartByUserIdAsync(int userId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddCartAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
    }

    public async Task AddCartItemAsync(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci =>
                ci.CartId == cartId &&
                ci.ProductId == productId);
    }

    public async Task<CartItem?> GetCartItemByIdAsync(int id)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == id);
    }
    public async Task<bool> UpdateCartItemQuantityAsync(
    int userId,
    int cartItemId,
    int quantity)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci =>
                ci.Id == cartItemId &&
                ci.Cart.UserId == userId);

        if (cartItem == null)
            return false;

        cartItem.Quantity = quantity;

        return true;
    }

    public async Task RemoveCartItemAsync(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public async Task RemoveCartAsync(Cart cart)
    {
        _context.Carts.Remove(cart);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}