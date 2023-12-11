using DuaTaxi.Common.Types;
using System;

namespace DuaTaxi.Service.Payments.Entities.Models
{
    public class Payment : BaseEntity
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



      
        public Payment(string id, string CustomerId, string Name, string Email, string PhoneNumber, DriverTypes types , PaymentTypes paymentTypes ,bool active, double DayOfExpiration) :base(id)
        {
            this.CustomerId = CustomerId;
            this.Name = Name;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            Type = types;
            PaymentType = paymentTypes;
            Active = active;
            this.DayOfExpiration = DayOfExpiration;



        }
    }
}
