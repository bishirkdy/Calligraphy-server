using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(int id);

    Task<List<User>> GetAllAsync();


    Task AddAsync(User user);

    Task<bool> BlockAsync(int id);
    Task<bool> UnblockAsync(int id);

    Task SaveChangesAsync();
}
