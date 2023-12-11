using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.BusApi.Messages.Commands
{
    
    public class DeleteBusDriverCustomer : IIdentifiable, ICommand
    {

        public string Id { get; set; }
        public string CustomerId { get; set; }

    

        [JsonConstructor]
        public DeleteBusDriverCustomer(string Id, string CustomerId)
        {
            this.Id = Id;
            this.CustomerId = CustomerId;
            
        }
    }
}
