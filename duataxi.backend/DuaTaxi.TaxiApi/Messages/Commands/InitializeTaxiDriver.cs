using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Messages.Commands
{
    public class InitializeTaxiDriver : IIdentifiable, ICommand
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string ConnectionId { get; }      

        [JsonConstructor]
        public InitializeTaxiDriver(string id, string customerId,
            string connectionId)
        {
            Id = id;
            CustomerId = customerId;
            ConnectionId = connectionId;


        }

    }
}
