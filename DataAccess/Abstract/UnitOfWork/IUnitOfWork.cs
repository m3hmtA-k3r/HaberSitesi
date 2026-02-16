using DataAccess.Abstract.Repository;
using Domain.Entities;

namespace DataAccess.Abstract.UnitOfWork
{
	/// <summary>
	/// Unit of Work pattern for managing database transactions
	/// </summary>
	public interface IUnitOfWork : IDisposable
	{
		// Repositories
		IRepository<Haberler> HaberlerRepository { get; }
		IRepository<Kategoriler> KategorilerRepository { get; }
		IRepository<Yazarlar> YazarlarRepository { get; }
		IRepository<Yorumlar> YorumlarRepository { get; }
		IRepository<Slaytlar> SlaytlarRepository { get; }
		IRepository<Kullanicilar> KullanicilarRepository { get; }
		IRepository<Roller> RollerRepository { get; }
		IRepository<KullaniciRol> KullaniciRollerRepository { get; }

		// Blog Repositories
		IRepository<Bloglar> BloglarRepository { get; }
		IRepository<BlogKategoriler> BlogKategorilerRepository { get; }
		IRepository<BlogYorumlar> BlogYorumlarRepository { get; }

		// Iletisim Repository
		IRepository<IletisimMesajlari> IletisimMesajlariRepository { get; }

		// Sistem Log Repository
		IRepository<SistemLog> SistemLoglarRepository { get; }

		// Menu Repositories
		IRepository<Menuler> MenulerRepository { get; }
		IRepository<MenuOgeleri> MenuOgeleriRepository { get; }
		IRepository<MenuRoller> MenuRollerRepository { get; }
		IRepository<MenuOgeRoller> MenuOgeRollerRepository { get; }

		// Transaction methods
		int SaveChanges();
		Task<int> SaveChangesAsync();

		// Transaction support
		void BeginTransaction();
		void Commit();
		void Rollback();
	}
}
