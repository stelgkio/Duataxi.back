
using DuaTaxi.Entities.Core.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity ,new()
    {

        private DbSet<TEntity> _dbContext;

        private DbContext _context;
        public DbContext Context
        {
            get
            {
                return _context;
            }
        }
        public Repository(DbContext context)
        {
            _context = context;
            _dbContext = _context.Set<TEntity>();

        }

        public  IQueryable<TEntity> GetAllAsync()
        {
           return   _dbContext.AsNoTracking();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbContext.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Create(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, TEntity entity)
        {
            _dbContext.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            _dbContext.Remove(entity);
            await _context.SaveChangesAsync();
        }

       
    }
}
