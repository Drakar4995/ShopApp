using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{


    public class Suscripcion
    {
        [Key]
        public virtual int Id
        {
            get;
            set;
        }

        public virtual String Titulo
        {
            get;
            set;
        }

        public virtual String motivo

        { get; set; }
        [Required]
        public virtual String Descripcion
        {
            get;
            set;
        }

        [Required]
        public virtual DateTime FechaCaducidad
        {
            get;
            set;
        }
        [Required]
        public virtual IList<MotivoSuscripcion> motivoSuscripcion
        {
            get;
            set;
        }

        [ForeignKey("ClienteID")]
        [NotMappedAttribute]
        public virtual  Cliente Cliente
        {
            get;
            set;

        }

        public virtual string clienteID
        {
            get;
            set;
        }
       
        public Suscripcion()
        {
            motivoSuscripcion = new List<MotivoSuscripcion>();
        }
        public override bool Equals(object obj)
        {
            return obj is Suscripcion suscripcion &&
                         Id == suscripcion.Id &&
                         Titulo == suscripcion.Titulo &&
                        //Marca.Equals(suscripcion.Marca) &&
                   (this.FechaCaducidad.Subtract(suscripcion.FechaCaducidad) < new TimeSpan(0, 1, 0)) &&
                   Descripcion == suscripcion.Descripcion;
                   //EqualityComparer<Cliente>.Default.Equals(Cliente, suscripcion.Cliente);
        }
    }

}