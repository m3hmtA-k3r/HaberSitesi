namespace DataAccess.Abstract.Repository
{
	public interface IRepository<TEntity> where TEntity : class
	{
		TEntity GetById(int id);
		IEnumerable<TEntity> GetAll();
		IQueryable<TEntity> Query();
		TEntity Insert(TEntity entity);
		TEntity Update(TEntity entity);
		bool Delete(TEntity entity);
	}
}
