using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Messages.Commands
{
    public class TaxiDriverActivation : IIdentifiable, ICommand
    {
        public string Id { get; set; }

        public string CustomerId { get; set; }

        public double Latitude { get; set; }

        public double Longtitute { get; set; }

        [JsonConstructor]
        public TaxiDriverActivation(string Id, string customerId, double Latitude, double Longtitute)
        {
            this.Id = Id;
            CustomerId = customerId;
            this.Latitude= Latitude;
            this.Longtitute = Longtitute;
        }

    }
}
