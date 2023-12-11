using DuaTaxi.Services.TaxiApi.Entities.DTO;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers.SetPickUp
{
   public interface ISetPickUpHandler
    {
        Task<ActiveDrivers> HandleAsync(SetPickUpDto setPick);
    }
}
