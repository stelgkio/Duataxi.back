using System.Threading.Tasks;
using DuaTaxi.Common.Types;
using DuaTaxi.Common.Messages;

namespace DuaTaxi.Common.Dispatchers
{
    public interface IDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}