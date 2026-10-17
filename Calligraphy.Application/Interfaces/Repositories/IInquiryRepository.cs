using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Repositories;

public interface IInquiryRepository
{
    Task AddAsync(Inquiry inquiry);

    Task<List<Inquiry>> GetAllAsync();

    Task<Inquiry?> GetByIdAsync(int id);

    Task UpdateAsync(Inquiry inquiry);

    Task DeleteAsync(Inquiry inquiry);

    Task SaveChangesAsync();
}