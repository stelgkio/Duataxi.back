using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Entities.Models
{
    public class DriversMapPosition : BaseEntity
    {
        public string CustomerId { get; set; }

        public double Latitude { get; set; }

        public double Longtitute { get; set; }
       

        [JsonConstructor]
        public DriversMapPosition(string Id, string customerId, double Latitude, double Longtitute) : base(Id)
        {
            
            this.CustomerId = customerId;
            this.Latitude = Latitude;
            this.Longtitute = Longtitute;            
        }
    }
}
