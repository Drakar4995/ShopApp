using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class Retirada
    {
        [Key]
        public int id
        {
            get;
            set;
        }

        [Display(Name = "Titulo")]
        [Required]
        public String titulo
        {
            get;
            set;

        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripcion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, escribe una descripcion")]
        public String descripcion
        {
            get;
            set;
        }

        [Display(Name = "Fecha Efectiva")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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

       // [ForeignKey("gestorId")]
        public virtual Gestor Gestor
        {
            get;
            set;

        }

        public String gestorId
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
            (this.fechaEfectiva.Subtract(retirada.fechaEfectiva) < new TimeSpan(0, 1, 0)) &&
            Gestor.Equals(retirada.Gestor) &&
            gestorId == retirada.gestorId;
        }

    }
}
