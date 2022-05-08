using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ShopApp.Models.RetiradaViewModels
{
    public class RetiradaCreateViewModel
    {

        [Display(Name = "Titulo")]
        public String Titulo
        {
            get;
            set;
        }
        
        public String GestorId
        {

            get;
            set;
        }

        [Display(Name = "Fecha Efectiva")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Introduce una fecha")]
        public virtual DateTime FechaEfectiva
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripcion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, escribe una descripcion")]
        public String Descripcion
        {
            get;
            set;
        }

        public virtual IList<MotivoRetiradaViewModel> MotivosRetirada
        {

            get;
            set;
        }

        public RetiradaCreateViewModel()
        {
            MotivosRetirada = new List<MotivoRetiradaViewModel>();
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj is RetiradaCreateViewModel model)
                result = Titulo == model.Titulo &&
                    GestorId == model.GestorId &&
                    FechaEfectiva == model.FechaEfectiva &&
                    Descripcion == model.Descripcion;
            else
                return false;
            for (int i = 0; i < this.MotivosRetirada.Count; i++)
                result = result && (this.MotivosRetirada[i].Equals(model.MotivosRetirada[i]));

            return result;
        }

    }

    public class MotivoRetiradaViewModel
    {
        public virtual int PrendaID
        {
            get;
            set;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Escribe un motivo")]
        public virtual String Descripcion
        {
            get;
            set;
        }
        public virtual String Nombre
        {
            get;
            set;
        }
        
        public virtual String Marca
        {
            get;
            set;
        }

        [DataType(DataType.Currency)]
        public virtual int Precio
        {

            get;
            set;
        }

        public virtual int VentasSemanales
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            MotivoRetiradaViewModel motivoRetirada = obj as MotivoRetiradaViewModel;
            bool result = false;
            if ((PrendaID == motivoRetirada.PrendaID)
                && (Precio == motivoRetirada.Precio)
                && (Descripcion == motivoRetirada.Descripcion)
                && (Nombre == motivoRetirada.Nombre)
                && (Marca == motivoRetirada.Marca)
                && (VentasSemanales == motivoRetirada.VentasSemanales))
                result = true;
            return result;
        }
    }
}
