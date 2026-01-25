using DataAccess.Abstract.Repository;
using DataAccess.Abstract.UnitOfWork;
using DataAccess.Base.Repository;
using DataAccess.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Base.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly HaberContext _context;
		private IDbContextTransaction _transaction;
		private bool _disposed = false;

		// Lazy repository initialization
		private IRepository<Haberler> _haberlerRepository;
		private IRepository<Kategoriler> _kategorilerRepository;
		private IRepository<Yazarlar> _yazarlarRepository;
		private IRepository<Yorumlar> _yorumlarRepository;
		private IRepository<Slaytlar> _slaytlarRepository;
		private IRepository<Kullanicilar> _kullanicilarRepository;
		private IRepository<Roller> _rollerRepository;
		private IRepository<KullaniciRol> _kullaniciRollerRepository;

		public UnitOfWork(HaberContext context)
		{
			_context = context;
		}

		public IRepository<Haberler> HaberlerRepository
		{
			get
			{
				return _haberlerRepository ??= new Repository<Haberler>(_context);
			}
		}

		public IRepository<Kategoriler> KategorilerRepository
		{
			get
			{
				return _kategorilerRepository ??= new Repository<Kategoriler>(_context);
			}
		}

		public IRepository<Yazarlar> YazarlarRepository
		{
			get
			{
				return _yazarlarRepository ??= new Repository<Yazarlar>(_context);
			}
		}

		public IRepository<Yorumlar> YorumlarRepository
		{
			get
			{
				return _yorumlarRepository ??= new Repository<Yorumlar>(_context);
			}
		}

		public IRepository<Slaytlar> SlaytlarRepository
		{
			get
			{
				return _slaytlarRepository ??= new Repository<Slaytlar>(_context);
			}
		}

		public IRepository<Kullanicilar> KullanicilarRepository
		{
			get
			{
				return _kullanicilarRepository ??= new Repository<Kullanicilar>(_context);
			}
		}

		public IRepository<Roller> RollerRepository
		{
			get
			{
				return _rollerRepository ??= new Repository<Roller>(_context);
			}
		}

		public IRepository<KullaniciRol> KullaniciRollerRepository
		{
			get
			{
				return _kullaniciRollerRepository ??= new Repository<KullaniciRol>(_context);
			}
		}

		public int SaveChanges()
		{
			return _context.SaveChanges();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void BeginTransaction()
		{
			_transaction = _context.Database.BeginTransaction();
		}

		public void Commit()
		{
			try
			{
				_context.SaveChanges();
				_transaction?.Commit();
			}
			catch
			{
				Rollback();
				throw;
			}
			finally
			{
				_transaction?.Dispose();
				_transaction = null;
			}
		}

		public void Rollback()
		{
			_transaction?.Rollback();
			_transaction?.Dispose();
			_transaction = null;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_transaction?.Dispose();
					_context.Dispose();
				}
			}
			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
