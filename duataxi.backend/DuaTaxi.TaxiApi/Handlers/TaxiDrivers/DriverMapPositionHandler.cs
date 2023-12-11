using DuaTaxi.Common.Handlers;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Service.TaxiApi.Repositories;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using DuaTaxi.Services.TaxiApi.Messages.Commands;
using DuaTaxi.Services.TaxiApi.Repositories.ActiveDriversRepo;
using DuaTaxi.Services.TaxiApi.Repositories.DriverMapPosition;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers
{
    public class DriverMapPositionHandler : ICommandHandler<TaxiDriverMapPosition>
    {
        private readonly ITaxiDriverRepository _taxiDriverRepository;
        private readonly IActiveTaxiDriverRepository _activeTaxiDriverRepository;
        private readonly IDriverMapPositionService _driverMapPositionService;
        private readonly ILogger<DriverMapPositionHandler> _logger;

        public DriverMapPositionHandler(ITaxiDriverRepository taxiDriverRepository, IActiveTaxiDriverRepository activeTaxiDriverRepository, IDriverMapPositionService driverMapPositionService, ILogger<DriverMapPositionHandler> logger)
        {
            _taxiDriverRepository = taxiDriverRepository;
            _activeTaxiDriverRepository = activeTaxiDriverRepository;
            _driverMapPositionService = driverMapPositionService;
            _logger = logger;
        }


        public async Task HandleAsync(TaxiDriverMapPosition command, ICorrelationContext context)
        {
            var driver = await _activeTaxiDriverRepository.GetCustomerAsync(command.CustomerId);
            if (!driver.IsActive) {
                _logger.LogError($"Driver with id: {command.CustomerId}  not exist", "DriverMapPositionHandler---> set Driver Map Position");
                return;
            }

            await _activeTaxiDriverRepository.UpdateAsync(new ActiveDrivers(command.Id, command.CustomerId, command.Latitude, command.Longtitute, true));

            await _driverMapPositionService.UpdateAsync(new DriversMapPosition(command.Id, command.CustomerId, command.Latitude, command.Longtitute));

            return;
        }
    }
}
