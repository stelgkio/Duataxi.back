using DuaTaxi.Common.Mongo;
using DuaTaxi.Service.TaxiApi.Entities;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.TaxiApi.Repositories
{
    public class TaxiDriverRespository : ITaxiDriverRepository
    {
        IMongoRepository<TaxiDriverStatus> _repository;

     
        public TaxiDriverRespository(IMongoRepository<TaxiDriverStatus> repository)
        {
            _repository = repository;
        }

        #region TaxiDriverStatus
        public async Task AddAsync(TaxiDriverStatus driver)        
            => await _repository.AddAsync(driver);

        public async Task DeleteAsync(TaxiDriverStatus discount)
            => await _repository.DeleteAsync(discount.Id);

        public async Task<TaxiDriverStatus> GetCustomerAsync(string Id)
        {
            var Customer = await _repository.FindAsync(x => x.CustomerId == Id.ToString());

            return Customer.FirstOrDefault();
        }
        public async Task UpdateAsync(TaxiDriverStatus driver)
            => await _repository.UpdateAsync(driver);

        #endregion

       
    }
}
