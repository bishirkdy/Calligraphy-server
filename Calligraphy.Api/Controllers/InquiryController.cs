using System.Security.Claims;
using Calligraphy.Application.DTOs.Inquiry;
using Calligraphy.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InquiryController : ControllerBase
{
    private readonly IInquiryService _inquiryService;

    public InquiryController(IInquiryService inquiryService)
    {
        _inquiryService = inquiryService;
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Create(CreateInquiryDto dto)
    {
        // Use the account's own details, not what was typed in the form
        dto.Name = User.FindFirstValue(ClaimTypes.Name) ?? dto.Name;
        dto.Email = User.FindFirstValue(ClaimTypes.Email) ?? dto.Email;

        await _inquiryService.CreateInquiryAsync(dto);

        return Ok(new
        {
            message = "Inquiry created successfully."
        });
    }
}