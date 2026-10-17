using Calligraphy.Application.DTOs.Inquiry;
using Calligraphy.Application.Interfaces.Repositories;
using Calligraphy.Application.Interfaces.Services;
using Calligraphy.Domain.Entities;

namespace Calligraphy.Application.Services;

public class InquiryService : IInquiryService
{
    private readonly IInquiryRepository _inquiryRepository;

    public InquiryService(IInquiryRepository inquiryRepository)
    {
        _inquiryRepository = inquiryRepository;
    }

    public async Task CreateInquiryAsync(CreateInquiryDto dto)
    {
        var inquiry = new Inquiry
        {
            Name = dto.Name,
            Email = dto.Email,
            Address = dto.Address,
            Requirements = dto.Requirements,
            CreatedAt = DateTime.UtcNow,
            Status = "Pending"
        };

        await _inquiryRepository.AddAsync(inquiry);
        await _inquiryRepository.SaveChangesAsync();
    }

    public async Task<List<InquiryDto>> GetAllInquiriesAsync()
    {
        var inquiries = await _inquiryRepository.GetAllAsync();

        return inquiries.Select(i => new InquiryDto
        {
            Id = i.Id,
            Name = i.Name,
            Email = i.Email,
            Address = i.Address,
            Requirements = i.Requirements,
            CreatedAt = i.CreatedAt,
            Status = i.Status
        }).ToList();
    }

    public async Task<InquiryDto?> GetInquiryByIdAsync(int id)
    {
        var inquiry = await _inquiryRepository.GetByIdAsync(id);

        if (inquiry == null)
            return null;

        return new InquiryDto
        {
            Id = inquiry.Id,
            Name = inquiry.Name,
            Email = inquiry.Email,
            Address = inquiry.Address,
            Requirements = inquiry.Requirements,
            CreatedAt = inquiry.CreatedAt,
            Status = inquiry.Status
        };
    }

    public async Task<bool> UpdateInquiryStatusAsync(int id, string status)
    {
        var inquiry = await _inquiryRepository.GetByIdAsync(id);

        if (inquiry == null)
            return false;

        inquiry.Status = status;

        await _inquiryRepository.UpdateAsync(inquiry);
        await _inquiryRepository.SaveChangesAsync();

        return true;
    }
    public async Task<bool> DeleteInquiryAsync(int id)
    {
        var inquiry = await _inquiryRepository.GetByIdAsync(id);
        if (inquiry == null)
            return false;

        await _inquiryRepository.DeleteAsync(inquiry);
        await _inquiryRepository.SaveChangesAsync();
        return true;
    }

}
