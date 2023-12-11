using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Common.Handlers;
using DuaTaxi.Common.Mongo;
using DuaTaxi.Service.TaxiApi.Dto;
using DuaTaxi.Service.TaxiApi.Entities;
using DuaTaxi.Service.TaxiApi.Query;

namespace DuaTaxi.Service.TaxiApi.Handlers.TaxiDriver
{
    public class BrowseTaxiDriverHandler : IQueryHandler<BrowseTaxiDrive,IEnumerable<CreateTaxiDriverDto>>
    {
        private readonly IMongoRepository<TaxiDriverStatus> _driverRepo;

        public BrowseTaxiDriverHandler(IMongoRepository<TaxiDriverStatus> driverRepo )
        {
            _driverRepo = driverRepo;
        }
        
        public async Task<IEnumerable<CreateTaxiDriverDto>> HandleAsync(BrowseTaxiDrive query)
        {
            var taxidriver = await _driverRepo.FindAsync(x => x.CustomerId == query.CustomerId.ToString());
            return taxidriver.Select(d => new CreateTaxiDriverDto
            {
                Id= d.Id

            });
        }
    }
}