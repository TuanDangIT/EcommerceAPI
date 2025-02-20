using Ecommerce.Shared.Abstractions.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Auth
{
    internal static class Extensions
    {
        private const string _authenticationOptionsSectionName = "Authentication";
        private const string _adminOptionsSectionName = "Admin";
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            var authOptions = services.GetOptions<AuthOptions>(_authenticationOptionsSectionName);
            services.AddSingleton(authOptions);
            var adminOptions = services.GetOptions<AdminOptions>(_adminOptionsSectionName);
            services.AddSingleton(adminOptions);
            services.AddSingleton<IAuthManager, AuthManager>();
            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    //o.Audience = authOptions.Audience;

                    //o.SaveToken = authOptions.SaveToken;
                    //o.Challenge = JwtBearerDefaults.AuthenticationScheme;
                    o.RequireHttpsMetadata = false; //nie wymuszamy https
                    o.SaveToken = true; //zapisujemy dany token
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = authOptions.Issuer, //wydawca tokenu
                        ValidAudience = authOptions.Audience, //jakie podmioty moga uzywac tokenu
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.IssuerSigningKey)), //wygenerowany klucz prywatny
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.IssuerSigningKey)),
                        ValidateLifetime = true
                    }; //tutaj sprawdzamy Jwt token, sprawdzamy z tym z serwera.

                });
            services.AddAuthorization();
            return services;
        }
        public static WebApplication UseAuth(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
