using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Context
{
    public class HaberContextFactory : IDesignTimeDbContextFactory<HaberContext>
    {
        public HaberContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HaberContext>();

            // PostgreSQL kullan
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MaskerDB;Username=masker_admin;Password=Masker2026!SecurePass");

            return new HaberContext(optionsBuilder.Options);
        }
    }
}
