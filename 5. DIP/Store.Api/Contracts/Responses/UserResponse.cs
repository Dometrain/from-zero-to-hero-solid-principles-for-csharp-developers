namespace Store.Api.Contracts.Responses;

public class UserResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string DeliveryAddress { get; set; }
    public string CountryCode { get; set; }
}