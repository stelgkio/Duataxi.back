using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.MiniBusApi.Messages.Commands
{
    [MessageNamespace("minibusapi")]
    public class DeleteMiniBusDriverCustomer : IIdentifiable, ICommand
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }


     



        [JsonConstructor]
        public DeleteMiniBusDriverCustomer( string Id, string CustomerId )
        {
            this.Id = Id;
            this.CustomerId = CustomerId;
          
        }
    }
}
