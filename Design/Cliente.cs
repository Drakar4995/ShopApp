
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using ShopApp.Design;

namespace ShopApp.Design
{
    public class Cliente : UsuarioApp
    {
        public virtual IList<Compra> Compras
        {
            get;
            set;
        }
        public virtual IList<Suscripcion> Suscripcions
        {
            get;
            set;
        }


    }
}


