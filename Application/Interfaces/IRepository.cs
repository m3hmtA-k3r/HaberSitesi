namespace Application.Interfaces
{
	/// <summary>
	/// Generic repository interface for Application layer
	/// Implementation will be in DataAccess layer
	/// </summary>
	public interface IRepository<TEntity> where TEntity : class
	{
		TEntity GetById(int id);
		IEnumerable<TEntity> GetAll();
		TEntity Insert(TEntity entity);
		TEntity Update(TEntity entity);
		bool Delete(TEntity entity);
	}
}
