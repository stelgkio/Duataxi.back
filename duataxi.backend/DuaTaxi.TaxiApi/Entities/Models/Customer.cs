using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Entities.Models
{
    public class Customer : BaseEntity
    {                     
        public string Name { get; set; }

        public string Email { get; set; }


        public string PhoneNumber { get; set; }

        public DriverTypes Type { get; set; }   
      
        public Customer(string id,  string Name, string Email, string PhoneNumber, DriverTypes types)  :base (id)
        {             
            
            this.Name = Name;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            this.Type = types;
        }
    }
}
