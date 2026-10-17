using Calligraphy.Application.DTOs.Wishlist;
using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Domain.Entities;
using Calligraphy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calligraphy.Infrastructure.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly ApplicationDbContext _context;

    public WishlistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WishlistItemDto>> GetWishlistItemsAsync(
        int userId)
    {
        return await _context.WishlistItems
            .Where(wi => wi.Wishlist.UserId == userId)
            .Select(wi => new WishlistItemDto
            {
                Id = wi.Id,
                ProductId = wi.ProductId,
                ProductName = wi.Product.Name,
                Price = wi.Product.Price,
                ImageUrl = wi.Product.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<WishlistItemDto> AddToWishlistAsync(
        int userId,
        int productId)
    {
        var wishlist = await _context.Wishlists
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wishlist == null)
        {
            wishlist = new Wishlist
            {
                UserId = userId
            };

            _context.Wishlists.Add(wishlist);

            await _context.SaveChangesAsync();
        }

        var existingItem = await _context.WishlistItems
            .FirstOrDefaultAsync(wi =>
                wi.WishlistId == wishlist.Id &&
                wi.ProductId == productId);

        if (existingItem != null)
        {
            throw new InvalidOperationException(
                "Product is already in the wishlist.");
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            throw new KeyNotFoundException(
                "Product not found.");
        }

        var wishlistItem = new WishlistItem
        {
            WishlistId = wishlist.Id,
            ProductId = productId
        };

        _context.WishlistItems.Add(wishlistItem);

        await _context.SaveChangesAsync();

        return new WishlistItemDto
        {
            Id = wishlistItem.Id,
            ProductId = product.Id,
            ProductName = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl
        };
    }

    public async Task<bool> RemoveFromWishlistAsync(
        int userId,
        int productId)
    {
        var wishlistItem = await _context.WishlistItems
            .FirstOrDefaultAsync(wi =>
                wi.Wishlist.UserId == userId &&
                wi.ProductId == productId);

        if (wishlistItem == null)
        {
            return false;
        }

        _context.WishlistItems.Remove(wishlistItem);

        await _context.SaveChangesAsync();

        return true;
    }
}