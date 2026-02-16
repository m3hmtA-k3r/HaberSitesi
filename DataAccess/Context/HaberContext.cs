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

		// Blog Modulu
		public DbSet<Bloglar> Bloglar { get; set; }
		public DbSet<BlogKategoriler> BlogKategoriler { get; set; }
		public DbSet<BlogYorumlar> BlogYorumlar { get; set; }

		// Iletisim Modulu
		public DbSet<IletisimMesajlari> IletisimMesajlari { get; set; }

		// Sistem Log Modulu
		public DbSet<SistemLog> SistemLoglari { get; set; }

		// Menu Modulu
		public DbSet<Menuler> Menuler { get; set; }
		public DbSet<MenuOgeleri> MenuOgeleri { get; set; }
		public DbSet<MenuRoller> MenuRoller { get; set; }
		public DbSet<MenuOgeRoller> MenuOgeRoller { get; set; }

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

			// Blog configuration
			modelBuilder.Entity<Bloglar>(entity =>
			{
				entity.HasIndex(e => e.KategoriId);
				entity.HasIndex(e => e.YazarId);
				entity.HasIndex(e => e.AktifMi);
				entity.HasOne(e => e.Kategori)
					.WithMany(c => c.Bloglar)
					.HasForeignKey(e => e.KategoriId)
					.OnDelete(DeleteBehavior.SetNull);
				entity.HasOne(e => e.Yazar)
					.WithMany()
					.HasForeignKey(e => e.YazarId)
					.OnDelete(DeleteBehavior.SetNull);
			});

			modelBuilder.Entity<BlogYorumlar>(entity =>
			{
				entity.HasIndex(e => e.BlogId);
				entity.HasOne(e => e.Blog)
					.WithMany(b => b.Yorumlar)
					.HasForeignKey(e => e.BlogId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// Menu configuration
			modelBuilder.Entity<Menuler>(entity =>
			{
				entity.HasIndex(e => e.Sira);
				entity.HasMany(e => e.MenuOgeleri)
					.WithOne(o => o.Menu)
					.HasForeignKey(o => o.MenuId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<MenuOgeleri>(entity =>
			{
				entity.HasIndex(e => e.MenuId);
				entity.HasIndex(e => e.Sira);
			});

			modelBuilder.Entity<MenuRoller>(entity =>
			{
				entity.HasIndex(e => new { e.MenuId, e.RolId }).IsUnique();
			});

			modelBuilder.Entity<MenuOgeRoller>(entity =>
			{
				entity.HasIndex(e => new { e.MenuOgeId, e.RolId }).IsUnique();
			});

			// Seed default roles
			modelBuilder.Entity<Roller>().HasData(
				new Roller { Id = 1, RolAdi = "Admin", Aciklama = "Sistem yöneticisi - tüm yetkiler", AktifMi = true },
				new Roller { Id = 2, RolAdi = "Editor", Aciklama = "İçerik editörü - haber ve kategori yönetimi", AktifMi = true },
				new Roller { Id = 3, RolAdi = "Yazar", Aciklama = "Yazar - kendi haberlerini yönetir", AktifMi = true },
				new Roller { Id = 4, RolAdi = "Moderator", Aciklama = "Moderatör - yorum moderasyonu", AktifMi = true }
			);
		}

		/// <summary>
		/// Seed default admin user at runtime (called from Program.cs)
		/// </summary>
		/// <param name="passwordHash">BCrypt hashed password string</param>
		public async Task SeedAdminUserAsync(string passwordHash)
		{
			// Check if admin already exists
			var existingAdmin = await Kullanicilar.FirstOrDefaultAsync(k => k.Eposta == "admin@masker.com");
			if (existingAdmin != null)
				return;

			// Check if Admin role exists
			var adminRole = await Roller.FirstOrDefaultAsync(r => r.RolAdi == "Admin");
			if (adminRole == null)
			{
				adminRole = new Roller { RolAdi = "Admin", Aciklama = "Sistem yöneticisi - tüm yetkiler", AktifMi = true };
				Roller.Add(adminRole);
				await SaveChangesAsync();
			}

			// Create admin user with hashed password
			var adminUser = new Kullanicilar
			{
				Ad = "Admin",
				Soyad = "User",
				Eposta = "admin@masker.com",
				SifreHash = passwordHash,
				AktifMi = true,
				OlusturmaTarihi = DateTime.UtcNow
			};

			Kullanicilar.Add(adminUser);
			await SaveChangesAsync();

			// Assign admin role
			var kullaniciRol = new KullaniciRol
			{
				KullaniciId = adminUser.Id,
				RolId = adminRole.Id,
				AtanmaTarihi = DateTime.UtcNow
			};

			KullaniciRoller.Add(kullaniciRol);
			await SaveChangesAsync();
		}
	}
}
