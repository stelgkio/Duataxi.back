using DuaTaxi.CoreApi.Models.Operation;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.CoreApi.Services
{
    public interface ITaxiService
    {
        [AllowAnyStatusCode]
        [Get("/{id}")]
        Task<Operation> GetAsync([Path] Guid id);


        [AllowAnyStatusCode]
        [Get("TaxiDriver/DeActivate/{CustomerId}")]
        Task<object> DeActivateDriverAsync([Path] string CustomerId);
    }
}

