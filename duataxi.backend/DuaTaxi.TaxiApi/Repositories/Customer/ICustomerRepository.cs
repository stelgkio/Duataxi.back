using DuaTaxi.Services.TaxiApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer discount);
        Task UpdateAsync(Customer discount);
        Task DeleteAsync(string Id);
        Task<Customer> GetAsync(string Id);
    }
}
