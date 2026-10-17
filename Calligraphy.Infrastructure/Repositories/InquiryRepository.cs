using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Domain.Entities;
using Calligraphy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calligraphy.Infrastructure.Repositories;

public class InquiryRepository : IInquiryRepository
{
    private readonly ApplicationDbContext _context;

    public InquiryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Inquiry inquiry)
    {
        await _context.Inquiries.AddAsync(inquiry);
    }

    public async Task<List<Inquiry>> GetAllAsync()
    {
        return await _context.Inquiries
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<Inquiry?> GetByIdAsync(int id)
    {
        return await _context.Inquiries
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task UpdateAsync(Inquiry inquiry)
    {
        _context.Inquiries.Update(inquiry);
    }

    public Task DeleteAsync(Inquiry inquiry)
    {
        _context.Inquiries.Remove(inquiry);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
