using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Design;

namespace ShopApp.Design
{
    public class Gestor : UsuarioApp
    {
        public virtual IList<Retirada> Retiradas
        {
            get;
            set;
        }
    }
}
