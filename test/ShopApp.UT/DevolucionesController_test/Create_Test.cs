using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopApp.Controllers;
using System.Threading.Tasks;
using Xunit;
using ShopApp.Models;
using ShopApp.Models.ItemCompraViewModels;
using ShopApp.Models.DevolucionViewModels;
using Microsoft.AspNetCore.Mvc;
using ShopApp.UT.ItemCompraController_test;

namespace ShopApp.UT.DevolucionesController_test
{
    public class Create_Test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext devolucionesContext;


        public Create_Test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

         
            UtilitiesForItemCompra.InitializeDbItemCompraForTests(context);
            


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            devolucionesContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = identity
            };

        }

        [Fact]
        public void Create_Get_WithSelectedPrendas()
        {
            using (context)
            {

                // Arrange
                var controller = new DevolucionesController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = devolucionesContext;

                String[] ids = new string[1] { "1" };
                SelectedPrendasForDevolucionViewModel selectedPrendas = new() { IdsToAdd = ids };
               
                ItemCompra expectedItemCompra = UtilitiesForItemCompra.GetItemsCompra(0,1).First();
                Cliente expectedCustomer = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<ItemDevolucionViewModel> itemDevolucionExpected = new ItemDevolucionViewModel [1] {
                    new ItemDevolucionViewModel {
                        
                        CompraId = expectedItemCompra.Compra.Id,
                        ItemCompraID = expectedItemCompra.Id,
                        nombrePrenda = expectedItemCompra.Prenda.Nombre,
                        precio = expectedItemCompra.Prenda.PrecioPrenda,
                        cantidad = expectedItemCompra.Cantidad,
                        nombreMarca = expectedItemCompra.Prenda.Marca.Nombre,
                        
                        } };
                CreateDevolucionViewModel expectedDevolucion = new() { 
                    ItemsDevolucion = itemDevolucionExpected,
                    Nombre = expectedCustomer.Name, 
                    PrimerApellido = expectedCustomer.FirstSurname, 
                    SegundoApellido = expectedCustomer.SecondSurname,
                                                      };
                
                

                // Act
                var result = controller.Create(selectedPrendas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CreateDevolucionViewModel currentDevolucion = viewResult.Model as CreateDevolucionViewModel;

                Assert.Equal(currentDevolucion, expectedDevolucion);

            }
        }
        [Fact]
        public void Create_Get_WithoutPrendas()
        {
            using (context)
            {

                // Arrange
                var controller = new DevolucionesController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = devolucionesContext;
                Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
                SelectedPrendasForDevolucionViewModel prendasforDevolucion = new();

                CreateDevolucionViewModel expectedDevoluciones = new()
                {
                    Nombre = customer.Name,
                    PrimerApellido = customer.FirstSurname,
                    SegundoApellido= customer.SecondSurname,
                    ItemsDevolucion = new List<ItemDevolucionViewModel>()
                };


                // Act
                var result = controller.Create(prendasforDevolucion);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CreateDevolucionViewModel currentDevolucion = viewResult.Model as CreateDevolucionViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentDevolucion, expectedDevoluciones);
                Assert.Equal("Tienes que Seleccionar al menos una prenda a Devolver", error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithErrors()
        {
            //Error para el tipoRecogida NULL
            //Input Values
            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;

            ItemCompra itemcompra = UtilitiesForItemCompra.GetItemsCompra(3,1).First();
            IList<ItemDevolucionViewModel> itemsDevolucionViewModel = new ItemDevolucionViewModel[1]
            { new ItemDevolucionViewModel
                {
                    ItemCompraID = itemcompra.Id,
                    nombreMarca = itemcompra.Prenda.Marca.Nombre ,
                    precio= itemcompra.Prenda.PrecioPrenda,
                    cantidad = itemcompra.Cantidad,
                    nombrePrenda = itemcompra.Prenda.Nombre,
                    CompraId = itemcompra.CompraID
                }};
            CreateDevolucionViewModel devolucion1 = new()
            {
                Nombre = customer.Name,
                PrimerApellido = customer.FirstSurname,
                SegundoApellido = customer.SecondSurname,
                ItemsDevolucion = itemsDevolucionViewModel,
                Email = customer.Email,
                Phone = customer.PhoneNumber,
                Prefix = "+34",
                tipoRecogida = null,
                PaymentMethod= "PayPal"
            };

            //Expected Values
            IList<ItemDevolucionViewModel> expectedItemDevolucionVM = new ItemDevolucionViewModel[1]
            { new ItemDevolucionViewModel
                {
                    ItemCompraID = itemcompra.Id,
                    nombreMarca = itemcompra.Prenda.Marca.Nombre ,
                    precio= itemcompra.Prenda.PrecioPrenda,
                    cantidad = itemcompra.Cantidad,
                    nombrePrenda = itemcompra.Prenda.Nombre,
                    CompraId = itemcompra.CompraID,
                    
                }};
            CreateDevolucionViewModel expectedDevolucionVM = new()
            {
                Nombre = customer.Name,
                PrimerApellido = customer.FirstSurname,
                SegundoApellido = customer.SecondSurname,
                ItemsDevolucion = expectedItemDevolucionVM,
                Email = customer.Email,
                Phone = customer.PhoneNumber,
                Prefix = "+34",
                tipoRecogida= null,
                PaymentMethod = "PayPal",
                
            };

            string expectedErrorMessage1 = "Seleccione un metodo de Recogida";
           


            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { devolucion1,  expectedDevolucionVM, expectedErrorMessage1 }
                //,
                //new object[] { purchase2,  expectedPurchaseVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithErrors))]
        public void Create_Post_WithErrors(CreateDevolucionViewModel devolucion, CreateDevolucionViewModel expectedDevolucionVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new DevolucionesController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = devolucionesContext;

                // Act
                var result = controller.CreatePost(devolucion);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                CreateDevolucionViewModel currentDevolucion = viewResult.Model as CreateDevolucionViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedDevolucionVM, currentDevolucion);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }
        

        public static IEnumerable<object[]>   TestCasesForDevolucionCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Devolucion expectedDevolucion1 = UtilitiesForDevoluciones.GetDevolucion(0, 1).First();
            Cliente expectedCustomer1 = expectedDevolucion1.cliente;
            var expectedPayment1 = expectedDevolucion1.MetodoPago as PayPal;
            ItemDevolucion expectedItemDevolucion1 = expectedDevolucion1.ItemsDevolucion.First();

            //int expectedQuantityForPurchase1 = UtilitiesForMovies.GetMovies(0, 1).First().QuantityForPurchase - expectedItemDevolucion1.Quantity;
            IList<ItemDevolucionViewModel> itemDevolucionViewModel1 = new ItemDevolucionViewModel[1] {
                new ItemDevolucionViewModel {
                    cantidad = expectedItemDevolucion1.ItemCompra.Cantidad,
                    ItemCompraID = expectedItemDevolucion1.ItemCompraID,
                    nombrePrenda = expectedItemDevolucion1.ItemCompra.Prenda.Nombre,
                    nombreMarca = expectedItemDevolucion1.ItemCompra.Prenda.Marca.Nombre,
                    precio = expectedItemDevolucion1.ItemCompra.Prenda.PrecioPrenda,
                    CompraId = expectedItemDevolucion1.ItemCompra.CompraID,
                   } };
            CreateDevolucionViewModel devolucion1 = new()
            {
                Nombre = expectedCustomer1.Name,
                PrimerApellido = expectedCustomer1.FirstSurname,
                SegundoApellido = expectedCustomer1.SecondSurname,
                ItemsDevolucion = itemDevolucionViewModel1,
                Direccion = expectedDevolucion1.Direccion,
                PaymentMethod = "PayPal",
                Email = expectedPayment1.Email,
                Prefix = expectedPayment1.Prefix,
                Phone = expectedPayment1.Phone,
                tipoRecogida = "Correos"
               
            };
            expectedDevolucion1.MetodoDevolucion = "PayPal";

            //Payment with Paypal
            Devolucion expectedDevolucion2 = UtilitiesForDevoluciones.GetDevolucion( 1, 1).First();
            expectedDevolucion2.ID = 1;
            expectedDevolucion2.ItemsDevolucion.First().ID = 1;
            expectedDevolucion2.ItemsDevolucion.First().DevolucionID = 1;
            ItemDevolucion expectedItemdevolucion2 = expectedDevolucion2.ItemsDevolucion.First();
            //int expectedQuantityForPurchase2 = UtilitiesForMovies.GetMovies(1, 1).First().QuantityForPurchase - expectedPurchaseItem2.Quantity;
            var expectedPayment2 = expectedDevolucion2.MetodoPago as TarjetaBancaria;
            //expectedPayment2.ID = 1;
            Cliente expectedCustomer2 = expectedDevolucion2.cliente;

            IList<ItemDevolucionViewModel> ItemDevolucionViewModel2 = new ItemDevolucionViewModel[1] {
                new ItemDevolucionViewModel {
                    cantidad = expectedItemdevolucion2.ItemCompra.Cantidad,
                    ItemCompraID = expectedItemdevolucion2.ItemCompraID,
                    nombrePrenda = expectedItemdevolucion2.ItemCompra.Prenda.Nombre,
                    nombreMarca = expectedItemdevolucion2.ItemCompra.Prenda.Marca.Nombre,
                    precio = expectedItemdevolucion2.ItemCompra.Prenda.PrecioPrenda,
                    CompraId = expectedItemdevolucion2.ItemCompra.CompraID,
                    
                   } };
           CreateDevolucionViewModel devolucion2 = new()
            {
               Nombre = expectedCustomer2.Name,
               PrimerApellido = expectedCustomer2.FirstSurname,
               SegundoApellido = expectedCustomer2.SecondSurname,
               ItemsDevolucion = ItemDevolucionViewModel2,
               Direccion = expectedDevolucion2.Direccion,
               PaymentMethod = "CreditCard",
               CreditCardNumber = expectedPayment2.CreditCardNumber,
               CCV = expectedPayment2.CCV,
               ExpirationDate = expectedPayment2.ExpirationDate,                
               tipoRecogida = "Domicilio",
               
                               
           };
            devolucion2.Direccion = "Calle 1";
            expectedDevolucion2.MetodoDevolucion = "CreditCard";
            var allTests = new List<object[]>
            {                  //Input values   // expected values
                new object[] { devolucion1,  expectedDevolucion1},
                new object[] { devolucion2,  expectedDevolucion2}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForDevolucionCreatePost_WithoutErrors))]
        public void Create_Post_WithoutErrors(CreateDevolucionViewModel devolucion, Devolucion expectedDevolucion)
        {
            using (context)
            {

                // Arrange
                var controller = new DevolucionesController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = devolucionesContext;

                // Act
                var result = controller.CreatePost(devolucion); //ItemCompra a null , ver utilities

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualDevolucion = context.Devolucion.Include(p => p.ItemsDevolucion).
                                    FirstOrDefault(p => p.ID == expectedDevolucion.ID);
                Assert.Equal(expectedDevolucion, actualDevolucion);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
               
                
            }

        }
        
    }
}
