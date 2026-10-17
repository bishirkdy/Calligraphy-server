using Calligraphy.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/admin/inquiries")]
[Authorize(Roles = "Admin")]
public class AdminInquiryController : ControllerBase
{
    private readonly IInquiryService _inquiryService;

    public AdminInquiryController(IInquiryService inquiryService)
    {
        _inquiryService = inquiryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var inquiries =
            await _inquiryService.GetAllInquiriesAsync();

        return Ok(inquiries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var inquiry =
            await _inquiryService.GetInquiryByIdAsync(id);

        if (inquiry == null)
            return NotFound("Inquiry not found.");

        return Ok(inquiry);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] string status)
    {
        var updated =
            await _inquiryService.UpdateInquiryStatusAsync(
                id,
                status);

        if (!updated)
            return NotFound("Inquiry not found.");

        return Ok(new
        {
            message = "Inquiry status updated successfully."
        });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _inquiryService.DeleteInquiryAsync(id);

        if (!deleted)
            return NotFound("Inquiry not found.");

        return Ok(new
        {
            message = "Inquiry deleted successfully."
        });
    }
}