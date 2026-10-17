using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Repositories;

public interface IAddressRepository
{
    Task<List<Address>> GetByUserIdAsync(int userId);

    Task<Address?> GetByIdAsync(int id, int userId);

    Task AddAsync(Address address);

    Task UpdateAsync(Address address);

    Task DeleteAsync(Address address);

    Task SaveChangesAsync();
}