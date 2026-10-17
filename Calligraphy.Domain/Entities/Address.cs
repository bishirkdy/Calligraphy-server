namespace Calligraphy.Domain.Entities;

public class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string AddressLine { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

   
}