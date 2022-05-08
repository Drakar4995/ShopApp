
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Models;
using ShopApp.Data;
using ShopApp.UT.ItemCompraController_test;
namespace ShopApp.UT.DevolucionesController_test
{   
    public static class UtilitiesForDevoluciones
    {
        public static void InitializeDbDevolucionPostForTests(ApplicationDbContext db)
        {
            var devoluciones = GetDevolucion(0,2);
            foreach (Devolucion purchase in devoluciones)
            {
                db.Devolucion.Add(purchase as Devolucion);
               
            }
            db.SaveChanges();

        }
        public static void InitializeDbDevolucionDetailsForTests(ApplicationDbContext db)
        {
            var devoluciones = GetDevolucionDetails(0, 1);
            foreach (Devolucion purchase in devoluciones)
            {
                db.Devolucion.Add(purchase as Devolucion);
                
            }
            db.SaveChanges();

        }
        public static void ReInitializeDbPurchasesForTests(ApplicationDbContext db)
        {
            db.ItemDevolucion.RemoveRange(db.ItemDevolucion);
            db.Devolucion.RemoveRange(db.Devolucion);
            db.SaveChanges();
        }
        public static IList<Devolucion> GetDevolucion(int index, int numOfDevolucion)
        {

            Cliente customer = Utilities.GetUsers(0 , 1).First() as Cliente; 
            
            var allPurchases = new List<Devolucion>();
            Devolucion devolucion; //Incidencia
            ItemCompra itemCompra; //MotoAgua
            ItemDevolucion itemDevolucion; //IncidenciaItem

            for (int i = 1; i < 3; i++)
            {
                itemCompra = UtilitiesForDevoluciones.GetItemsCompra(i - 1, 1).First();

                devolucion = new Devolucion
                {
                    ID = i,
                    cliente = customer,
                    clienteId = customer.Id,
                    MetodoPago = GetPaymentMethod(i - 1, 1).First(),
                    FechaDevolucion = System.DateTime.Now,
                    precioTotal = itemCompra.Prenda.PrecioPrenda,
                    ItemsDevolucion = new List<ItemDevolucion>(),
                    

                };
                itemDevolucion = new ItemDevolucion
                {
                    ID = i,
                    ItemCompra = itemCompra,
                    ItemCompraID = itemCompra.Id,
                    MotivoDevolucion = "Talla Erronea",
                    Devolucion = devolucion,
                    DevolucionID = devolucion.ID,
                    
                };
                devolucion.ItemsDevolucion.Add(itemDevolucion);
                devolucion.precioTotal = itemCompra.Cantidad * itemCompra.Prenda.PrecioPrenda;
                allPurchases.Add(devolucion);
                if (i == 1)
                {
                    devolucion.tipoRecogida = "Correos";
                }
                else
                {
                    devolucion.tipoRecogida = "Domicilio";
                    devolucion.Direccion = "Calle 1";
                }
            }

            return allPurchases.GetRange(index, numOfDevolucion);
        }

        public static IList<Devolucion> GetDevolucionDetails(int index, int numOfDevolucion)
        {

            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente; ;
            var allPurchases = new List<Devolucion>();
            Devolucion devolucion;
            ItemCompra itemCompra;
            ItemDevolucion itemDevolucion;
          
            for (int i = 1; i < 2; i++)
            {
                itemCompra = UtilitiesForDevoluciones.GetItemsCompra(i - 1, 1).First();

                devolucion = new Devolucion
                {
                    ID = i+1,
                    cliente = customer,
                    clienteId = customer.Id,
                    Direccion = "Avd. España s/n",
                    MetodoPago = GetPaymentMethod(0 , 1).First(),
                    FechaDevolucion = System.DateTime.Now,
                    precioTotal = itemCompra.Prenda.PrecioPrenda,
                    ItemsDevolucion = new List<ItemDevolucion>(),
                    tipoRecogida = "Correos"
                };
                itemDevolucion = new ItemDevolucion
                {
                    ID = i+1,
                    ItemCompra = itemCompra,
                    ItemCompraID = itemCompra.Id,
                    MotivoDevolucion = "Talla Erronea",
                    Devolucion = devolucion,
                    DevolucionID = devolucion.ID,
                };
                devolucion.ItemsDevolucion.Add(itemDevolucion);
                devolucion.precioTotal = itemCompra.Cantidad * itemCompra.Prenda.PrecioPrenda;
                allPurchases.Add(devolucion);

            }

            return allPurchases.GetRange(index, numOfDevolucion);
        }
        public static IList<MetodoPago> GetPaymentMethod(int index, int numOfPaymentMethods)
        {
            Cliente customer = Utilities.GetUsers(0,1).First() as Cliente;
            var allPaymentMethods = new List<MetodoPago>
                {
                new PayPal { ID = 2, Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" },
                new TarjetaBancaria {ID = 2, CreditCardNumber = "1111111111111111", CCV = "111", ExpirationDate = new DateTime(2020, 10, 10) },
                new PayPal { ID = 3, Email = "peter@uclm.com", Prefix = "34", Phone = "123456789" },
                
                };
            return allPaymentMethods.GetRange(index, numOfPaymentMethods);
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
