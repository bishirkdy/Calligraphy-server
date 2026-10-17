using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Domain.Entities;
using Calligraphy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calligraphy.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<List<Payment>> GetAllPaymentsAsync()
    {
        return await _context.Payments
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalSuccessfulAmountAsync()
    {
        return await _context.Payments
            .Where(p => p.Status == "Success")
            .SumAsync(p => p.Amount);
    }

    public async Task<int> GetSuccessfulPaymentCountAsync()
    {
        return await _context.Payments
            .CountAsync(p => p.Status == "Success");
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}