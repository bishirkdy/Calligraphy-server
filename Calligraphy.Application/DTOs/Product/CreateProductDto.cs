using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Calligraphy.Application.DTOs.Product;

public class CreateProductDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public int? CategoryId { get; set; }

    public string? Style { get; set; }

    [Required]
    public IFormFile? Image { get; set; }
}