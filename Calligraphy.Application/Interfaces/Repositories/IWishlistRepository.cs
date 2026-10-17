using Calligraphy.Application.DTOs.Wishlist;

namespace Calligraphy.Application.Interfaces.Repositories;

public interface IWishlistRepository
{
    Task<IEnumerable<WishlistItemDto>> GetWishlistItemsAsync(int userId);

    Task<WishlistItemDto> AddToWishlistAsync(
        int userId,
        int productId);

    Task<bool> RemoveFromWishlistAsync(
        int userId,
        int productId);
}