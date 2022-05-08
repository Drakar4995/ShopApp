using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ShopApp.Models.SuscripcionsViewModels
{
    public class SuscipcionCreateViewModel
    {
        [Required]
        public virtual String Titulo
        {
            get;
            set;
        }

        public IList<Marca>  Marca
        {
            get;
            set;
        }

        public IList<Prenda> Prendas
        {
            get;
            set;

        }
        [Display(Name = "Descripcion de la suscripcion")]
        [Required]
        public virtual string Descripcion
        {
            get;
            set;
        }
        [Display(Name = "Fecha de caducidad")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime FechaCaducidad
        {
            get;
            set;

        }
        public virtual string Name
        {
            get;
            set;
        }
        [Display(Name = "First Surname")]
        public virtual string FirstSurname
        {
            get;
            set;
        }

        [Display(Name = "Second Surname")]
        public virtual string SecondSurname
        {
            get;
            set;
        }
        [Display(Name = "Motivo de la suscripcion")]
        [Required]
        public virtual string Motivo
        {
            get;
            set;
        }


        //It will be necessary whenever we need a relationship with ApplicationUser or any child class
        public string ClienteId
        {
            get;
            set;
        }
       

        public virtual IList<MotivoSuscripcionViewModel> MotivoSuscripcions
        {
            get;
            set;
        }


        public SuscipcionCreateViewModel()
        {

            MotivoSuscripcions = new List<MotivoSuscripcionViewModel>();
        }

        public override bool Equals(object obj)
        {
            bool result;
            if (obj is SuscipcionCreateViewModel model)
                result = 
                  Descripcion == model.Descripcion &&
                  Motivo == model.Motivo &&
                  Name == model.Name &&
                  FirstSurname == model.FirstSurname &&
                  SecondSurname == model.SecondSurname&&
                  ClienteId == model.ClienteId &&
                  FechaCaducidad == model.FechaCaducidad;

            else
                return false;
            for (int i = 0; i < this.MotivoSuscripcions.Count; i++)
                result = result && (this.MotivoSuscripcions[i].Equals(model.MotivoSuscripcions[i]));


            return result;
        }

    }


    public class MotivoSuscripcionViewModel
    {
        public virtual string TituloNewssletter
        { get;
            set;
        }
            public virtual int NewsletterId
            {
                get;
                set;
            }
        public virtual String Categoria
            {
                get;
                set;
            }
        public virtual String   Marca
        {
            get;
            set;
        }



        public virtual int Id
        {
            get;
            set;
        }
        public virtual string Motivo
        {
            get;
            set;
        }

        public override bool Equals(object obj)
            {
                MotivoSuscripcionViewModel motivoSuscripcion = obj as MotivoSuscripcionViewModel;
                bool result = false;
                if ((NewsletterId == motivoSuscripcion.NewsletterId))
                    result = true;
            return result;
            }
        }
    }


