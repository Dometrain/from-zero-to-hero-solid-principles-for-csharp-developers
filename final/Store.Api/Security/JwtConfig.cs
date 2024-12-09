using System.Text;
using Microsoft.IdentityModel.Tokens;
using Store.Common.Helpers;

namespace Store.Api.Security;

public class JwtConfig
{
    public JwtConfig(string issuer, string audience, string key)
    {
        Issuer = issuer.NotNull();
        Audience = audience.NotNull();
        var keyString = key.NotNull();
        Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
    }

    public string Issuer { get; }
    public string Audience { get; }
    public SymmetricSecurityKey Key { get; }
}