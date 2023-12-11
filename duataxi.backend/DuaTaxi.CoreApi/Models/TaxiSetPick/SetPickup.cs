using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.CoreApi.Models.TaxiSetPick
{
    public class SetPickup : IIdentifiable, ICommand
    {           
        public string Id { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public string Address { get; set; }
    }
}
