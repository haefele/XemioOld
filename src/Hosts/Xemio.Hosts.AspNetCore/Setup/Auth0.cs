using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("ClientSecret"))),
                    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                }
            });
        }
    }
}
