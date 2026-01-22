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

		// Transaction methods
		int SaveChanges();
		Task<int> SaveChangesAsync();

		// Transaction support
		void BeginTransaction();
		void Commit();
		void Rollback();
	}
}
