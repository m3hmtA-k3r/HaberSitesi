using DataAccess.Abstract.Repository;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Base.Repository
{
	/// <summary>
	/// Generic repository implementation without SaveChanges
	/// SaveChanges is now managed by Unit of Work pattern
	/// </summary>
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private readonly HaberContext context;

		public Repository(HaberContext context)
		{
			this.context = context;
		}

		public IEnumerable<TEntity> GetAll()
		{
			return context.Set<TEntity>().ToList();
		}

		public IQueryable<TEntity> Query()
		{
			return context.Set<TEntity>().AsQueryable();
		}

		public TEntity GetById(int id)
		{
			var entity = context.Set<TEntity>().Find(id);
			if (entity == null)
			{
				return null;
			}
			return entity;
		}

		public TEntity Insert(TEntity entity)
		{
			context.Set<TEntity>().Add(entity);
			// SaveChanges removed - managed by UnitOfWork
			return entity;
		}

		public TEntity Update(TEntity entity)
		{
			context.Entry(entity).State = EntityState.Modified;
			// SaveChanges removed - managed by UnitOfWork
			return entity;
		}

		public bool Delete(TEntity entity)
		{
			try
			{
				context.Set<TEntity>().Remove(entity);
				// SaveChanges removed - managed by UnitOfWork
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
