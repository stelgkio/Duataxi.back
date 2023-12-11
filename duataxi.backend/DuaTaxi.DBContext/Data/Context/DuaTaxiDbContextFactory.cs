using Microsoft.EntityFrameworkCore;

namespace DuaTaxi.DBContext.Data.Context
{
    public class DuaTaxiDbContextFactory : DesignTimeDbContextFactoryBase<DuaTaxiDbContext>
    {
        protected override DuaTaxiDbContext CreateNewInstance(DbContextOptions<DuaTaxiDbContext> options)
        {
            return new DuaTaxiDbContext(options);
        }
    }
}
