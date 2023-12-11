using DuaTaxi.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.Services.TaxiApi.Entities.DTO
{
    public class PaymentDto
    {
        public string CustomerId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }


        public string PhoneNumber { get; set; }

        public DriverTypes Type { get; set; }

        public bool Active { get; set; }

        public PaymentTypes PaymentType { get; set; }

        //TODO: usos xreiazetai na ginei kapoios upologosimous me kapoio bisness logic
        public double DayOfExpiration { get; set; }



    }
}
