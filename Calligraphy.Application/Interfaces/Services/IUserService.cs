using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByEmailAsync(string email);

    Task<User> CreateAsync(User user);

    Task<List<User>> GetAllAsync();

    Task<(List<User> Users, int TotalCount)> GetUsersPagedAsync(
        string? search, int page, int pageSize);
    Task<bool> BlockAsync(int id);
    Task<bool> UnblockAsync(int id);
}
