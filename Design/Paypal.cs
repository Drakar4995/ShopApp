using System;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.Design
{
    public class PayPal : MetodoPago
    {
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(3, MinimumLength = 2)]
        public string Prefix { get; set; }


        [StringLength(7, MinimumLength = 6)]

        public string Phone { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PayPal pal &&
                   base.Equals(obj) &&
                   ID == pal.ID &&
                   Email == pal.Email &&
                   Prefix == pal.Prefix &&
                   Phone == pal.Phone;
        }
    }
}