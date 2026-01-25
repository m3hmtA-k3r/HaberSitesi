using Domain.Entities;

namespace Application.Interfaces
{
	/// <summary>
	/// Unit of Work interface for Application layer
	/// Implementation will be in DataAccess layer
	/// This follows Dependency Inversion Principle
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

		// Transaction methods
		int SaveChanges();
		Task<int> SaveChangesAsync();
		void BeginTransaction();
		void Commit();
		void Rollback();
	}
}
