using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models.PrendaViewModels
{
    public class SelectPrendasForRetirarViewModel
    {
        public IEnumerable<Prenda> Prendas { get; set; }
    //Usado para filtrar por marca
        public SelectList Marcas;
        [Display(Name = "Marca")]
        public string prendaMarcaSelected { get; set; }
    //Usado para filtrar por número de ventas semanales de prenda
        [Display(Name = "Ventas Semanales")]
        public int prendaVentasSemanales { get; set; }
    }
}
