using Calligraphy.Application.DTOs.Payment;

namespace Calligraphy.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<CreatePaymentResponseDto> CreatePaymentAsync(
        int orderId,
        int userId,
        string paymentMethod);

    Task<bool> VerifyPaymentAsync(
        VerifyPaymentDto dto,
        int userId);
}