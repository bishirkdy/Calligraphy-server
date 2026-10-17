namespace Calligraphy.Application.DTOs.Payment;

public class CreatePaymentResponseDto
{
    public int OrderId { get; set; }

    public decimal Amount { get; set; }

    public string RazorpayOrderId { get; set; } = string.Empty;

    public string KeyId { get; set; } = string.Empty;
}