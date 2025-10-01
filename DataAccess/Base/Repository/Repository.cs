using DataAccess.Abstract.Repository;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Base.Repository
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private readonly HaberContext context;
		public Repository(HaberContext context)
		{
			this.context = context;
		}
		public IEnumerable<TEntity> GetAll()
		{
			return	context.Set<TEntity>();
		}

        public TEntity GetById(int id)
        {
            var entity = context.Set<TEntity>().Find(id);
            if (entity == null)
            {
                // Burada null dönebilirsin ya da özel hata fırlatabilirsin
                return null;
                // veya throw new Exception($"ID {id} ile kayıt bulunamadı");
            }
            return entity;
        }

        public TEntity Insert(TEntity entity)
		{
			context.Entry(entity).State = EntityState.Added;
			context.SaveChanges();
			return entity;
		}

		public TEntity Update(TEntity entity)
		{
			context.Entry(entity).State = EntityState.Modified;
			context.SaveChanges();
			return entity;
		}
		public bool Delete(TEntity entity)
		{
			try
			{
				context.Entry(entity).State = EntityState.Deleted;
				context.SaveChanges();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
