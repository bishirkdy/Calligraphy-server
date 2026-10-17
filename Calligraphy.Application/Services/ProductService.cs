using Calligraphy.Application.DTOs.Product;
using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICloudinaryService _cloudinaryService;

    public ProductService(
        IProductRepository productRepository,
        ICloudinaryService cloudinaryService)
    {
        _productRepository = productRepository;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }


    public async Task<List<Product>> GetFilteredAsync(
        int? categoryId, string? style, string? sort)
    {
        var products = await _productRepository.GetAllAsync();
        IEnumerable<Product> query = products;

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(style))
        {
            var styleTerm = style.Trim();
            query = query.Where(p => p.Style != null &&
                p.Style.Contains(styleTerm, StringComparison.OrdinalIgnoreCase));
        }

        query = sort?.Trim().ToLowerInvariant() switch
        {
            "price-asc" => query.OrderBy(p => p.Price),
            "price-desc" => query.OrderByDescending(p => p.Price),
            "name-asc" => query.OrderBy(p => p.Name),
            "name-desc" => query.OrderByDescending(p => p.Name),
            "newest" => query.OrderByDescending(p => p.Id),
            _ => query.OrderBy(p => p.Name)
        };

        return query.ToList();
    }

    public async Task<List<Product>> GetAllForAdminAsync()
    {
        return await _productRepository.GetAllForAdminAsync();
    }
    public async Task<(List<Product> Items, int TotalCount)> GetAdminPagedAsync(
    string? search, bool showDeleted, int page, int pageSize)
    {
        var products = await _productRepository.GetAllForAdminAsync();

        var query = products.Where(p => p.IsDeleted == showDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                $"sku-{p.Id}".Contains(term));
        }

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }


    public async Task<Product> CreateAsync(CreateProductDto dto)
    {
        if (dto.Image is null || dto.Image.Length == 0)
            throw new ArgumentException("A product image file is required.", nameof(dto.Image));

        var imageUrl = await _cloudinaryService.UploadImageAsync(dto.Image);

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = imageUrl,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            Style = dto.Style
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdateAsync(int id,ProductUpdateDto dto)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
            return null;

        existingProduct.Name = dto.Name;
        existingProduct.Description = dto.Description;
        existingProduct.Price = dto.Price;
        existingProduct.StockQuantity = dto.StockQuantity;
        existingProduct.CategoryId = dto.CategoryId;

        if (dto.Image is { Length: > 0 })
            existingProduct.ImageUrl = await _cloudinaryService.UploadImageAsync(dto.Image);

        await _productRepository.UpdateAsync(existingProduct);
        await _productRepository.SaveChangesAsync();

        return existingProduct;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            return false;

        await _productRepository.DeleteAsync(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }
    public async Task<bool> HardDeleteAsync(int id)
    {
        var products = await _productRepository.GetAllForAdminAsync();

        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
            return false;

        await _productRepository.HardDeleteAsync(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RestoreAsync(int id)
    {
        var products = await _productRepository.GetAllForAdminAsync();

        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
            return false;

        await _productRepository.RestoreAsync(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }
}