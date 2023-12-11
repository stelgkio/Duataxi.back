using DuaTaxi.Common.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.TaxiApi.Messages.Events
{
    public class TaxiDriverCreated : IEvent
    {
        public string Id { get; protected set; }
        public Guid CustomerId { get; protected set; }
       

        [JsonConstructor]
        public TaxiDriverCreated(string id, Guid customerId)
        {
            Id = id;
            CustomerId = customerId;          
        }
    
    }
}
