using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using ShopApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApp.Models
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
