using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Notifications.WebApi.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;

namespace Notifications.WebApi.IntegrationTests
{
    public class StartupStub : Startup
    {
        public StartupStub(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void AddAuthorization(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(5),
                        RequireAudience = false,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        RequireExpirationTime = false,
                        RequireSignedTokens = false,
                        SignatureValidator = (x, y) => new JwtSecurityToken(x)
                    };
                });

            var policies = typeof(NotificationsPolicies)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .Select(x => x.GetRawConstantValue().ToString())
                .ToList();

            services
                .AddAuthorization(options =>
                {
                    foreach (var policy in policies)
                    {
                        options.AddPolicy(policy, builder => builder.RequireRole("test"));
                    }
                });
        }
    }
}
