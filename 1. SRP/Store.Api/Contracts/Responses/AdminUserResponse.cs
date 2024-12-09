namespace Store.Api.Contracts.Responses;

public class AdminUserResponse
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string DeliveryAddress { get; set; }
    public string CountryCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<string> Roles { get; set; }
}