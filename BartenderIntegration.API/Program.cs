using BartenderIntegration.API.Helper;
using BartenderIntegration.Infrastructure.Persistence;
using BartenderIntegration.Infrastructure.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IBartenderHelper, BartenderHelper>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Bar Tender API", Version = "v1" });

    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Description = "Please enter valid access token.",
        In = ParameterLocation.Header,
        Name = "Access token",
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=  new OpenApiReference
                {
                    Type= ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    using (var scope = app.Services.CreateScope())
    {
        var init = scope.ServiceProvider.GetRequiredService<IAppDBInitializer>();
        await init.SeedAsync();
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyMethod());

app.MapControllers();

app.Run();
