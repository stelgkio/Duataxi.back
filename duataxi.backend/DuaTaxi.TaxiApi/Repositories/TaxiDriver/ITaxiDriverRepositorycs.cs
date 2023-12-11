using DuaTaxi.Service.TaxiApi.Entities;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Service.TaxiApi.Repositories
{
    public interface ITaxiDriverRepository
    {        
        #region TaxiDriverStatus
        Task AddAsync(TaxiDriverStatus discount);
        Task UpdateAsync(TaxiDriverStatus discount);

        Task DeleteAsync(TaxiDriverStatus discount);

        Task<TaxiDriverStatus> GetCustomerAsync(string Id);

        #endregion                 
    }
}
