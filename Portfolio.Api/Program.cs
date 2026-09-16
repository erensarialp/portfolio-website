using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Portfolio.Api.Services.Chat;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

/*
 * CHAT
 */

builder.Services.AddSingleton<
    IChatSessionStore,
    InMemoryChatSessionStore
>();

builder.Services.AddHttpClient<
    IAiChatService,
    NvidiaChatService
>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(25);
});

builder.Services.AddScoped<
    IChatService,
    ChatService
>();

/*
 * CORS
 */

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("Frontend", policy =>
//    {
//        policy
//            .WithOrigins(
//                "http://localhost:5173"
//            )
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://portfolio-web-125d.onrender.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

/*
 * SWAGGER
 */

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT token giriniz."
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference(
                    "Bearer",
                    document),
                new List<string>()
            }
        });
});

/*
 * INFRASTRUCTURE
 */

builder.Services.AddInfrastructure(
    builder.Configuration);

/*
 * JWT
 */

var jwtKey =
    builder.Configuration["Jwt:Key"];

var issuer =
    builder.Configuration["Jwt:Issuer"];

var audience =
    builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT Key yapılandırması bulunamadı.");
}

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = issuer,
                ValidAudience = audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtKey)),

                ClockSkew =
                    TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

/*
 * DEVELOPMENT
 */

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*
 * MIDDLEWARE
 */

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

/*
 * BASIC HEALTH
 */

app.MapGet(
    "/api/health",
    () =>
        Results.Ok(
            new
            {
                status = "healthy"
            }));

/*
 * DATABASE HEALTH
 */

app.MapGet(
    "/api/health/database",
    async (
        PortfolioDbContext dbContext) =>
    {
        try
        {
            var canConnect =
                await dbContext.Database
                    .CanConnectAsync();

            if (!canConnect)
            {
                return Results.Problem(
                    detail:
                        "PostgreSQL veritabanına bağlantı kurulamadı.",
                    statusCode:
                        StatusCodes
                            .Status503ServiceUnavailable);
            }

            return Results.Ok(
                new
                {
                    status = "healthy",
                    database = "PostgreSQL",
                    message =
                        "Neon PostgreSQL bağlantısı başarılı."
                });
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail:
                    $"Veritabanı bağlantısı sırasında hata oluştu: {ex.Message}",
                statusCode:
                    StatusCodes
                        .Status500InternalServerError);
        }
    });

app.Run();