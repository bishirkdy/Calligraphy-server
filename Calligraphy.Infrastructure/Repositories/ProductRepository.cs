using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Domain.Entities;
using Calligraphy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calligraphy.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p =>
                p.Id == id &&
                !p.IsDeleted);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }
    public async Task<List<Product>> GetAllForAdminAsync()
    {
        return await _context.Products
            .OrderByDescending(p => p.Id)
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    
    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
    }

    public async Task DeleteAsync(Product product)
    {
        product.IsDeleted = true;
        _context.Products.Update(product);
    }

    public async Task RestoreAsync(Product product)
    {
        product.IsDeleted = false;

        _context.Products.Update(product);
    }

    public async Task HardDeleteAsync(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}