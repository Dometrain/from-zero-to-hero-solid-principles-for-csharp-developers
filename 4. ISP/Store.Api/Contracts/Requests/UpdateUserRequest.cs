namespace Store.Api.Contracts.Requests;

public class UpdateUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DeliveryAddress { get; set; }
    public string CountryCode { get; set; }
    public string Email { get; set; }
}