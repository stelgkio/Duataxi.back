using System;
using System.Collections.Generic;
using DuaTaxi.Common.Types;
using DuaTaxi.Service.TaxiApi.Dto;

namespace DuaTaxi.Service.TaxiApi.Query
{
    public class BrowseTaxiDrive: IQuery<IEnumerable<CreateTaxiDriverDto>>
    {
        public Guid CustomerId  { get; set; }
        
        
    }
}