using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Models;

namespace ShopApp.Design
{
    public class Prenda
    {
        [Key]
        public virtual int PrendaID
        {
            get;
            set;
        }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public virtual String Nombre
        {
            get;
            set;
        }



        [Required]
        [DataType(DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "Minimum price is 1 ")]
        [Display(Name = "Price For Purchase")]
        public virtual int PrecioPrenda
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        public virtual DateTime FechaLanzamiento
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Quantity For Purchase")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Purchase is 1")]
        public virtual int CantidadCompra
        {
            get;
            set;
        }

        [Required]
        public virtual Marca Marca
        {
            get;
            set;
        }



        public virtual IList<ItemCompra> PrendasCompradas
        {
            get;
            set;
        }

        public bool isRetired
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            return obj is Prenda prenda &&
                   PrendaID == prenda.PrendaID &&
                   Nombre == prenda.Nombre &&
                   PrecioPrenda == prenda.PrecioPrenda &&
                   FechaLanzamiento == prenda.FechaLanzamiento &&
                   CantidadCompra == prenda.CantidadCompra &&
                   isRetired == prenda.isRetired &&
                   Marca.Equals(prenda.Marca);

        }
    }
}