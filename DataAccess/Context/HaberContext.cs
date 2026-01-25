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
		public DbSet<Kullanicilar> Kullanicilar { get; set; }
		public DbSet<Roller> Roller { get; set; }
		public DbSet<KullaniciRol> KullaniciRoller { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Kullanicilar configuration
			modelBuilder.Entity<Kullanicilar>(entity =>
			{
				entity.HasIndex(e => e.Eposta).IsUnique();
			});

			// KullaniciRol configuration - Many-to-Many relationship
			modelBuilder.Entity<KullaniciRol>(entity =>
			{
				entity.HasIndex(e => new { e.KullaniciId, e.RolId }).IsUnique();
			});

			// Yazarlar - Kullanicilar relationship
			modelBuilder.Entity<Yazarlar>(entity =>
			{
				entity.HasIndex(e => e.KullaniciId);
			});

			// Seed default roles
			modelBuilder.Entity<Roller>().HasData(
				new Roller { Id = 1, RolAdi = "Admin", Aciklama = "Sistem yöneticisi - tüm yetkiler", AktifMi = true },
				new Roller { Id = 2, RolAdi = "Editor", Aciklama = "İçerik editörü - haber ve kategori yönetimi", AktifMi = true },
				new Roller { Id = 3, RolAdi = "Yazar", Aciklama = "Yazar - kendi haberlerini yönetir", AktifMi = true },
				new Roller { Id = 4, RolAdi = "Moderator", Aciklama = "Moderatör - yorum moderasyonu", AktifMi = true }
			);
		}
	}
}
