using System.Threading.Tasks;
using DuaTaxi.Services.Signalr.Messages.Events;

namespace DuaTaxi.Services.Signalr.Services
{
    public interface IHubService
    {
        Task PublishOperationPendingAsync(OperationPending @event);
        Task PublishOperationCompletedAsync(OperationCompleted @event);
        Task PublishOperationRejectedAsync(OperationRejected @event);
    }
}