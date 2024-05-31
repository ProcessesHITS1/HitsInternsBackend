using Interns.Auth.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Interns.Auth.Extensions
{
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Overload for default ConfigureAuth that requires "Auth" section in appsettings.json or any configuration provider
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
        {
            var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>();

            ArgumentNullException.ThrowIfNull(authOptions);
            ArgumentException.ThrowIfNullOrEmpty(authOptions?.Key);
            ArgumentException.ThrowIfNullOrEmpty(authOptions?.Issuer);

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOptions.Key));

            return builder.ConfigureAuth(authOptions.Issuer, securityKey);
        }

        /// <summary>
        /// Default method for configuration auth, adds jwt handling and stuff
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="issuer"></param>
        /// <param name="issuerSigningKey"></param>
        public static WebApplicationBuilder ConfigureAuth(
            this WebApplicationBuilder builder,
            string issuer,
            SecurityKey issuerSigningKey
        )
        {
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = issuer;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuers = [issuer],
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = issuerSigningKey,
                        ValidateIssuerSigningKey = true
                    };

                    // leave for [Authorize] debugging: set breakpoint on lambda
#pragma warning disable
                    options.Events = new()
                    {
                        OnAuthenticationFailed = async ctx =>
                        {
                            var breakpoint = true;
                        }
                    };
#pragma warning restore
                });

            builder.Services.AddAuthorization();

            return builder;
        }
    }
}
