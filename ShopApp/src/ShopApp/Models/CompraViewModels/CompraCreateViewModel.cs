using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;



namespace ShopApp.Models.CompraViewModels
{
    public class CompraCreateViewModel : IValidatableObject
    {
        public virtual string Nombre
        {
            get;
            set;
        } 
        [Display(Name = "First Surname")]
        public virtual string FirstSurname
        {
            get;
            set;
        }

        [Display(Name = "Second Surname")]
        public virtual string SecondSurname
        {
            get;
            set;
        }

        //It will be necessary whenever we need a relationship with ApplicationUser or any child class
        public string ClienteId
        {
            get;
            set;
        }

        public double PrecioTotal
        {
            get;
            set;
        }

        public DateTime FechaCompra
        {
            get;
            set;
        }

        
        public virtual IList<ItemCompraViewModel> ItemsCompra
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Direccion de envio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca una direccion de envio valida")]

        public String DireccionEnvio
        {
            get;
            set;
        }

        [Display(Name = "Metodo Pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Seleccione un Metodo de Pago")]
        public String MetodoPago
        {
            get;
            set;
        }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(3, MinimumLength = 2)]
        public string Prefix { get; set; }


        [StringLength(7, MinimumLength = 6)]

        public string Phone { get; set; }

        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Tienes que introducir 16 numeros")]
        [Display(Name = "Credit Card")]
        public virtual string CreditCardNumber { get; set; }

        [RegularExpression(@"^[0-9]{3}$",ErrorMessage = "Tienes que introducir 3 numeros")]
        public virtual string CCV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime? ExpirationDate { get; set; }

        public CompraCreateViewModel()
        {

            ItemsCompra = new List<ItemCompraViewModel>();
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj is CompraCreateViewModel model)
                result = Nombre == model.Nombre &&
                  FirstSurname == model.FirstSurname &&
                  SecondSurname == model.SecondSurname &&
                  ClienteId == model.ClienteId &&
                  PrecioTotal == model.PrecioTotal &&
                  FechaCompra == model.FechaCompra &&
                  DireccionEnvio == model.DireccionEnvio &&
                  MetodoPago == model.MetodoPago &&
                  Email == model.Email &&
                  Prefix == model.Prefix &&
                  Phone == model.Phone &&
                  CreditCardNumber == model.CreditCardNumber &&
                  CCV == model.CCV &&
                  ExpirationDate == model.ExpirationDate;
            else
                return false;
            for (int i = 0; i < this.ItemsCompra.Count; i++)
                result = result && (this.ItemsCompra[i].Equals(model.ItemsCompra[i]));

            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MetodoPago == "CreditCard")
            {
                if (CreditCardNumber == null)
                    yield return new ValidationResult("Please, fill in your Credit Card Number for your Credit Card payment",
                        new[] { nameof(CreditCardNumber) });
                if (CCV == null)
                    yield return new ValidationResult("Please, fill in your CCV for your Credit Card payment",
                        new[] { nameof(CCV) });
                if (ExpirationDate == null)
                {
                    yield return new ValidationResult("Please, fill in your ExpirationDate for your Credit Card payment",
                      new[] { nameof(ExpirationDate) });
                }
            }
            else
            {
                if (Email == null)
                    yield return new ValidationResult("Please, fill in your Email for your PayPal payment",
                        new[] { nameof(Email) });
                if (Prefix == null)
                    yield return new ValidationResult("Please, fill in your Prefix for your PayPal payment",
                        new[] { nameof(Prefix) });
                if (Phone == null)
                    yield return new ValidationResult("Please, fill in your Phone for your PayPal payment",
                        new[] { nameof(Phone) });
            }

            //it is checked whether quantity is higher than 0 for at least one movie
            if (ItemsCompra.Sum(pi => pi.Cantidad) <= 0)
                yield return new ValidationResult("Por favor, selecciona mas de una prenda para realizar el pedido",
                     new[] { nameof(ItemsCompra) });

        }
    }

    public class ItemCompraViewModel
    {
        public virtual int PrendaID
        {
            get;
            set;
        }


        [StringLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres.")]
        public virtual String Nombre
        {
            get;
            set;
        }


        [Display(Name = "Precio Prenda")]
        public virtual int PrecioPrenda
        {
            get;
            set;
        }


        public virtual String Marca
        {
            get;
            set;
        }

        [Required]
        public virtual int Cantidad
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            ItemCompraViewModel purchaseItem = obj as ItemCompraViewModel;
            bool result = false;
            if ((PrendaID == purchaseItem.PrendaID)
                && (this.PrecioPrenda == purchaseItem.PrecioPrenda)
                    && (this.Cantidad == purchaseItem.Cantidad)
                    && (this.Nombre == purchaseItem.Nombre))
                result = true;
            return result;
        }
    }
}