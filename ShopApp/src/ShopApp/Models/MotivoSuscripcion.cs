using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class MotivoSuscripcion
    {
        [Key]
        public virtual int Id
        {
            get;
            set;
        }
        
        [ForeignKey("suscripcionId")]
        public virtual Suscripcion Suscripcion
        {
            get;
            set;

        }

        public virtual int suscripcionId
        {
            get;
            set;
        }

        

        [ForeignKey("newsletterId")]

        public virtual NewsLetter NewsLetter
        {
            get;
            set;

        }

        public virtual int newsletterId
        {
            get;
            set;
        }


        public override bool Equals(object obj)
        {
            return obj is MotivoSuscripcion motivoSuscripcion &&
                   Id == motivoSuscripcion.Id &&
                   Suscripcion.Equals(motivoSuscripcion.Suscripcion)
                   &&
        
                   suscripcionId == motivoSuscripcion.suscripcionId &&
                   NewsLetter.Equals(motivoSuscripcion.NewsLetter) 
                   &&
                   newsletterId == motivoSuscripcion.newsletterId;
        }
    }
}
