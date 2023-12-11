using DuaTaxi.Common.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Messages.Events.Customer
{
    public class CustomerCreated : IEvent
    {

        public Guid Id { get; protected set; }
        public Guid CustomerId { get; protected set; }


        [JsonConstructor]
        public CustomerCreated(Guid id, Guid customerId)
        {
            Id = id;
            CustomerId = customerId;
        }
    }
}
