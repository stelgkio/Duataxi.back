using System;
using DuaTaxi.Common.Messages;
using Newtonsoft.Json;

namespace DuaTaxi.Services.Operations.Messages.Customers.Events
{
    [MessageNamespace("customers")]
    public class CartCleared : IEvent
    {
        public Guid CustomerId { get; }

        [JsonConstructor]
        public CartCleared(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}