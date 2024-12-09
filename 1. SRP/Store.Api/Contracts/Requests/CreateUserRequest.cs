using System.ComponentModel.DataAnnotations;

namespace Store.Api.Contracts.Requests;

public class CreateUserRequest
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string DeliveryAddress { get; set; }
    [Required, MinLength(3), MaxLength(3)]
    public string CountryCode { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}