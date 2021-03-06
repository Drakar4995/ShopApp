using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class Categoria
    {
        [Key]
        public int CategoriaID
        {
            get;
            set;
        }

        [Required]
        public virtual String Nombre
        {
            get;
            set;

        }
        public override bool Equals(object obj)
        {
            return obj is Categoria categoria &&
            Nombre == categoria.Nombre;
        }

    }
}
