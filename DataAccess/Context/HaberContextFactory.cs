using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Context
{
    public class HaberContextFactory : IDesignTimeDbContextFactory<HaberContext>
    {
        public HaberContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HaberContext>();

            // SQLite kullan
            optionsBuilder.UseSqlite("Data Source=../ApiUI/HaberSitesi.db");

            return new HaberContext(optionsBuilder.Options);
        }
    }
}
