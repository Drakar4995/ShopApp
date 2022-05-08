using ShopApp.Data;
using ShopApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UT.ItemCompraController_test
{
    class UtilitiesForItemCompra
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

        public static void InitializeDbItemCompraForTests(ApplicationDbContext db)
        {
            db.ItemCompra.AddRange(GetItemsCompra(0, 4));
            
            db.ItemDevolucion.AddRange(GetItemDevolucion(0, 1));
            //db.SaveChanges();
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
            Marca marca3 = GetMarcas(2, 1).First();
            Marca marca4 = GetMarcas(3, 1).First();
            var allPrendas = new List<Prenda>
            {
                new Prenda { PrendaID=1 ,Nombre = "Camiseta", PrecioPrenda = 20 , FechaLanzamiento = new DateTime(2018, 10, 20), CantidadCompra = 10, Marca =marca1 , isRetired = false},
                new Prenda { PrendaID=2,Nombre = "Camisa",  PrecioPrenda = 40 , FechaLanzamiento = new DateTime(2017, 02, 23), CantidadCompra = 40, Marca = marca2, isRetired = false},
                new Prenda { PrendaID=3,Nombre = "Pantalon", PrecioPrenda = 60, FechaLanzamiento = new DateTime(2019, 04, 04), CantidadCompra = 20, Marca = marca3, isRetired = false},
                new Prenda { PrendaID=4,Nombre = "Sudadera", PrecioPrenda = 100 , FechaLanzamiento = new DateTime(2020, 01, 01), CantidadCompra = 1, Marca = marca4, isRetired = false},
                                
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
            MetodoPago m1 = new PayPal {ID=1, Email = "peter@uclm.com", Prefix = "34", Phone = "123456789" };
            //MetodoPago m2 = new PayPal { ID = 2, Email = "peter@uclm.com", Prefix = "34", Phone = "123456789" };
            //MetodoPago m3 = new TarjetaBancaria { ID = 1, CreditCardNumber = "1111111111111111", CCV = "342", ExpirationDate = new DateTime(2023,12,20) };
            Cliente cliente1 = Utilities.GetUsers(0, 1).First() as Cliente; 
            //Cliente cliente1 = new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Name = "Peter", FirstSurname = "Jackson", SecondSurname = "García" };
            var allCompras = new List<Compra>
                {
                    new Compra { Id=1,PrecioTotal=180, FechaCompra=new DateTime(2021,11,8),DireccionEnvio="Calle Madrid",ClienteId=cliente1.Id ,MetodoPago=m1,Cliente =cliente1},
                    new Compra { Id=2,PrecioTotal=180, FechaCompra=new DateTime(2021,11,8),DireccionEnvio="Calle Madrid",ClienteId=cliente1.Id ,MetodoPago=m1,Cliente =cliente1},


                };

            return allCompras.GetRange(index, numOfCompras);
        }
        
        public static IList<ItemCompra> GetItemsCompra(int index, int numOfItemsCompra)
        {
            Prenda prenda1 = GetPrendas(0, 1).First();
            Prenda prenda2 = GetPrendas(1, 1).First();
            Prenda prenda3 = GetPrendas(2, 1).First();
            Prenda prenda4 = GetPrendas(3, 1).First();

            Compra compra = GetCompras(0, 1).First();
            Compra compra1 = GetCompras(1, 1).First();
            //Compra compra2 = GetCompras(1, 1).First();
            //Compra compra3 = GetCompras(2, 1).First();

            //Compra c1 = new Compra { Id = 1, PrecioTotal = 180, FechaCompra = new DateTime(2021, 11, 8), DireccionEnvio = "Calle Madrid", ClienteId = "1", MetodoPago = m1 }
            var allItemsCompra = new List<ItemCompra>
                {                    
                    new ItemCompra {Id=1, Prenda = prenda1 , PrendaID=prenda1.PrendaID , Cantidad=2, Compra=compra, CompraID=compra.Id},
                    new ItemCompra {Id=2,Prenda = prenda2,   PrendaID=prenda2.PrendaID,  Cantidad=3, Compra=compra, CompraID=compra.Id},
                    new ItemCompra {Id=3,Prenda = prenda3,   PrendaID=prenda3.PrendaID , Cantidad=5, Compra=compra, CompraID=compra.Id},
                    new ItemCompra {Id=4,Prenda = prenda4,   PrendaID=prenda4.PrendaID , Cantidad=60, Compra=compra, CompraID=compra.Id},
                    /*
                    new ItemCompra {Id=5,Prenda = prenda4,   PrendaID=prenda4.PrendaID , Cantidad=60, Compra=compra1, CompraID=compra1.Id},
                    new ItemCompra {Id=6,Prenda = prenda4,   PrendaID=prenda4.PrendaID , Cantidad=60, Compra=compra1, CompraID=compra1.Id},
                */  
                    };

            //ItemDevolucion itemdevolucion = new ItemDevolucion { ID = 1, ItemCompra = allItemsCompra.GetRange(2, 1).First(), ItemCompraID = allItemsCompra.GetRange(2, 1).First().Id };
            
            return allItemsCompra.GetRange(index, numOfItemsCompra);
        }
        /*
        public static IList<Devolucion> GetDevolucion(int index, int numofDevoluciones)
        {
            Cliente cliente = GetCompras(0, 1).First().Cliente;
            IList <ItemDevolucion> itemdevolucion= GetItemDevolucion(0, 1);
            //ItemDevolucion itemDevolucion = new ItemDevolucion { ItemCompra = GetItemsCompra(2, 1).First(),ItemCompraID = GetItemsCompra(2, 1).First().Id, };
            var alldevoluciones = new List<Devolucion>
            { new Devolucion{ cliente = cliente, clienteId = cliente.Id , ItemsDevolucion = itemdevolucion , ID = 1 },
            };
            return alldevoluciones.GetRange(index,numofDevoluciones);
        }*/
        
        public static IList<ItemDevolucion> GetItemDevolucion(int index, int numofDevoluciones)
        {
            ItemCompra itemCompra = GetItemsCompra(3, 1).First() ;
            var alldevoluciones = new List<ItemDevolucion>
            { new ItemDevolucion{ID = 1 ,ItemCompraID = itemCompra.Id},
            };
            return alldevoluciones.GetRange(index,numofDevoluciones);
        }
        

    }

 }
