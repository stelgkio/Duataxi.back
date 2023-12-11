using System.Threading.Tasks;
using DuaTaxi.Common.RabbitMq;

namespace DuaTaxi.Services.Operations.Services
{
    public interface IOperationPublisher
    {
        Task PendingAsync(ICorrelationContext context);
        Task CompleteAsync(ICorrelationContext context);
        Task RejectAsync(ICorrelationContext context, string code, string message);
    }
}