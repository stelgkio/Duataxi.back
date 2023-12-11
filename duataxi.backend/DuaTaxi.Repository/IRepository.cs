
using DuaTaxi.Entities.Core.Base;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Repository
{
   public interface IRepository<TEntity> where TEntity: BaseEntity 
    {
        IQueryable<TEntity> GetAllAsync();

        Task<TEntity> GetById(int id);

        Task Create(TEntity entity);

        Task Update(int id, TEntity entity);

        Task Delete(int id);
    }
}
