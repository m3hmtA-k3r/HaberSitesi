using Application.Interfaces;
using Domain.Entities;

namespace DataAccess.Base.UnitOfWork
{
	/// <summary>
	/// Adapter to map DataAccess IUnitOfWork to Application IUnitOfWork
	/// This follows the Adapter pattern to bridge between layers
	/// </summary>
	public class ApplicationUnitOfWorkAdapter : Application.Interfaces.IUnitOfWork
	{
		private readonly DataAccess.Abstract.UnitOfWork.IUnitOfWork _dataAccessUnitOfWork;

		public ApplicationUnitOfWorkAdapter(DataAccess.Abstract.UnitOfWork.IUnitOfWork dataAccessUnitOfWork)
		{
			_dataAccessUnitOfWork = dataAccessUnitOfWork;
		}

		public Application.Interfaces.IRepository<Haberler> HaberlerRepository =>
			new RepositoryAdapter<Haberler>(_dataAccessUnitOfWork.HaberlerRepository);

		public Application.Interfaces.IRepository<Kategoriler> KategorilerRepository =>
			new RepositoryAdapter<Kategoriler>(_dataAccessUnitOfWork.KategorilerRepository);

		public Application.Interfaces.IRepository<Yazarlar> YazarlarRepository =>
			new RepositoryAdapter<Yazarlar>(_dataAccessUnitOfWork.YazarlarRepository);

		public Application.Interfaces.IRepository<Yorumlar> YorumlarRepository =>
			new RepositoryAdapter<Yorumlar>(_dataAccessUnitOfWork.YorumlarRepository);

		public Application.Interfaces.IRepository<Slaytlar> SlaytlarRepository =>
			new RepositoryAdapter<Slaytlar>(_dataAccessUnitOfWork.SlaytlarRepository);

		public Application.Interfaces.IRepository<Kullanicilar> KullanicilarRepository =>
			new RepositoryAdapter<Kullanicilar>(_dataAccessUnitOfWork.KullanicilarRepository);

		public Application.Interfaces.IRepository<Roller> RollerRepository =>
			new RepositoryAdapter<Roller>(_dataAccessUnitOfWork.RollerRepository);

		public Application.Interfaces.IRepository<KullaniciRol> KullaniciRollerRepository =>
			new RepositoryAdapter<KullaniciRol>(_dataAccessUnitOfWork.KullaniciRollerRepository);

		public int SaveChanges() => _dataAccessUnitOfWork.SaveChanges();

		public Task<int> SaveChangesAsync() => _dataAccessUnitOfWork.SaveChangesAsync();

		public void BeginTransaction() => _dataAccessUnitOfWork.BeginTransaction();

		public void Commit() => _dataAccessUnitOfWork.Commit();

		public void Rollback() => _dataAccessUnitOfWork.Rollback();

		public void Dispose() => _dataAccessUnitOfWork.Dispose();
	}

	/// <summary>
	/// Repository adapter to bridge DataAccess and Application interfaces
	/// </summary>
	internal class RepositoryAdapter<TEntity> : Application.Interfaces.IRepository<TEntity> where TEntity : class
	{
		private readonly DataAccess.Abstract.Repository.IRepository<TEntity> _dataAccessRepository;

		public RepositoryAdapter(DataAccess.Abstract.Repository.IRepository<TEntity> dataAccessRepository)
		{
			_dataAccessRepository = dataAccessRepository;
		}

		public TEntity GetById(int id) => _dataAccessRepository.GetById(id);

		public IEnumerable<TEntity> GetAll() => _dataAccessRepository.GetAll();

		public TEntity Insert(TEntity entity) => _dataAccessRepository.Insert(entity);

		public TEntity Update(TEntity entity) => _dataAccessRepository.Update(entity);

		public bool Delete(TEntity entity) => _dataAccessRepository.Delete(entity);
	}
}
