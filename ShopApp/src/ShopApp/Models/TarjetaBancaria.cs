using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class TarjetaBancaria : MetodoPago
    {


        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "You have to introduce 16 numbers")]
        [Display(Name = "Credit Card")]
        [Required]
        public virtual string CreditCardNumber { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        [Required]
        public virtual string CCV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]

        public virtual DateTime ExpirationDate { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TarjetaBancaria card &&
                   base.Equals(obj) &&
                   ID == card.ID &&
                   CreditCardNumber == card.CreditCardNumber &&
                   CCV == card.CCV &&
                   ExpirationDate == card.ExpirationDate;
        }
    }
}