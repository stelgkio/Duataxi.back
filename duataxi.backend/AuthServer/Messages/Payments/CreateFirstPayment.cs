﻿using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;

namespace DuaTaxi.AuthServer.Messages.Payments
{
    [MessageNamespace("payment")]
    public class CreateFirstPayment : IIdentifiable, ICommand
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }


        public string PhoneNumber { get; set; }

        public DriverTypes Type { get; set; }

        //TODO: usos xreiazetai na ginei kapoios upologosimous me kapoio bisness logic
        public int DayOfExpiation { get; set; }



        [JsonConstructor]
        public CreateFirstPayment(string Id, string CustomerId, string Name, string Email, string PhoneNumber, DriverTypes types)
        {
            this.Id = Id;
            this.CustomerId = CustomerId;
            this.Name = Name;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            Type = types;
           

        }
    }
}
