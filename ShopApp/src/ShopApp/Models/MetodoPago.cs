using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public abstract class MetodoPago
    {
        [Key]
        public virtual int ID
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            return obj is MetodoPago method &&
                   ID == method.ID;
        }
    }
    
}
