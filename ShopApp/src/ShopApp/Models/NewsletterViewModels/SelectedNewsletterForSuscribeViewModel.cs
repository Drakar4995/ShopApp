using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models.NewsletterViewModels
{
    public class SelectedNewsletterForSuscribeViewModel
    {
        public int[] IdsToAdd { get; set; }
        [Display(Name = "Marca")]
        public string newslettermarcaselected { get; set; }

        [Display(Name = "Categoria")]
        public string newsletterCategoria { get; set; }
    }
}
