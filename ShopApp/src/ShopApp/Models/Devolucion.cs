using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class Devolucion
    {
        [Key]
        public int ID
        { 
            get;
            set;
        }
         

        public virtual IList<ItemDevolucion> ItemsDevolucion
        {
            get;
            set;
        }
        [Display(Name = "Fecha de Devolucion")]
        public virtual DateTime FechaDevolucion
        {
            get;
            set;
        }
        [Display(Name = "Tarjeta / PayPal")]
        public virtual string MetodoDevolucion
        {
            get;
            set;
        }

        [ForeignKey("MetodoPagoId")]
        [Display(Name = "Metodo de Pago")]
        public virtual MetodoPago MetodoPago
        {
            get;
            set;

        }
       
        [ForeignKey ("clienteId")]
        public virtual Cliente cliente
        {
            get;
            set;
        }
        public virtual string clienteId
        {
            get;
            set;
        }
        [Display(Name="Total a Devolver")]
        [DataType(DataType.Currency)]
        public virtual int precioTotal
        {
            get;
            set;
        }
        [Display(Name = "Recogido en")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Seleccione un metodo de Recogida")]
        public virtual String tipoRecogida
        {
            get;
            set;
        }
        public virtual string Direccion
        {
            get; set;
        }
        public override bool Equals(object obj)
        {
            return obj is Devolucion item &&
                   ID == item.ID &&
                   MetodoPago.Equals(item.MetodoPago) &&
                   (this.FechaDevolucion.Subtract(item.FechaDevolucion)) < new TimeSpan(0, 1, 0) &&
                   MetodoDevolucion == item.MetodoDevolucion &&
                   clienteId == item.clienteId &&
                   tipoRecogida == item.tipoRecogida &&
                   cliente.Equals(item.cliente);
        }
    }
}
