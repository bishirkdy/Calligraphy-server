using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task AddPaymentAsync(Payment payment);

    Task<Payment?> GetPaymentByOrderIdAsync(int orderId);

    Task<List<Payment>> GetAllPaymentsAsync();

    Task<decimal> GetTotalSuccessfulAmountAsync();

    Task<int> GetSuccessfulPaymentCountAsync();

    Task SaveChangesAsync();
}