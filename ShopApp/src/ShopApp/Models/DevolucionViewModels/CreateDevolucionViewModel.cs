using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ShopApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApp.Models.DevolucionViewModels
{
    public class CreateDevolucionViewModel : IValidatableObject
    {
        //[DataType(DataType.MultilineText)]
        [Display(Name = "Recogida en")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Seleccione un metodo de Recogida")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Seleccione un tipo de Recogida")]
        public virtual String tipoRecogida
        {
            get;set;
        }
       
        public  virtual string idCliente
        {
            get;
            set;
        }
        public virtual string Nombre
        {
            get;
            set;
        }
        [Display(Name = "Primer Apellido")]
        public virtual string PrimerApellido
        {
            get;
            set;
        }

        [Display(Name = "Segundo Apellido")]
        public virtual string SegundoApellido
        {
            get;
            set;
        }
        public virtual IList<ItemDevolucionViewModel> ItemsDevolucion
        {
            get;
            set;
        }
                        
        public String Direccion
        {
            get;
            set;
        }

        [Display(Name = "Metodo de Pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Seleccione un Metodo de Pago")]
        public String PaymentMethod
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

        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Tienes que introducir 16 digitos")]
        [Display(Name = "Credit Card")]
        [StringLength(16)]
        public virtual string CreditCardNumber { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        [StringLength(3,MinimumLength = 2)]
        public virtual string CCV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime? ExpirationDate { get; set; }

        public CreateDevolucionViewModel()
        {
            ItemsDevolucion = new List<ItemDevolucionViewModel>();
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PaymentMethod == "CreditCard")
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
               else
                {
                    if (DateTime.Compare((DateTime)ExpirationDate, DateTime.Now) < 0)
                        yield return new ValidationResult("La Tarjeta esta Caducada",
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
            if(tipoRecogida == null)
            {
                yield return new ValidationResult("Seleccione un metodo de Recogida",
                       new[] { nameof(tipoRecogida) });
            }
               
            if (tipoRecogida == "Domicilio")
            {
                if (Direccion == null)
                    yield return new ValidationResult("Introduza una direccion Valida",
                    new[] { nameof(Direccion) });
            }
            
        }


            public override bool Equals(object obj)
            {
                bool result;
                if (obj is CreateDevolucionViewModel model)
                    result = Nombre == model.Nombre &&
                        PrimerApellido == model.PrimerApellido &&
                        SegundoApellido == model.SegundoApellido &&
                        Direccion == model.Direccion &&
                        idCliente == model.idCliente &&
                        tipoRecogida == model.tipoRecogida &&
                        PaymentMethod == model.PaymentMethod &&
                        Email == model.Email &&
                        Prefix == model.Prefix &&
                        Phone == model.Phone &&
                        CreditCardNumber == model.CreditCardNumber &&
                        CCV == model.CCV &&
                        ExpirationDate == model.ExpirationDate;
                else
                    return false;
            for (int i = 0; i < this.ItemsDevolucion.Count; i++)
                result = result && (this.ItemsDevolucion[i].Equals(model.ItemsDevolucion[i]));
            return result; 
            }


    }
    

    public class ItemDevolucionViewModel
    {
        public virtual int Id
        {
              get;
              set;

        }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Motivo Devolucion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca un Motivo")]
        public virtual String motivoDevolucion
        {
            get;
            set;
        }




        public virtual int CompraId
            {
                get;
                set;
            }
            
            
            public virtual int ItemCompraID
            {
                get;
                set;
            }
            
            public virtual string nombrePrenda
            {
                get;
                set;
            }
        [DataType(DataType.Currency)]
        public virtual int precio
            {
            get;
            set;
            }
            public virtual int cantidad
            {
            get;
            set;
             }

            
            public virtual int DevolucionID
            {
                get;
                set;
            }
            
            public virtual string nombreMarca
             {
                  get;
                  set;
             }
            public override bool Equals(object obj)
            {
                ItemDevolucionViewModel item = obj as ItemDevolucionViewModel;
                bool result = false;

                if ((Id == item.Id)
                    && (this.DevolucionID == item.DevolucionID)
                    && (this.ItemCompraID == item.ItemCompraID))
                result = true;
            
                return result;
                            
            }
        }
    
}
