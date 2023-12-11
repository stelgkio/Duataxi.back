using DuaTaxi.Services.TaxiApi.Entities.Models;
using DuaTaxi.Services.TaxiApi.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers.DriverActivation
{
    public interface ITaxiDriverActivationHandler
    {
        Task<ActiveDrivers> HandleAsync(TaxiDriverActivation taxiDriverActivation);
    }
}
