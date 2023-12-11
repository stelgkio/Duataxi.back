using System.Threading.Tasks;
using Chronicle;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Services.Operations.Messages.TaxiApi.Events;
using DuaTaxi.Services.Operations.Messages.Notifications.Commands;

namespace DuaTaxi.Services.Operations.Sagas
{
    public class TaxiDriverCreatedSaga : Saga,
        ISagaStartAction<TaxiDriverCreated>
    {
        private readonly IBusPublisher _busPublisher;

        public TaxiDriverCreatedSaga(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }
        
        public Task HandleAsync(TaxiDriverCreated message, ISagaContext context)
        {
            //    return _busPublisher.SendAsync(new SendEmailNotification("user1@dshop.com",
            //            "Discount", $"New discount: {message.Id}"),
            //        CorrelationContext.Empty);
            return Task.CompletedTask;
        }

        public Task CompensateAsync(TaxiDriverCreated message, ISagaContext context)
        {
            return Task.CompletedTask;
        }
    }

    public class State
    {
        public string Code { get; set; }
    }
}