using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models.PrendaViewModels
{
    public class SelectedPrendasForRetirarViewModel
    {
        public string[] IdsToAdd { get; set; }
        [Display(Name = "Marca")]
        public string prendaMarcaSelected { get; set; }

        [Display(Name = "VentasSemanales")]
        public int prendaVentasSemanales { get; set; }

    }
}
