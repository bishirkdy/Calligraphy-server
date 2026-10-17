using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
