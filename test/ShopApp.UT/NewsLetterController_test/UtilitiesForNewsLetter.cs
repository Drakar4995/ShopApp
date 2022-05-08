using ShopApp.Data;
using ShopApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UT.NewsLetterController_test
{
    public static class UtilitiesForNewsLetter
    {


        public static void InitializeDbMarcasForTests(ApplicationDbContext db)
        {
            db.Marca.AddRange(GetMarcas(0, 4));
            //db.Categoria.AddRange(GetCategorias(0, 3));
            db.SaveChanges();

        }

        public static void InitializeDbCategoriasForTests(ApplicationDbContext db)
        {
            db.Categoria.AddRange(GetCategorias(0, 4));
            //db.Categoria.AddRange(GetCategorias(0, 3));
            db.SaveChanges();

        }

        public static void ReInitializeDbMarcasForTests(ApplicationDbContext db)
        {
            db.Marca.RemoveRange(db.Marca);
            //db.Categoria.RemoveRange(db.Categoria);
            db.SaveChanges();
        }

        public static void ReInitializeDbCategoriasForTests(ApplicationDbContext db)
        {
            db.Categoria.RemoveRange(db.Categoria);
            //db.Categoria.RemoveRange(db.Categoria);
            db.SaveChanges();
        }

        public static void InitializeDbNewsLetterForTests(ApplicationDbContext db)
        {
            //db.Categoria.AddRange(GetCategorias(0, 4));
            db.NewsLetter.AddRange(GetNewsLetter(0, 4));//(GetNewsLetter(0, 4))
                                                        //genre id=1 it is already added because it is related to the movies
                                                        //db.Marca.AddRange(GetMarcas(0, 4));
                                                        // db.Categoria.AddRange(GetCategorias(2, 2));
            db.SaveChanges();

            db.Users.Add(new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Name = "Peter", FirstSurname = "Jackson", SecondSurname = "García" });
            db.SaveChanges();
        }

        public static void ReInitializeDbNewsLetterForTests(ApplicationDbContext db)
        {
            db.NewsLetter.RemoveRange(db.NewsLetter);
            db.Marca.RemoveRange(db.Marca);
            db.Categoria.RemoveRange(db.Categoria);
            db.SaveChanges();
        }

        public static IList<NewsLetter> GetNewsLetter(int index, int numOfNewsLetter)
        {
            Marca marca = GetMarcas(0, 1).First();
            Marca marca2 = GetMarcas(1, 1).First();
            Categoria categoria = GetCategorias(0, 1).First();
            Categoria categoria2 = GetCategorias(1, 1).First();
            var allNewsLetter = new List<NewsLetter>
            {
                new NewsLetter { Id = 1, Titulo = "Adidas NewsLetter",Descripcion = "FFF",Marca=marca  , Categoria = categoria},
                new NewsLetter { Id = 2, Titulo = "Nike NewsLetter",  Descripcion = "kavsf",Marca=marca, Categoria = categoria},
                new NewsLetter { Id = 3, Titulo = "Puma NewsLetter",  Descripcion = "ajgaj", Marca=marca, Categoria = categoria},
                new NewsLetter { Id = 4, Titulo = "Joma NewsLetter" , Descripcion = "shdv",Marca = marca2,Categoria= categoria2}

            };

            return allNewsLetter.GetRange(index, numOfNewsLetter);
        }

        public static IList<Marca> GetMarcas(int index, int numOfMarcas)
        {
            var allMarcas = new List<Marca>
                {
                    new Marca { MarcaID = 1,Nombre = "Adidas" } ,
                    new Marca { MarcaID = 2,Nombre = "Puma" },
                    new Marca { MarcaID = 3,Nombre = "Nike" },
                    new Marca { MarcaID = 4,Nombre = "Joma" }
                };
            //return from the list as much instances as specified in numOfMarcas
            return allMarcas.GetRange(index, numOfMarcas);
        }

        public static IList<Categoria> GetCategorias(int index, int numOfCategorias)
        {
            var allCategorias = new List<Categoria>
                {
                    new Categoria { CategoriaID = 1, Nombre = "Sport" } ,
                    new Categoria { CategoriaID = 2, Nombre = "Urban" },
                    new Categoria { CategoriaID = 3, Nombre = "Kids" },
                    new Categoria { CategoriaID = 4, Nombre = "Originals" }
                };
            //return from the list as much instances as specified in numOfCategorias
            return allCategorias.GetRange(index, numOfCategorias);
        }



    }
}
