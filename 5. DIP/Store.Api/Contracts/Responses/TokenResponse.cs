namespace Store.Api.Contracts.Responses;

public class TokenResponse
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}