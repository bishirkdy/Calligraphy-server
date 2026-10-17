using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);
}