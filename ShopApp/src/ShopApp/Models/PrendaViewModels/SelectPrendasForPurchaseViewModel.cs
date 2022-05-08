using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models.PrendaViewModels
{
    public class SelectPrendasForPurchaseViewModel
    {
        public IEnumerable<Prenda> Prendas { get; set; }

        //para filtrar por Marca
        public SelectList Marcas;
        [Display(Name = "Marca")]
        public string prendaMarcaSeleccionada { get; set; }

        //para filtrar por nombre
        [Display(Name = "Nombre")]
        public string prendaNombre { get; set; }

        //para filtrar por precio
        [Display(Name = "Precio")]
        public int prendaPrecio { get; set; }



    }
}
