using DuaTaxi.Common.Handlers;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Service.TaxiApi.Entities;
using DuaTaxi.Service.TaxiApi.Repositories;
using DuaTaxi.Services.TaxiApi.Messages.Commands;
using DuaTaxi.Services.TaxiApi.Messages.Events.InitTaxiDriver;
using DuaTaxi.Services.TaxiApi.Repositories;
using DuaTaxi.Services.TaxiApi.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers
{
    public class InitializeTaxiDriverHandler : IInitializeTaxiDriverHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITaxiDriverRepository _taxidriverRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<InitializeTaxiDriverHandler> _logger;
        private readonly IPaymentService _paymentService;

        public InitializeTaxiDriverHandler(
            IPaymentService paymentService,
            ITaxiDriverRepository taxidriverRepository,
            IBusPublisher busPublisher,
            ILogger<InitializeTaxiDriverHandler> logger,
            ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _taxidriverRepository = taxidriverRepository;
            _busPublisher = busPublisher;
            _logger = logger;
            _paymentService = paymentService;
        }
        public async Task<TaxiDriverStatus> HandleAsync(string CustomerId)
        {      
            var driverExist = await _customerRepository.GetAsync(CustomerId);

            if (driverExist is null) {
                ////add to TaxiDriverStatus
                var status = new TaxiDriverStatus(CustomerId, "", false, "No Customer");
                await _taxidriverRepository.AddAsync(status);

                _logger.LogError("Driver not exist", null);

                return status;
            }

            // Get From Payment-Service Payment for 
            var payment = await _paymentService.GetAsync(CustomerId);

            if (payment is null) {

                var status = new TaxiDriverStatus(CustomerId, "", false, "No Payment Exist");
                await _taxidriverRepository.AddAsync(status);

                _logger.LogError("No Payment Exist", null);

                return status;
            }



            if (payment.DayOfExpiration == 0) {

                var status = new TaxiDriverStatus(CustomerId, "", false, "Payment expired", payment.DayOfExpiration);
                await _taxidriverRepository.AddAsync(status);


                _logger.LogError("Payment expired", null);
                return status;


            }
            else if (payment.DayOfExpiration > 3) {
                //add to TaxiDriverStatus    
                var status = new TaxiDriverStatus(CustomerId, "", true, "3 days left", payment.DayOfExpiration);
                await _taxidriverRepository.AddAsync(status);

                return status;

            }
            else {
                //add to TaxiDriverStatus

                var status = new TaxiDriverStatus(CustomerId, "", true, null, payment.DayOfExpiration);
                await _taxidriverRepository.AddAsync(status);

                return status;
            }



        }
    }
}
