using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Messages.Commands
{
    public class CreateTaxiDriverCustomer : IIdentifiable, ICommand
    {    
        public string Id { get; set; }
        public string CustomerId { get; }

        public string Name { get; set; }

        public string Email { get; set; }


        public string PhoneNumber { get; set; }

        public DriverTypes Type { get; set; }



        [JsonConstructor]
        public CreateTaxiDriverCustomer(string Id, string CustomerId, string Name, string Email, string PhoneNumber, DriverTypes types)
        {
            this.Id = Id;
            this.CustomerId = CustomerId;
            this.Name = Name;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            Type = types;
        }
    }
}
