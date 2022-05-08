using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models.NewsletterViewModels
{
    public class SelectNewsletterForSuscribeViewModel
    {
        public IEnumerable<NewsLetter> Newsletters { get; set; }

        //needed to populate a dropdownlist 
        public SelectList marcaNombres;
        //needed to store the genre selected by the user
        [Display(Name = "Marca")]
        public string newslettermarcaselected { get; set; }

        [Display(Name = "Categoria")]
        public string newsletterCategoria { get; set; }
    }
}
