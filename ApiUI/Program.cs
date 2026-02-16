using ApiUI.Middleware;
using Business.Abstract;
using Business.Base;
using DataAccess.Abstract.UnitOfWork;
using DataAccess.Base.UnitOfWork;
using DataAccess.Context;
using FluentValidation;
using Infrastructure.Caching;
using Infrastructure.Identity;
using Infrastructure.Security;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

// PostgreSQL DateTime compatibility
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT Bearer
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Masker API",
        Version = "v1",
        Description = "Masker - Multi-Client Content Platform API - JWT Authentication Required"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database Connection (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<HaberContext>(option =>
    option.UseNpgsql(connectionString));

// JWT Authentication - Environment variable takes priority over appsettings
var jwtSecretKey = Environment.GetEnvironmentVariable("MASKER_JWT_SECRET")
    ?? builder.Configuration["JwtSettings:SecretKey"]
    ?? throw new InvalidOperationException(
        "JWT SecretKey is not configured. Set MASKER_JWT_SECRET environment variable or configure JwtSettings:SecretKey in appsettings.json");
var jwtIssuer = Environment.GetEnvironmentVariable("MASKER_JWT_ISSUER")
    ?? builder.Configuration["JwtSettings:Issuer"]
    ?? "MaskerAPI";
var jwtAudience = Environment.GetEnvironmentVariable("MASKER_JWT_AUDIENCE")
    ?? builder.Configuration["JwtSettings:Audience"]
    ?? "MaskerClients";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// CORS - Environment variable or config takes priority
var corsOrigins = Environment.GetEnvironmentVariable("MASKER_CORS_ORIGINS")
    ?? builder.Configuration["CorsSettings:AllowedOrigins"]
    ?? "http://localhost:5200,http://localhost:5251,http://localhost:5167";
var allowedOrigins = corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MaskerPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

#region DependencyInjection
// UNIT OF WORK PATTERN
// UnitOfWork manages all repositories and transactions
builder.Services.AddScoped<DataAccess.Abstract.UnitOfWork.IUnitOfWork, UnitOfWork>();

// Adapter: Map DataAccess IUnitOfWork to Application IUnitOfWork
builder.Services.AddScoped<Application.Interfaces.IUnitOfWork, DataAccess.Base.UnitOfWork.ApplicationUnitOfWorkAdapter>();

// APPLICATION LAYER - CQRS with MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Features.Haberler.Queries.GetAllHaberler.GetAllHaberlerQuery).Assembly));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Application.Mappings.MappingProfile));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Application.Features.Haberler.Commands.CreateHaber.CreateHaberCommandValidator>();

// INFRASTRUCTURE LAYER
// File Storage Service - Environment variable or config determines provider
// MASKER_STORAGE_PROVIDER: "s3", "hybrid", or "local" (default)
var storageProvider = Environment.GetEnvironmentVariable("MASKER_STORAGE_PROVIDER")
	?? builder.Configuration["FileStorage:Provider"]
	?? "local";

switch (storageProvider.ToLower())
{
	case "s3":
		builder.Services.AddSingleton<IFileStorageService, S3FileStorageService>();
		break;
	case "hybrid":
		builder.Services.AddSingleton<IFileStorageService, HybridFileStorageService>();
		break;
	default:
		builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
		break;
}

// Caching Service - Environment variable or config determines provider
// MASKER_CACHE_PROVIDER: "redis", "hybrid", or "memory" (default)
builder.Services.AddMemoryCache();
var cacheProvider = Environment.GetEnvironmentVariable("MASKER_CACHE_PROVIDER")
	?? builder.Configuration["Cache:Provider"]
	?? "memory";

switch (cacheProvider.ToLower())
{
	case "redis":
		builder.Services.AddSingleton<ICacheService, RedisCacheService>();
		break;
	case "hybrid":
		builder.Services.AddSingleton<ICacheService, HybridCacheService>();
		break;
	default:
		builder.Services.AddScoped<ICacheService, InMemoryCacheService>();
		break;
}

// JWT Token Service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Password Hasher
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

// BUSINESS LAYER
// Business services now use UnitOfWork instead of individual repositories
builder.Services.AddScoped<IHaberService, HaberManager>();
builder.Services.AddScoped<IKategoriService, KategoriManager>();
builder.Services.AddScoped<ISlaytService, SlaytManager>();
builder.Services.AddScoped<IYorumService, YorumManager>();
builder.Services.AddScoped<IYazarService, YazarManager>();

// Yazar-Kullanici Migration Service (for legacy system consolidation)
builder.Services.AddScoped<IYazarMigrationService, YazarMigrationService>();

// User and Role Management Services
builder.Services.AddScoped<IKullaniciService, KullaniciManager>();
builder.Services.AddScoped<IRolService, RolManager>();
builder.Services.AddScoped<IAuthService, AuthManager>();

// Blog Module Services
builder.Services.AddScoped<IBlogService, BlogManager>();
builder.Services.AddScoped<IBlogKategoriService, BlogKategoriManager>();
builder.Services.AddScoped<IBlogYorumService, BlogYorumManager>();

// Iletisim Module Service
builder.Services.AddScoped<IIletisimService, IletisimManager>();

// Menu Module Service
builder.Services.AddScoped<IMenuService, MenuManager>();

// Sistem Log Module Service
builder.Services.AddScoped<ISistemLogService, SistemLogManager>();
#endregion

var app = builder.Build();

// Veritabanını oluştur ve seed data ekle
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HaberContext>();
    db.Database.EnsureCreated();

    // Eksik tablolari olustur (EnsureCreated mevcut DB'ye yeni tablo eklemez)
    await db.Database.ExecuteSqlRawAsync(@"
        CREATE TABLE IF NOT EXISTS ""ILETISIM_MESAJLARI"" (
            ""ID"" SERIAL PRIMARY KEY,
            ""AD"" VARCHAR(100) NOT NULL,
            ""EPOSTA"" VARCHAR(255) NOT NULL,
            ""KONU"" VARCHAR(200) NOT NULL,
            ""MESAJ"" TEXT NOT NULL,
            ""IP_ADRESI"" VARCHAR(45),
            ""EKLEME_TARIHI"" TIMESTAMP NOT NULL DEFAULT NOW(),
            ""OKUNDU_MU"" BOOLEAN NOT NULL DEFAULT FALSE,
            ""CEVAPLANDI_MI"" BOOLEAN NOT NULL DEFAULT FALSE,
            ""CEVAP_TARIHI"" TIMESTAMP
        );
        CREATE TABLE IF NOT EXISTS ""SISTEM_LOGLARI"" (
            ""ID"" SERIAL PRIMARY KEY,
            ""KULLANICI_ID"" INTEGER,
            ""KULLANICI_ADI"" VARCHAR(100),
            ""ISLEM_TIPI"" VARCHAR(50) NOT NULL,
            ""MODUL"" VARCHAR(100) NOT NULL,
            ""ACIKLAMA"" TEXT NOT NULL,
            ""IP_ADRESI"" VARCHAR(45),
            ""TARIH"" TIMESTAMP NOT NULL DEFAULT NOW(),
            ""SEVIYE"" VARCHAR(20) NOT NULL DEFAULT 'Info'
        );
    ");

    // Seed default admin user (admin@masker.com / Admin123)
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    var adminPasswordHash = passwordHasher.HashPassword("Admin123");
    await db.SeedAdminUserAsync(adminPasswordHash);
}

// Configure the HTTP request pipeline.
// Global Exception Handler - must be first in pipeline
app.UseGlobalExceptionHandler();

// Enable Swagger in all environments for testing
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Masker API v1");
    c.RoutePrefix = string.Empty; // Swagger UI at root
});

// Security Headers
app.Use(async (context, next) =>
{
    if (!context.Request.Path.StartsWithSegments("/swagger"))
    {
        context.Response.Headers.Append("Content-Security-Policy",
            "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline';");
    }
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.Remove("Server");
    await next();
});

app.UseCors("MaskerPolicy");
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Cache Invalidation Signal - AdminUI uzerinden veri degistiginde WebUI cache'ini bilgilendir
app.Use(async (context, next) =>
{
    await next();

    var method = context.Request.Method;
    if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300 &&
        (method == "POST" || method == "PUT" || method == "DELETE"))
    {
        try
        {
            await System.IO.File.WriteAllTextAsync(
                "/tmp/masker_cache_signal",
                DateTime.UtcNow.Ticks.ToString());
        }
        catch { /* Sinyal yazilamazsa cache dogal TTL ile expire olur */ }
    }
});

app.MapControllers();

app.Run();
