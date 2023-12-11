using System.Threading.Tasks;
using DuaTaxi.Common.Types;

namespace DuaTaxi.Common.Handlers
{
    public interface IQueryHandler<TQuery,TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}