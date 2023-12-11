using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Common.Mongo;
using DuaTaxi.Services.TaxiApi.Entities.Models;

namespace DuaTaxi.Services.TaxiApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        IMongoRepository<Customer> _repository;
        public CustomerRepository(IMongoRepository<Customer> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(Customer discount)
        => await _repository.AddAsync(discount);

        public async Task DeleteAsync(string Id)
         => await _repository.DeleteAsync(Id);

        public async Task<Customer> GetAsync(string Id)
        => await _repository.GetAsync(Id);

        public async Task UpdateAsync(Customer discount)
         => await _repository.UpdateAsync(discount);
    }
}
