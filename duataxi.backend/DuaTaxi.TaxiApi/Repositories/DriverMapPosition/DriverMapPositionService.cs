using DuaTaxi.Common.Mongo;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Repositories.DriverMapPosition {
    public class DriverMapPositionService : IDriverMapPositionService {
        IMongoRepository<DriversMapPosition> _repository;

        public DriverMapPositionService(IMongoRepository<DriversMapPosition> repository)
        {
            _repository = repository;
        }

        #region DriversMapPosition
        public async Task AddAsync(DriversMapPosition driver)
            => await _repository.AddAsync(driver);

        public async Task DeleteAsync(DriversMapPosition discount)
            => await _repository.DeleteAsync(discount.Id);

        public async Task<DriversMapPosition> GetCustomerAsync(string Id) {
            var Customer = await _repository.FindAsync(x => x.CustomerId == Id.ToString());

            return Customer.FirstOrDefault();
        }
        public async Task UpdateAsync(DriversMapPosition driver)
            => await _repository.UpdateAsync(driver);

        #endregion
    }
}
