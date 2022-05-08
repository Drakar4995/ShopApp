using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models.ItemCompraViewModels

{
    public class SelectPrendasForDevolucionViewModel
    {
        public IList<ItemCompra> ItemCompras { get; set; }

        //para filtrar por Marca
        public SelectList Marcas;
        [Display(Name = "Marca")]
               
        public string marcaSelect{ get; set; }

        //para filtrar por Nombre Prenda
        [Display(Name = "Nombre Prenda")]
        public string nombrePrenda { get; set; }

        public int id { get; set; }


    }
}
