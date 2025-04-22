using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SightSeeing.DAL.DbContext
{
    public class SightSeeingDbContextFactory : IDesignTimeDbContextFactory<SightSeeingDbContext>
    {
        public SightSeeingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SightSeeingDbContext>();
            optionsBuilder.UseSqlite("Data Source=sightseeing.db");

            return new SightSeeingDbContext(optionsBuilder.Options);
        }
    }
}