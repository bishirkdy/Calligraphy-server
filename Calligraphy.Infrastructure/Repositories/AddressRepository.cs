using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Domain.Entities;
using Calligraphy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calligraphy.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _context;

    public AddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Address>> GetByUserIdAsync(int userId)
    {
        return await _context.Addresses
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<Address?> GetByIdAsync(
        int id,
        int userId)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.UserId == userId);
    }

    public async Task AddAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
    }

    public async Task UpdateAsync(Address address)
    {
        _context.Addresses.Update(address);
    }

    public async Task DeleteAsync(Address address)
    {
        _context.Addresses.Remove(address);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}