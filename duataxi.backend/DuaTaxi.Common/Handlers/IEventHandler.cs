using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Common.Messages;
using System.Threading.Tasks;

namespace DuaTaxi.Common.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, ICorrelationContext context);
    }
}