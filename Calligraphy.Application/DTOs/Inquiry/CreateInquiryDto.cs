using System.ComponentModel.DataAnnotations;

namespace Calligraphy.Application.DTOs.Inquiry;

public class CreateInquiryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Requirements { get; set; } = string.Empty;
}