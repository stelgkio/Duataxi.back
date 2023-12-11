using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Common.Messages;
using System.Threading.Tasks;

namespace DuaTaxi.Common.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ICorrelationContext context);
    }
}