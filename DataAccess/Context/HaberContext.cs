using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace DataAccess.Context
{
	public class HaberContext : DbContext
	{
        public HaberContext(DbContextOptions<HaberContext> opt) : base(opt)
        {
            
        }
        DbSet<Haberler> Haberler { get; set; }
		DbSet<Kategoriler> Kategoriler { get; set; }
		DbSet<Slaytlar> Slaytlar { get; set; }
		DbSet<Yazarlar> Yazarlar { get; set; }
		DbSet<Yorumlar> Yorumlar { get; set; }
	}
}
