using ShopApp.Data;
using ShopApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UT.PrendasController_test
{
    class UtilitiesForPrendas
    {
        public static void InitializeDbMarcasForTests(ApplicationDbContext db)
        {
            db.Marca.AddRange(GetMarcas(0, 4));
            db.SaveChanges();

        }

        public static void ReInitializeDbMarcasForTests(ApplicationDbContext db)
        {
            db.Marca.RemoveRange(db.Marca);
            db.SaveChanges();
        }

        public static void InitializeDbPrendasForTests(ApplicationDbContext db)
        {
           // db.Marca.AddRange(GetMarcas(0, 4));
            db.Prenda.AddRange(GetPrendas(0, 4));
            

            db.Compra.AddRange(GetCompras(0, 4));
            db.ItemCompra.AddRange(GetItemsCompra(0, 9));
            //genre id=1 it is already added because it is related to the movies
            //db.Marca.AddRange(GetMarcas(2, 2));
            db.SaveChanges();

            db.Users.Add(new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Name = "Peter", FirstSurname = "Jackson", SecondSurname = "García" });
            db.Users.Add(new Gestor { Id = "4", UserName = "joseAngel@uclm.com", PhoneNumber = "967959593", Email = "joseAngel@uclm.com", Name = "JoseAngel", FirstSurname = "Jackson", SecondSurname = "García" });
            db.SaveChanges();
        }

        public static void ReInitializeDbPrendasForTests(ApplicationDbContext db)
        {
            db.Prenda.RemoveRange(db.Prenda);
            db.Marca.RemoveRange(db.Marca);
            db.SaveChanges();
        }

        public static IList<Prenda> GetPrendas(int index, int numOfPrendas)
        {
            Marca marca1 = GetMarcas(0, 1).First();
            Marca marca2 = GetMarcas(1, 1).First();

            var allPrendas = new List<Prenda>
            {
                new Prenda { PrendaID = 1, Nombre = "Camiseta", PrecioPrenda = 20 , FechaLanzamiento = new DateTime(2018, 10, 20), CantidadCompra = 10, Marca =marca1 , isRetired = false},
                new Prenda { PrendaID = 2, Nombre = "Camisa",  PrecioPrenda = 40 , FechaLanzamiento = new DateTime(2017, 02, 23), CantidadCompra = 40, Marca = marca1, isRetired = false},
                new Prenda { PrendaID = 3, Nombre = "Pantalon", PrecioPrenda = 60, FechaLanzamiento = new DateTime(2019, 04, 04), CantidadCompra = 20, Marca = marca2, isRetired = false},
                new Prenda { PrendaID = 4, Nombre = "Sudadera", PrecioPrenda = 100 , FechaLanzamiento = new DateTime(2020, 01, 01), CantidadCompra = 1, Marca = marca1, isRetired = false},           
                              
            };

            return allPrendas.GetRange(index, numOfPrendas);
        }

        public static IList<Marca> GetMarcas(int index, int numOfMarcas)
        {
            var allMarcas = new List<Marca>
                {
                    new Marca { MarcaID = 1, Nombre = "Adidas" } ,
                    new Marca { MarcaID = 2, Nombre = "Nike" },
                    new Marca { MarcaID = 3, Nombre = "Puma" },
                    new Marca { MarcaID = 4, Nombre = "Joma" }
                };
            //return from the list as much instances as specified in numOfMarcas
            return allMarcas.GetRange(index, numOfMarcas);
        }
        
        public static IList<Compra> GetCompras(int index, int numOfCompras)
        {
            
            var allCompras = new List<Compra>
                {
                    new Compra { Id=1, PrecioTotal=180, FechaCompra=DateTime.Today.AddDays(-1),DireccionEnvio="Calle Madrid",ClienteId=null,MetodoPago=null },
                    new Compra { Id=2, PrecioTotal=120, FechaCompra=DateTime.Today.AddDays(-1),DireccionEnvio="Plza Espana",ClienteId=null,MetodoPago=null },
                    new Compra { Id=3, PrecioTotal=160, FechaCompra=DateTime.Today.AddDays(-1),DireccionEnvio="Calle Tinte",ClienteId=null,MetodoPago=null },
                    new Compra { Id=4, PrecioTotal=60, FechaCompra=DateTime.Today.AddDays(-1),DireccionEnvio="Calle Madrid",ClienteId=null,MetodoPago=null }

                };
            
            return allCompras.GetRange(index, numOfCompras);
        }
       
        public static IList<ItemCompra> GetItemsCompra(int index, int numOfItemsCompra)
        {
            Prenda prenda1 = GetPrendas(0, 1).First();
            Prenda prenda2 = GetPrendas(1, 1).First();
            Prenda prenda3 = GetPrendas(3, 1).First();
            Prenda prenda4 = GetPrendas(2, 1).First();

            var allItemsCompra = new List<ItemCompra>
                {
                    new ItemCompra { Id=1, PrendaID=prenda1.PrendaID, Cantidad=60, CompraID=GetCompras(0,1).First().Id},
                    new ItemCompra { Id=2, PrendaID=prenda2.PrendaID, Cantidad=60, CompraID=GetCompras(0,1).First().Id},
                    new ItemCompra { Id=3, PrendaID=prenda3.PrendaID, Cantidad=1, CompraID=GetCompras(0,1).First().Id},
                    new ItemCompra { Id=4, PrendaID=prenda1.PrendaID, Cantidad=1, CompraID=GetCompras(1,1).First().Id},
                    new ItemCompra { Id=5, PrendaID=prenda3.PrendaID, Cantidad=1, CompraID=GetCompras(1,1).First().Id},
                    new ItemCompra { Id=6, PrendaID=prenda1.PrendaID, Cantidad=3, CompraID=GetCompras(2,1).First().Id},
                    new ItemCompra { Id=7, PrendaID=prenda3.PrendaID, Cantidad=1, CompraID=GetCompras(2,1).First().Id},
                    new ItemCompra { Id=8, PrendaID=prenda2.PrendaID, Cantidad=1, CompraID=GetCompras(3,1).First().Id},
                    new ItemCompra { Id=9, PrendaID=prenda4.PrendaID, Cantidad=1, CompraID=GetCompras(3,1).First().Id}

                };

            return allItemsCompra.GetRange(index, numOfItemsCompra);
        }
    }
}
