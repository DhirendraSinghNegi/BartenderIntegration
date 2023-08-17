using BartenderIntegration.Infrastructure.Identity;
using BartenderIntegration.Infrastructure.Models;
using BartenderIntegration.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BartenderIntegration.Infrastructure.Services
{
    public static class ConfigurationServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultUser = configuration.GetSection("AppUser");
            var appSettings = configuration.GetSection("AppSettings");
            var jwtSettings = configuration.GetSection("JWT");
            services.Configure<DefaultUser>(defaultUser);
            services.Configure<AppSettings>(appSettings);
            services.Configure<JwtHandler>(jwtSettings);
            services.AddScoped<IAppDBInitializer, AppDBInitializer>();
            services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("bartender"));
            services.AddDefaultIdentity<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                        ValidAudience = jwtSettings.GetValue<string>("Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("Key")!))
                    };
                });

            return services;
        }
    }
}
