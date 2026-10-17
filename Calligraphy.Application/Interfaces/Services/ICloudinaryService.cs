using Microsoft.AspNetCore.Http;

namespace Calligraphy.Application.Interfaces.Services;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(IFormFile file);
}