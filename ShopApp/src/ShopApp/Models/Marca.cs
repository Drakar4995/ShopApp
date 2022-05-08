using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    
    public class Marca
    {

        [Key]
        [Required]
        public virtual int MarcaID
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
            return obj is Marca marca &&
            MarcaID == marca.MarcaID &&
            Nombre == marca.Nombre;
        }
    }
}
