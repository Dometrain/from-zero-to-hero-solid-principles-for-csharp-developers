namespace Store.Infrastructure.Models;

public class UserRecord
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string DeliveryAddress { get; set; }
    public string CountryCode { get; set; }
    public string HashedPassword { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedAt { get; set; }
}