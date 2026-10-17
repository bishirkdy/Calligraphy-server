using Calligraphy.Application.DTOs.Cart;
using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<Cart?> GetCartByUserIdAsync(int userId)
    {
        return await _cartRepository.GetCartByUserIdAsync(userId);
    }

    public async Task<bool> AddToCartAsync(int userId, AddToCartDto dto)
    {
        if (dto.Quantity <= 0)
            return false;

        var product = await _productRepository.GetByIdAsync(dto.ProductId);

        if (product == null)
            return false;

       
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };

            await _cartRepository.AddCartAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        var existingItem = await _cartRepository.GetCartItemAsync(
            cart.Id,
            dto.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await _cartRepository.AddCartItemAsync(cartItem);
        }

        await _cartRepository.SaveChangesAsync();

        return true;
    }
    public async Task<bool> UpdateCartItemQuantityAsync(
    int userId,
    int cartItemId,
    int quantity)
    {
        if (quantity <= 0)
            return false;

        var updated = await _cartRepository
            .UpdateCartItemQuantityAsync(
                userId,
                cartItemId,
                quantity);

        if (!updated)
            return false;

        await _cartRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveCartItemAsync(
        int userId,
        int cartItemId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
            return false;

        var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

        if (cartItem == null)
            return false;

        if (cartItem.CartId != cart.Id)
            return false;

        await _cartRepository.RemoveCartItemAsync(cartItem);
        await _cartRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveCartAsync(int userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
            return false;

        await _cartRepository.RemoveCartAsync(cart);
        await _cartRepository.SaveChangesAsync();

        return true;
    }
}