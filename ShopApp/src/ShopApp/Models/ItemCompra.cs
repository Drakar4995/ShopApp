using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace ShopApp.Models
{
    public class ItemCompra
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        [ForeignKey("PrendaID")]
        public virtual Prenda Prenda
        {
            get;
            set;
        }
        public virtual int PrendaID
        {
            get;
            set;
        }

        [Range(1, Double.MaxValue, ErrorMessage = "You must provide a valid quantity")]
        public virtual int Cantidad
        {
            get;
            set;
        }


        [ForeignKey("CompraID")]

        public virtual Compra Compra
        {
            get;
            set;

        }
        [Display(Name = "ID de Compra")]
        public virtual int CompraID
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemCompra item &&
                   Id == item.Id &&
                   Prenda.Equals(item.Prenda) &&
                   Cantidad == item.Cantidad &&
                   PrendaID == item.PrendaID &&
                   Compra.Equals(item.Compra)&&
                   CompraID == item.CompraID;
        }
    }

}