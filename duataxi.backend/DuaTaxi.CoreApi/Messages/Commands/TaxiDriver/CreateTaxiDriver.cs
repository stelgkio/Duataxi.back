using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.CoreApi.Messages.Commands.TaxiDriver
{
    [MessageNamespace("taxiapi")]
    public class CreateTaxiDriver : BaseEntity, ICommand
    {       
        public Guid CustomerId { get; set; }
       
        public string ConnectionId { get;  }

        public bool Status { get; }



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
