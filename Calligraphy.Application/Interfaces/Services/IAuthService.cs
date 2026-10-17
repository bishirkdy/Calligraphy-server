using System;
using System.Collections.Generic;
using System.Text;
using Calligraphy.Application.DTOs.Auth;

namespace Calligraphy.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);

        Task<AuthResponse?> LoginAsync(LoginRequest request);
    }
}
