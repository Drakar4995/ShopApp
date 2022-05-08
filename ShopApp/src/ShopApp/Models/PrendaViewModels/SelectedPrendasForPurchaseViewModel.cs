using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ShopApp.Models.PrendaViewModels
{
    public class SelectedPrendasForPurchaseViewModel
    {
        public string[] IdsToAdd { get; set; }
        [Display(Name = "Marca")]
        public string prendaMarcaSeleccionada { get; set; }

        [Display(Name = "Nombre")]
        public string prendaNombre { get; set; }

        [Display(Name = "Precio")]
        public int prendaPrecio { get; set; }

    }
}
