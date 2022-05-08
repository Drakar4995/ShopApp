using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ShopApp.Models.ItemCompraViewModels

{
    public class SelectedPrendasForDevolucionViewModel
    {
        public string[] IdsToAdd { get; set; }
        public string marcaSelect { get; set; }

        //para filtrar por Nombre Prenda

        public string nombrePrenda { get; set; }

        public int id { get; set; }

    }
}
