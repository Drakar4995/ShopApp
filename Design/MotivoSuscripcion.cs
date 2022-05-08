using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Design;

namespace ShopApp.Design
{
    public class MotivoSuscripcion
    {
        [Key]
        public virtual String Id
        {
            get;
            set;
        }
        public virtual String Motivo
        {
            get;
            set;
        }
        [ForeignKey("tituloSuscripcion")]

        public virtual Suscripcion Suscripcion
        {
            get;
            set;

        }

        public virtual string Titulo
        { get;
            set;
        }

        [ForeignKey("newsletterIdent")]

        public virtual NewsLetter NewsLetter
        {
            get;
            set;

        }

        public virtual string  NewsletterId
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            return obj is MotivoSuscripcion motivoSuscripcion &&
                   Id == motivoSuscripcion.Id &&
                   Suscripcion.Equals(motivoSuscripcion.Suscripcion) &&
                   Motivo == motivoSuscripcion.Motivo &&
                   Titulo == motivoSuscripcion.Titulo &&
                   NewsLetter.Equals(motivoSuscripcion.NewsLetter) &&
                   NewsletterId == motivoSuscripcion.NewsletterId;
        }
    }
}
