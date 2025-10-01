using Business.Abstract;
using Business.Base;
using DataAccess.Abstract.Repository;
using DataAccess.Base.Repository;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HaberContext>(option => option.UseSqlServer("Server=MEHMET\\SQLEXPRESS;Database=HABER_SITESI;Trusted_Connection=True;TrustServerCertificate=true;"));

#region DependencyInjection
//DBCONTEXT
builder.Services.AddScoped<DbContext, HaberContext>();

//DATA ACCESS START
builder.Services.AddScoped<IRepository<Haberler>, Repository<Haberler>>();
builder.Services.AddScoped<IRepository<Kategoriler>, Repository<Kategoriler>>();
builder.Services.AddScoped<IRepository<Slaytlar>, Repository<Slaytlar>>();
builder.Services.AddScoped<IRepository<Yazarlar>, Repository<Yazarlar>>();
builder.Services.AddScoped<IRepository<Yorumlar>, Repository<Yorumlar>>();
//DATA ACCESS FINISH

//BUSINESS START
builder.Services.AddScoped<IHaberService, HaberManager>();
builder.Services.AddScoped<IKategoriService, KategoriManager>();
builder.Services.AddScoped<ISlaytService, SlaytManager>();
builder.Services.AddScoped<IYorumService, YorumManager>();
builder.Services.AddScoped<IYazarService, YazarManager>();
//BUSINESS FINISH

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
