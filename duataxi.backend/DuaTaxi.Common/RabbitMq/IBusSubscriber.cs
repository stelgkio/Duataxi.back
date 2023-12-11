using System;
using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;

namespace DuaTaxi.Common.RabbitMq
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
            Func<TCommand, DuaTaxiException, IRejectedEvent> onError = null)
            where TCommand : ICommand;

        IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
            Func<TEvent, DuaTaxiException, IRejectedEvent> onError = null) 
            where TEvent : IEvent;
    }
}
