using System.Threading.Tasks;
using DuaTaxi.Common.Messages;

namespace DuaTaxi.Common.Dispatchers
{
    public interface ICommandDispatcher
    {
         Task SendAsync<T>(T command) where T : ICommand;
    }
}