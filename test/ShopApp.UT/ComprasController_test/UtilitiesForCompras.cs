using ShopApp.Data;
using ShopApp.Models;
using ShopApp.UT.PrendasController_test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UT.ComprasController_test
{

    class UtilitiesForCompras
    {
        public static void InitializeDbPurchasesForTests(ApplicationDbContext db)
        {
            var purchases = GetPurchases(0, 1);
            foreach (Compra purchase in purchases)
            {
                db.Compra.Add(purchase as Compra);
            }
            db.SaveChanges();

        }
        public static void InitializeDbPrendasForTests(ApplicationDbContext db)
        {
            // db.Marca.AddRange(GetMarcas(0, 4));
            db.Prenda.AddRange(GetPrendas(0, 4));

            db.Users.Add(new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Name = "Peter", FirstSurname = "Jackson", SecondSurname = "García" });
            
            db.SaveChanges();
        }


        public static void ReInitializeDbPurchasesForTests(ApplicationDbContext db)
        {
            db.ItemCompra.RemoveRange(db.ItemCompra);
            db.Compra.RemoveRange(db.Compra);
            db.SaveChanges();
        }

        public static IList<Compra> GetPurchases(int index, int numOfPurchases)
        {

            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            var allPurchases = new List<Compra>();
            Compra purchase;
            Prenda prenda;
            ItemCompra purchaseItem;
            int quantity = 2;

            for (int i = 1; i < 3; i++)
            {
                prenda = UtilitiesForCompras.GetPrendas(i - 1, 1).First();
                prenda.CantidadCompra = prenda.CantidadCompra - quantity;
                purchase = new Compra
                {
                    Id = i,
                    Cliente = customer,
                    ClienteId = customer.Id,
                    DireccionEnvio = "Avd. España s/n",
                    MetodoPago = GetPaymentMethod(i - 1, 1).First(),
                    FechaCompra = System.DateTime.Now,
                    PrecioTotal = prenda.PrecioPrenda,
                    ItemsCompra = new List<ItemCompra>()
                };
                purchaseItem = new ItemCompra
                {
                    Id = i,
                    Cantidad = quantity,
                    Prenda = prenda,
                    PrendaID = prenda.PrendaID,
                    Compra = purchase,
                    CompraID = purchase.Id

                };
                purchase.ItemsCompra.Add(purchaseItem);
                purchase.PrecioTotal = purchaseItem.Cantidad * purchaseItem.Prenda.PrecioPrenda;
                allPurchases.Add(purchase);

            }

            return allPurchases.GetRange(index, numOfPurchases);
        }

        public static IList<MetodoPago> GetPaymentMethod(int index, int numOfPaymentMethods)
        {
            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            var allPaymentMethods = new List<MetodoPago>
                {
                new TarjetaBancaria {ID = 1, CreditCardNumber = "1111111111111111", CCV = "111", ExpirationDate = new DateTime(2020, 10, 10) },
                new PayPal { ID = 2, Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" },

            };
            //return from the list as much instances as specified in numOfGenres
            return allPaymentMethods.GetRange(index, numOfPaymentMethods);
        }

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
            db.SaveChanges();
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
            MetodoPago m1 = new PayPal { ID = 1, Email = "peter@uclm.com", Prefix = "34", Phone = "123456789" };
            MetodoPago m2 = new PayPal { ID = 2, Email = "peter@uclm.com", Prefix = "34", Phone = "123456789" };
            MetodoPago m3 = new TarjetaBancaria { ID = 1, CreditCardNumber = "1111111111111111", CCV = "342", ExpirationDate = new DateTime(2023, 12, 20) };

            Cliente cliente1 = Utilities.GetUsers(0, 1).First() as Cliente;
            Cliente cliente2 = Utilities.GetUsers(1, 1).First() as Cliente;
            Cliente cliente3 = Utilities.GetUsers(2, 1).First() as Cliente;
            var allCompras = new List<Compra>
                {
                    new Compra { Id=1,PrecioTotal=180, FechaCompra=new DateTime(2021,11,8),DireccionEnvio="Calle Madrid",ClienteId=cliente1.Id ,MetodoPago=m1,Cliente =cliente1},
                    new Compra { Id=2, PrecioTotal=120, FechaCompra=new DateTime(2021,11,8),DireccionEnvio="Plaza Espana",ClienteId=cliente2.Id,MetodoPago=m2,Cliente =cliente2},
                    new Compra { Id=3,PrecioTotal=160, FechaCompra=new DateTime(2021,11,8),DireccionEnvio="Calle Tinte",ClienteId=cliente3.Id,MetodoPago=m3,Cliente =cliente3},


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
            Compra compra2 = GetCompras(1, 1).First();
            Compra compra3 = GetCompras(2, 1).First();

            //Compra c1 = new Compra { Id = 1, PrecioTotal = 180, FechaCompra = new DateTime(2021, 11, 8), DireccionEnvio = "Calle Madrid", ClienteId = "1", MetodoPago = m1 }
            var allItemsCompra = new List<ItemCompra>
                {
                    new ItemCompra {Id=1, Prenda = prenda1 , PrendaID=prenda1.PrendaID , Cantidad=2, Compra=compra, CompraID=compra.Id},
                    new ItemCompra {Id=2,Prenda = prenda2,   PrendaID=prenda2.PrendaID,  Cantidad=3, Compra=compra2, CompraID=compra2.Id},
                    new ItemCompra {Id=3,Prenda = prenda3,   PrendaID=prenda3.PrendaID , Cantidad=5, Compra=compra3, CompraID=compra3.Id},
                    new ItemCompra {Id=4,Prenda = prenda4,   PrendaID=prenda4.PrendaID , Cantidad=60, Compra=compra, CompraID=compra.Id},

                };
            return allItemsCompra.GetRange(index, numOfItemsCompra);
        }
    }
}


