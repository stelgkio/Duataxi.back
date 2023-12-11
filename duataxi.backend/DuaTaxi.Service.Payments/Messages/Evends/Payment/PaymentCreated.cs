using DuaTaxi.Common.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.Payments.Messages.Evends.Payment
{
    public class PaymentCreated : IEvent
    {

        public Guid Id { get; protected set; }
        public Guid CustomerId { get; protected set; }


        [JsonConstructor]
        public PaymentCreated(Guid id, Guid customerId)
        {
            Id = id;
            CustomerId = customerId;
        }
    }
}
