using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.TaxiApi.Messages.Commands
{
    public class CreateTaxiDriver: BaseEntity, ICommand
    {                       
        public Guid CustomerId { get; protected set; }
        public string ConnectionId { get; protected set; }

        public bool Status { get; protected set; }


        [JsonConstructor]
        public CreateTaxiDriver(string id, Guid customerId,
            string connectionId, bool status)
        {
            Id = id;
            CustomerId = customerId;
            ConnectionId = connectionId;
            Status = status;

        }
    }
}
