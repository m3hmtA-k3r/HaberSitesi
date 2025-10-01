using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class HaberContextFactory : IDesignTimeDbContextFactory<HaberContext>
    {
        public HaberContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HaberContext>();
            optionsBuilder.UseSqlServer("Server=MEHMET\\SQLEXPRESS;Database=HABER_SITESI;Trusted_Connection=True;TrustServerCertificate=True;");

            return new HaberContext(optionsBuilder.Options);
        }
    }
}
