using Calligraphy.Application.Configuration;
using Calligraphy.Application.DTOs.Payment;
using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Domain.Entities;
using Microsoft.Extensions.Options;
using Razorpay.Api;

namespace Calligraphy.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly RazorpaySettings _settings;

    public PaymentService(
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository,
        IOptions<RazorpaySettings> options)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _settings = options.Value;
    }

    public async Task<CreatePaymentResponseDto> CreatePaymentAsync(
        int orderId,
        int userId,
        string paymentMethod)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);

        if (order == null || order.UserId != userId)
            throw new InvalidOperationException("Order not found.");

        var amount = order.TotalAmount;
        paymentMethod = (paymentMethod ?? string.Empty).Trim().ToUpperInvariant();
        if (paymentMethod is not ("COD" or "UPI"))
            throw new InvalidOperationException("Payment method must be COD or UPI.");

        string razorpayOrderId = string.Empty;
        if (paymentMethod == "UPI")
        {
            if (string.IsNullOrWhiteSpace(_settings.KeyId) ||
                string.IsNullOrWhiteSpace(_settings.KeySecret))
                throw new InvalidOperationException("Razorpay credentials are not configured.");

            var client = new RazorpayClient(_settings.KeyId, _settings.KeySecret);
            var options = new Dictionary<string, object>
            {
                { "amount", (int)(amount * 100) },
                { "currency", "INR" },
                { "receipt", $"order_{orderId}" }
            };
            var razorpayOrder = client.Order.Create(options);
            razorpayOrderId = razorpayOrder["id"].ToString() ?? string.Empty;
        }

        var payment = new Calligraphy.Domain.Entities. Payment
        {
            OrderId = orderId,
            Amount = amount,
            RazorpayOrderId = razorpayOrderId,
            PaymentMethod = paymentMethod,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddPaymentAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        return new CreatePaymentResponseDto
        {
            OrderId = orderId,
            Amount = amount,
            RazorpayOrderId = razorpayOrderId,
            KeyId = paymentMethod == "UPI" ? _settings.KeyId : string.Empty
        };
    }

    public async Task<bool> VerifyPaymentAsync(
        VerifyPaymentDto dto,
        int userId)
    {
        var order = await _orderRepository
            .GetOrderByIdAsync(dto.OrderId);

        if (order == null || order.UserId != userId)
            return false;

        var payment = await _paymentRepository
            .GetPaymentByOrderIdAsync(dto.OrderId);

        if (payment == null ||
            payment.RazorpayOrderId != dto.RazorpayOrderId)
            return false;

        try
        {
            var attributes = new Dictionary<string, string>
            {
                {
                    "razorpay_order_id",
                    dto.RazorpayOrderId
                },
                {
                    "razorpay_payment_id",
                    dto.RazorpayPaymentId
                },
                {
                    "razorpay_signature",
                    dto.RazorpaySignature
                }
            };

            Utils.verifyPaymentSignature(attributes);

            payment.RazorpayPaymentId =
                dto.RazorpayPaymentId;

            payment.Status = "Success";

            await _paymentRepository.SaveChangesAsync();

            return true;
        }
        catch
        {
            payment.Status = "Failed";

            await _paymentRepository.SaveChangesAsync();

            return false;
        }
    }
}