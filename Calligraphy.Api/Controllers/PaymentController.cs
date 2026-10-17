using System.Security.Claims;
using Calligraphy.Application.DTOs.Payment;
using Calligraphy.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create/{orderId}")]
    public async Task<ActionResult<CreatePaymentResponseDto>> CreatePayment(
        int orderId,
        [FromQuery] string paymentMethod)
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Unauthorized();

        if (string.IsNullOrWhiteSpace(paymentMethod))
            return BadRequest("Payment method is required.");

        paymentMethod = paymentMethod.ToUpperInvariant();

        if (paymentMethod != "COD" && paymentMethod != "UPI")
            return BadRequest("Payment method must be COD or UPI.");

        try
        {
            var result = await _paymentService.CreatePaymentAsync(
                orderId,
                userId,
                paymentMethod);

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyPayment(
        VerifyPaymentDto dto)
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Unauthorized();

        var verified =
            await _paymentService.VerifyPaymentAsync(
                dto,
                userId);

        if (!verified)
        {
            return BadRequest("Payment verification failed.");
        }

        return Ok(new
        {
            message = "Payment verified successfully."
        });
    }
}