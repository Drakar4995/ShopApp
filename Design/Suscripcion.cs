using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShopApp.Design;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApp.Design
{
    public class Suscripcion
    {
        [Key]
        public virtual int  suscripcionId
        {
            get;
            set;
        }
        public virtual String Titulo
        {
            get;
            set;
        }
        public virtual String Descripcion
        {
            get;
            set;
        }
        public virtual DateTime FechaCaducidad
        {
            get;
            set;
        }
        public virtual IList<MotivoSuscripcion> motivoSuscripcion
        {
            get;
            set;
        }

        [ForeignKey("ClienteID")]
        public virtual Cliente Cliente
        {
            get;
            set;

        }
        public virtual string clienteId
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
                        suscripcionId == suscripcion.suscripcionId &&
                        Titulo == suscripcion.Titulo &&
                   (this.FechaCaducidad.Subtract(suscripcion.FechaCaducidad) < new TimeSpan(0, 1, 0)) &&
                   Descripcion == suscripcion.Descripcion &&
                   Cliente.Equals(suscripcion.Cliente);
        }
    }


}
