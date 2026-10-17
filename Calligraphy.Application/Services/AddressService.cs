using Calligraphy.Application.DTOs.Address;
using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<List<Address>> GetMyAddressesAsync(int userId)
    {
        return await _addressRepository.GetByUserIdAsync(userId);
    }

    public async Task<Address?> GetByIdAsync(
        int id,
        int userId)
    {
        return await _addressRepository.GetByIdAsync(id, userId);
    }

    public async Task<Address> AddAsync(
        int userId,
        AddressDto dto)
    {
        var address = new Address
        {
            UserId = userId,
            FullName = dto.FullName,
            AddressLine = dto.AddressLine,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email
        };

        await _addressRepository.AddAsync(address);
        await _addressRepository.SaveChangesAsync();

        return address;
    }

    public async Task<bool> UpdateAsync(
        int id,
        int userId,
        AddressDto dto)
    {
        var address = await _addressRepository
            .GetByIdAsync(id, userId);

        if (address == null)
            return false;

        address.FullName = dto.FullName;
        address.AddressLine = dto.AddressLine;
        address.PhoneNumber = dto.PhoneNumber;
        address.Email = dto.Email;

        await _addressRepository.UpdateAsync(address);
        await _addressRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        int userId)
    {
        var address = await _addressRepository
            .GetByIdAsync(id, userId);

        if (address == null)
            return false;

        await _addressRepository.DeleteAsync(address);
        await _addressRepository.SaveChangesAsync();

        return true;
    }
}