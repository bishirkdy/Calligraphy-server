namespace Calligraphy.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public string ShippingAddress { get; set; } = string.Empty;
    public Payment? Payment { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}