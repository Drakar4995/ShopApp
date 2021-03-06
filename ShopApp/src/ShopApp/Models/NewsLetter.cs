using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class NewsLetter
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
        public virtual String Descripcion
        {
            get;
            set;
        }
      
        [Required]
        public virtual Marca Marca
        {
            get;
            set;

        }

        [Required]
        public virtual Categoria Categoria
        {
            get;
            set;

        }

        public virtual IList<MotivoSuscripcion> MotivoSuscripcions
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            return obj is NewsLetter newsLetter &&
                   Id == newsLetter.Id &&
                   Titulo == newsLetter.Titulo &&
                   Descripcion == newsLetter.Descripcion &&
                   Marca.Equals(newsLetter.Marca) &&
                   Categoria.Equals(newsLetter.Categoria);

        }

    }
}
