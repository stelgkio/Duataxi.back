using System.Threading.Tasks;
using DuaTaxi.Common.Handlers;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Services.Signalr.Messages.Events;
using DuaTaxi.Services.Signalr.Services;

namespace DuaTaxi.Services.Signalr.Handlers
{
    public class OperationUpdatedHandler : IEventHandler<OperationPending>,IEventHandler<OperationCompleted>, IEventHandler<OperationRejected>
    {
        private readonly IHubService _hubService;
        
        public OperationUpdatedHandler(IHubService hubService)
        {
            _hubService = hubService;
        }

        public async Task HandleAsync(OperationPending @event, ICorrelationContext context)
            => await _hubService.PublishOperationPendingAsync(@event);

        public async Task HandleAsync(OperationCompleted @event, ICorrelationContext context)
            => await _hubService.PublishOperationCompletedAsync(@event);

        public async Task HandleAsync(OperationRejected @event, ICorrelationContext context)
            => await _hubService.PublishOperationRejectedAsync(@event);
    }
}