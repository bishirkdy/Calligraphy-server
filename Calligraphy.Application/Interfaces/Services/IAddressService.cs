using Calligraphy.Application.DTOs.Address;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Services;

public interface IAddressService
{
    Task<List<Address>> GetMyAddressesAsync(int userId);

    Task<Address?> GetByIdAsync(int id, int userId);

    Task<Address> AddAsync(int userId, AddressDto dto);

    Task<bool> UpdateAsync(
        int id,
        int userId,
        AddressDto dto);

    Task<bool> DeleteAsync(
        int id,
        int userId);
}