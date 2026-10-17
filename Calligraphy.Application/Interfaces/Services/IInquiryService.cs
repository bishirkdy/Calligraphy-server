using Calligraphy.Application.DTOs.Inquiry;

namespace Calligraphy.Application.Interfaces.Services;

public interface IInquiryService
{
    Task CreateInquiryAsync(CreateInquiryDto dto);

    Task<List<InquiryDto>> GetAllInquiriesAsync();

    Task<InquiryDto?> GetInquiryByIdAsync(int id);

    Task<bool> UpdateInquiryStatusAsync(int id, string status);

    Task<bool> DeleteInquiryAsync(int id);  
}