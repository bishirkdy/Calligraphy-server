using Calligraphy.Application.DTOs.Product;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Services;

public interface IProductService
{
    Task<Product?> GetByIdAsync(int id);

    Task<List<Product>> GetAllAsync();

    Task<List<Product>> GetFilteredAsync(int? categoryId, string? style, string? sort);

    Task<List<Product>> GetAllForAdminAsync();

    Task<(List<Product> Items, int TotalCount)> GetAdminPagedAsync(
    string? search, bool showDeleted, int page, int pageSize);

    Task<Product> CreateAsync(CreateProductDto dto);

    Task<Product?> UpdateAsync(int id, ProductUpdateDto dto);

    Task<bool> DeleteAsync(int id);

    Task<bool> HardDeleteAsync(int id);

    Task<bool> RestoreAsync(int id);
}