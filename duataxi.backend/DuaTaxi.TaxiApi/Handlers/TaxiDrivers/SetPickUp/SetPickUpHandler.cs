using DuaTaxi.Services.TaxiApi.Entities.DTO;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using DuaTaxi.Services.TaxiApi.Repositories.ActiveDriversRepo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers.SetPickUp
{
    public class SetPickUpHandler : ISetPickUpHandler
    {
        private readonly IActiveTaxiDriverRepository _activeTaxiDriverRepository;
        private readonly ILogger<SetPickUpHandler> _logger;

        public SetPickUpHandler(IActiveTaxiDriverRepository activeTaxiDriverRepository, ILogger<SetPickUpHandler> logger)
        {
            _activeTaxiDriverRepository = activeTaxiDriverRepository;
            _logger = logger;
        }
        public Task<ActiveDrivers> HandleAsync(SetPickUpDto setPick)
        {
            throw new NotImplementedException();
        }
    }
}
