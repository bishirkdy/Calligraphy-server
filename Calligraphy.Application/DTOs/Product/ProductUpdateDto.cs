using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Calligraphy.Application.DTOs.Product;

public class ProductUpdateDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public IFormFile? Image { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public int? CategoryId { get; set; }
}