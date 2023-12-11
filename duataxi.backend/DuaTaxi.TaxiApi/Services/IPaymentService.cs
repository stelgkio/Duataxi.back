using DuaTaxi.Services.TaxiApi.Entities.DTO;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IPaymentService
    {
        [AllowAnyStatusCode]
        [Get("/{customerid}")]
        Task<PaymentDto> GetAsync([Path] string customerid);
    }
}
