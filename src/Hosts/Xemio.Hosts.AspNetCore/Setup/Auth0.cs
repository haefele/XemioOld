using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Xemio.Hosts.AspNetCore.Setup
{

    public static class Auth0
    {
        public static void UseAuth0Authentication(this IApplicationBuilder self, IConfiguration configuration)
        {
            self.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = $"https://{configuration.GetValue<string>("Domain")}/",
                    ValidAudience = configuration.GetValue<string>("ClientId"),
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(configuration.GetValue<string>("ClientSecret"))),
                    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                }
            });
        }
    }
}
