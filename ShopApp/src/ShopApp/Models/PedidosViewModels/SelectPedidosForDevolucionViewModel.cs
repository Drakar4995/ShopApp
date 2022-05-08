using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models.PedidosViewModels
{
    public class SelectPedidosForDevolucionViewModel
    {
        public IEnumerable<Compra> Compras { get; set; }

        public string[] IdsToAdd { get; set; }

        public string idPedido { get; set; }

    }
}
