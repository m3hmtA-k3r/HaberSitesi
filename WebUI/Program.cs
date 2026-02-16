using ApiAccess.Abstract;
using ApiAccess.Base;
using Ganss.Xss;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using Shared.Helpers.Abstract;
using Shared.Helpers.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// HtmlSanitizer - XSS korumasi
builder.Services.AddSingleton<HtmlSanitizer>(sp =>
{
	var sanitizer = new HtmlSanitizer();
	// Guvenli HTML etiketlerini koru (icerik editorunden gelen formatlamalar)
	sanitizer.AllowedTags.Add("iframe");
	sanitizer.AllowedAttributes.Add("class");
	sanitizer.AllowedAttributes.Add("style");
	sanitizer.AllowedCssProperties.Add("color");
	sanitizer.AllowedCssProperties.Add("font-size");
	sanitizer.AllowedCssProperties.Add("text-align");
	sanitizer.AllowedCssProperties.Add("margin");
	sanitizer.AllowedCssProperties.Add("padding");
	sanitizer.AllowedCssProperties.Add("border");
	sanitizer.AllowedCssProperties.Add("border-collapse");
	sanitizer.AllowedCssProperties.Add("width");
	sanitizer.AllowedCssProperties.Add("height");
	sanitizer.AllowedCssProperties.Add("background-color");
	return sanitizer;
});

// In-Memory Cache - API yanit onbellekleme
builder.Services.AddMemoryCache();

// Add Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

#region DependencyInjection
builder.Services.AddScoped<IRequestService, RequestManager>();
builder.Services.AddScoped<IHaberApiRequest, HaberApiRequest>();
builder.Services.AddScoped<IYazarApiRequest, YazarApiRequest>();
builder.Services.AddScoped<IKategoriApiRequest, KategoriApiRequest>();
builder.Services.AddScoped<IYorumApiRequest, YorumApiRequest>();
builder.Services.AddScoped<ICommonApiRequest, CommonApiRequest>();
builder.Services.AddScoped<ISlaytApiRequest, SlaytApiRequest>();
builder.Services.AddScoped<IBlogApiRequest, BlogApiRequest>();
builder.Services.AddScoped<IBlogKategoriApiRequest, BlogKategoriApiRequest>();
builder.Services.AddScoped<IBlogYorumApiRequest, BlogYorumApiRequest>();
builder.Services.AddScoped<IIletisimApiRequest, IletisimApiRequest>();
builder.Services.AddScoped<IAuthApiRequest, AuthApiRequest>();
#endregion

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Hesap/Giris";
        options.AccessDeniedPath = "/Hesap/Giris";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.Name = "HaberSitesi.WebAuth";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

// Tarayici Cache Onleme - Dinamik sayfalarin tarayici tarafindan cachelenmesini engelle
// (Static dosyalar UseStaticFiles tarafindan ayri yonetildigi icin bunlar etkilenmez)
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        if (context.Response.ContentType?.Contains("text/html") == true)
        {
            context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";
        }
        return Task.CompletedTask;
    });
    await next();
});

// Cache Invalidation Middleware - ApiUI'den gelen sinyal dosyasini kontrol et
// Veri degismisse tum WebUI cache'lerini temizle
app.Use(async (context, next) =>
{
    const string signalPath = "/tmp/masker_cache_signal";
    try
    {
        if (System.IO.File.Exists(signalPath))
        {
            var cache = context.RequestServices.GetRequiredService<IMemoryCache>();
            var signalTime = System.IO.File.GetLastWriteTimeUtc(signalPath);
            var lastCheck = cache.Get<DateTime?>("_cache_signal_time");

            if (lastCheck == null || signalTime > lastCheck.Value)
            {
                cache.Remove("home_slaytlar");
                cache.Remove("home_haberler");
                cache.Remove("haber_kategoriler");
                cache.Remove("blog_kategoriler");
                cache.Set("_cache_signal_time", DateTime.UtcNow);
            }
        }
    }
    catch { /* Sinyal okunamazsa cache dogal TTL ile expire olur */ }

    await next();
});

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
