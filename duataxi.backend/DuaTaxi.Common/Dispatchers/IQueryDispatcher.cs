using System.Threading.Tasks;
using DuaTaxi.Common.Types;

namespace DuaTaxi.Common.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}