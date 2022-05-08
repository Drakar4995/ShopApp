using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Design;

namespace ShopApp.Design
{
    public class Categoria
    {
        [Key]
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
