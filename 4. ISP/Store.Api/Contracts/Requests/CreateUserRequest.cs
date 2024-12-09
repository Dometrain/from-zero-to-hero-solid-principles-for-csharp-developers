namespace Store.Api.Contracts.Requests;

public class CreateUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DeliveryAddress { get; set; }
    public string CountryCode { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}