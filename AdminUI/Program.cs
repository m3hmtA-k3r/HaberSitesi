using ApiAccess.Abstract;
using ApiAccess.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.Helpers.Abstract;
using Shared.Helpers.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Session Configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

#region DependencyInjection
builder.Services.AddScoped<IRequestService, RequestManager>();
builder.Services.AddScoped<IHaberApiRequest, HaberApiRequest>();
builder.Services.AddScoped<IYazarApiRequest, YazarApiRequest>();
builder.Services.AddScoped<IKategoriApiRequest, KategoriApiRequest>();
builder.Services.AddScoped<IYorumApiRequest, YorumApiRequest>();
builder.Services.AddScoped<ICommonApiRequest, CommonApiRequest>();
builder.Services.AddScoped<ISlaytApiRequest, SlaytApiRequest>();
#endregion

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.Name = "HaberSitesi.AdminAuth";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
