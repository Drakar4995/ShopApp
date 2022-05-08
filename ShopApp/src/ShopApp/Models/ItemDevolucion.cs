using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class ItemDevolucion
    {

        public int ID
        {
            get;
            set;
        }
        [ForeignKey("ItemCompraID")]
        public virtual ItemCompra ItemCompra
        {
            get;
            set;
        }
        public virtual string MotivoDevolucion
        {
            get;
            set;
        }
        public virtual int ItemCompraID
        {
            get;
            set;
        }

        [ForeignKey("DevolucionID")]
        public virtual Devolucion Devolucion
        {
            get;
            set;
        }
        public virtual int DevolucionID
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            return obj is ItemDevolucion item &&
                   ID == item.ID &&
                   Devolucion.Equals(item.Devolucion) &&
                   DevolucionID == item.DevolucionID &&
                   ItemCompraID == item.ItemCompraID &&
                   ItemCompra.Equals(item.ItemCompra) &&
                   MotivoDevolucion == item.MotivoDevolucion;
        }

    }
}
