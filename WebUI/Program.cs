using ApiAccess.Abstract;
using ApiAccess.Base;
using Shared.Helpers.Abstract;
using Shared.Helpers.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
