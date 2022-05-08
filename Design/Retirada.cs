using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Design;

namespace ShopApp.Design
{
    public class Retirada
    {
        [Key]
        public int id
        {
            get;
            set;
        }

        [Required]
        public string titulo
        {
            get;
            set;

        }

        [Required]
        public string descripcion
        {
            get;
            set;
        }

        public DateTime fechaEfectiva
        {

            get;
            set;

        }

        public virtual IList<MotivoRetirada> motivosRetirada
        {
            get;
            set;
        }

        public Retirada()
        {

            motivosRetirada = new List<MotivoRetirada>();
        }

        [ForeignKey("gestorId")]
        public virtual Gestor Gestor
        {
            get;
            set;

        }

        public string gestorId
        {
            get;
            set;

        }

        public override bool Equals(object obj)
        {
            return obj is Retirada retirada &&
            id == retirada.id &&
            titulo == retirada.titulo &&
            descripcion == retirada.descripcion &&
            fechaEfectiva == retirada.fechaEfectiva &&
            Gestor.Equals(retirada.Gestor) &&
            gestorId == retirada.gestorId;
        }

    }
}
