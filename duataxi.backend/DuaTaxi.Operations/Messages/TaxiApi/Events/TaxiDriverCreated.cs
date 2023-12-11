using System;
using DuaTaxi.Common.Messages;
using Newtonsoft.Json;

namespace DuaTaxi.Services.Operations.Messages.TaxiApi.Events
{
    [MessageNamespace("taxiapi")]
    public class TaxiDriverCreated : IEvent
    {
        public Guid Id { get; }
        public Guid CustomerId { get; protected set; }
        public string ConnectionId { get; protected set; }  
        public bool Status { get; protected set; }


        [JsonConstructor]
        public TaxiDriverCreated(Guid id, Guid customerId,
            string connectionId, bool status)
        {
            Id = id;
            CustomerId = customerId;
            ConnectionId = connectionId;
            Status = status;
        }
    }
}