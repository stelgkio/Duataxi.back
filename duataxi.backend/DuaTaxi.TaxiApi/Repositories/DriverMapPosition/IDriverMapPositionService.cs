using DuaTaxi.Services.TaxiApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Repositories.DriverMapPosition
{
    public interface IDriverMapPositionService
    {

        #region DriversMapPosition
        Task AddAsync(DriversMapPosition discount);
        Task UpdateAsync(DriversMapPosition discount);

        Task DeleteAsync(DriversMapPosition discount);

        Task<DriversMapPosition> GetCustomerAsync(string Id);
        #endregion
    }
}
