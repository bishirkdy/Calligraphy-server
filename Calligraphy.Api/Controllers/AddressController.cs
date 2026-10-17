using System.Security.Claims;
using Calligraphy.Application.DTOs.Address;
using Calligraphy.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calligraphy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyAddresses()
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var addresses =
            await _addressService.GetMyAddressesAsync(userId.Value);

        return Ok(addresses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var address =
            await _addressService.GetByIdAsync(id, userId.Value);

        if (address == null)
            return NotFound("Address not found.");

        return Ok(address);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddressDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var address =
            await _addressService.AddAsync(
                userId.Value,
                dto);

        return Ok(address);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        AddressDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var updated =
            await _addressService.UpdateAsync(
                id,
                userId.Value,
                dto);

        if (!updated)
            return NotFound("Address not found.");

        return Ok("Address updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var deleted =
            await _addressService.DeleteAsync(
                id,
                userId.Value);

        if (!deleted)
            return NotFound("Address not found.");

        return NoContent();
    }

    private int? GetUserId()
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdValue == null)
            return null;

        if (!int.TryParse(userIdValue, out int userId))
            return null;

        return userId;
    }
}