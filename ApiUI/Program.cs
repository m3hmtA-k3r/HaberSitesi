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

// JWT Authentication
var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"]
    ?? "HaberSitesi_Default_Secret_Key_Min_32_Characters_Long_12345";
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "HaberSitesiAPI";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "HaberSitesiClients";

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

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MaskerPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5200", "http://localhost:5300")
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
// File Storage Service (Local implementation)
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

// Caching Service (InMemory implementation)
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, InMemoryCacheService>();

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

// User and Role Management Services
builder.Services.AddScoped<IKullaniciService, KullaniciManager>();
builder.Services.AddScoped<IRolService, RolManager>();
builder.Services.AddScoped<IAuthService, AuthManager>();

// Blog Module Services
builder.Services.AddScoped<IBlogService, BlogManager>();
builder.Services.AddScoped<IBlogKategoriService, BlogKategoriManager>();
builder.Services.AddScoped<IBlogYorumService, BlogYorumManager>();
#endregion

var app = builder.Build();

// Veritabanını oluştur (yoksa)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HaberContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
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

app.MapControllers();

app.Run();
