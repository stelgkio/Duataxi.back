using DuaTaxi.Service.TaxiApi.Entities;
using DuaTaxi.Services.TaxiApi.Messages.Commands;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Handlers.TaxiDrivers
{
    public interface IInitializeTaxiDriverHandler
    {
        Task<TaxiDriverStatus> HandleAsync(string customerId);
    }
}