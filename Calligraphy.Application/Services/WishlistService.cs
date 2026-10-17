using Calligraphy.Application.DTOs.Wishlist;
using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Application.Interfaces.Services;

namespace Calligraphy.Application.Services;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;

    public WishlistService(IWishlistRepository wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }

    public async Task<IEnumerable<WishlistItemDto>> GetWishlistItemsAsync(
        int userId)
    {
        return await _wishlistRepository
            .GetWishlistItemsAsync(userId);
    }

    public async Task<WishlistItemDto> AddToWishlistAsync(
        int userId,
        int productId)
    {
        return await _wishlistRepository
            .AddToWishlistAsync(userId, productId);
    }

    public async Task<bool> RemoveFromWishlistAsync(
        int userId,
        int productId)
    {
        return await _wishlistRepository
            .RemoveFromWishlistAsync(userId, productId);
    }
}