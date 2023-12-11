using DuaTaxi.Common.Mongo;
using DuaTaxi.Common.Types;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Repositories.ActiveDriversRepo
{
    public class ActiveTaxiDriverRepository : IActiveTaxiDriverRepository
    {
        IMongoRepository<ActiveDrivers> _repository;
        private ILogger<ActiveTaxiDriverRepository> _logger;
        public ActiveTaxiDriverRepository(IMongoRepository<ActiveDrivers> repository, ILogger<ActiveTaxiDriverRepository> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        #region   ActiveDrivers

        public async Task AddAsync(ActiveDrivers driver)
          => await _repository.AddAsync(driver);

        public async Task DeleteAsync(ActiveDrivers discount)
            => await _repository.DeleteAsync(discount.Id);

        public async Task<ActiveDrivers> GetCustomerAsync(string Id)
        {
            var xxx = await _repository.FindAsync(x => x.CustomerId == Id.ToString());

            return xxx.FirstOrDefault();
        }

        public async Task UpdateAsync(ActiveDrivers driver)
            => await _repository.UpdateAsync(driver);

        public async Task<ActiveDrivers> DeActivate(string CustomerId)
        {

            var activationStatus = await _repository.FindAsync(x => x.CustomerId == CustomerId);

            if (activationStatus.Count() == 0)
                throw new DuaTaxiException("User haven activate before", 0);

            activationStatus.FirstOrDefault().IsActive = false;

            await _repository.UpdateAsync(activationStatus.FirstOrDefault());
            return activationStatus.FirstOrDefault();
        }
        public async Task<ActiveDrivers> ReActivate(string CustomerId)
        {
            var activationStatus = await _repository.FindAsync(x => x.CustomerId == CustomerId);


            if (activationStatus.Count() == 0)
                throw new DuaTaxiException("User haven activate before", 0);


            if (activationStatus.FirstOrDefault().IsActive == false) {
                activationStatus.FirstOrDefault().IsActive = true;
            }
            else {
                throw new DuaTaxiException("User haven activate before", 0);
            }



            await _repository.UpdateAsync(activationStatus.FirstOrDefault());
            return activationStatus.FirstOrDefault();
        }



        #endregion
    }
}
