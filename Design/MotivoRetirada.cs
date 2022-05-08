using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Design;


namespace ShopApp.Design
{
    public class MotivoRetirada
    {
        [Key]
        public virtual int id
        {

            get;
            set;
        }

        [Required]
        public virtual String descripcion
        {

            get;
            set;

        }

        [ForeignKey("retiradaId")]
        public virtual Retirada Retirada
        {
            get;
            set;

        }

        public virtual int retiradaId
        {
            get;
            set;

        }

        [ForeignKey("prendaId")]
        public virtual Prenda Prenda
        {

            get;
            set;

        }

        public virtual int prendaId
        {

            get;
            set;
        }

        public override bool Equals(object obj)
        {
            return obj is MotivoRetirada motivoRetirada &&
            id == motivoRetirada.id &&
            descripcion == motivoRetirada.descripcion &&
            Retirada.Equals(motivoRetirada.Retirada) &&
            retiradaId == motivoRetirada.retiradaId &&
            Prenda.Equals(motivoRetirada.Prenda) &&
            prendaId == motivoRetirada.prendaId;
        }
    }
}
