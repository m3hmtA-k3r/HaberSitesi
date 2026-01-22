using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
	/// <summary>
	/// EF Core DbContext for Haber Sitesi
	/// Now uses Domain.Entities instead of Shared.Entities
	/// </summary>
	public class HaberContext : DbContext
	{
		public HaberContext(DbContextOptions<HaberContext> opt) : base(opt)
		{

		}

		public DbSet<Haberler> Haberler { get; set; }
		public DbSet<Kategoriler> Kategoriler { get; set; }
		public DbSet<Slaytlar> Slaytlar { get; set; }
		public DbSet<Yazarlar> Yazarlar { get; set; }
		public DbSet<Yorumlar> Yorumlar { get; set; }
	}
}
