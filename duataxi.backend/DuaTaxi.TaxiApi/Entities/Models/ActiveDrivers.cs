using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Entities.Models
{
    public class ActiveDrivers  : BaseEntity
    {
        public string CustomerId { get; set; }

        public double Latitude { get; set; }

        public double Longtitute { get; set; }

        public bool IsActive { get; set; }

        [JsonConstructor]
        public ActiveDrivers(string Id, string customerId, double Latitude, double Longtitute, bool IsActive) : base(Id)
        {
            
            this.CustomerId = customerId;
            this.Latitude = Latitude;
            this.Longtitute = Longtitute;
            this.IsActive = IsActive;                                    
        }
    }
}
