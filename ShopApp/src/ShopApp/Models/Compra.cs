using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace ShopApp.Models
{
    public class Compra
    {
        [Key]
        [Display(Name = "Id Compra")]
        public int Id
        {
            get;
            set;
        }

        [Display(Name = "Precio Total")]
        [DataType(DataType.Currency)]
        public double PrecioTotal
        {
            get;
            set;
        }
        [Display(Name = "Fecha de Compra")]
        public DateTime FechaCompra
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]

        public String DireccionEnvio
        {
            get;
            set;
        }
        [Display(Name = "Numero de Articulos")]
        public virtual IList<ItemCompra> ItemsCompra
        {
            get;
            set;
        }
        public Compra()
        {

            ItemsCompra = new List<ItemCompra>();
        }
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente
        {
            get;
            set;
        }

        //It will be necessary whenever we need a relationship with ApplicationUser or any child class
       
        [Required]
        public string ClienteId
        {
            get;
            set;
        }

        [Display(Name = "Payment Method")]
        [Required]
        public MetodoPago MetodoPago
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            return obj is Compra compra &&
                   Id == compra.Id &&
                   PrecioTotal == compra.PrecioTotal &&
                   (this.FechaCompra.Subtract(compra.FechaCompra) < new TimeSpan(0, 1, 0)) &&
                   DireccionEnvio == compra.DireccionEnvio &&
                   Cliente.Equals(compra.Cliente) &&
                   ClienteId == compra.ClienteId &&
                   MetodoPago.Equals(compra.MetodoPago);

        }
    }

}


