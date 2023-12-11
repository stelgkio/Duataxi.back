using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.AuthServer.Messages.Customers.Delete
{
    [MessageNamespace("taxiapi")]
    public class DeleteTaxiDriverCustomer : IIdentifiable, ICommand
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }


        [JsonConstructor]
        public DeleteTaxiDriverCustomer(string Id, string CustomerId)
        {
            this.Id = Id;
            this.CustomerId = CustomerId;
            
        }
    }
}
