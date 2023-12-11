using DuaTaxi.Services.TaxiApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Repositories.ActiveDriversRepo
{
    public interface IActiveTaxiDriverRepository
    {
        #region ActiveDrivers
        Task AddAsync(ActiveDrivers discount);
        Task UpdateAsync(ActiveDrivers discount);

        Task DeleteAsync(ActiveDrivers discount);

        Task<ActiveDrivers> GetCustomerAsync(string Id);

        Task<ActiveDrivers> DeActivate(string CustomerId);

        Task<ActiveDrivers> ReActivate(string CustomerId);

        #endregion
    }
}
