using ShopApp.Controllers;
using ShopApp.Data;
using ShopApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ShopApp.Models.CompraViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Runtime.ExceptionServices;
using ShopApp.Models.PrendaViewModels;
using ShopApp.UT.PrendasController_test;
using ShopApp.UT.ComprasController_test;
using ShopApp.UT;

namespace ShopApp.UT.ComprasController_test
{
    public class Create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;


        public Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForCompras.InitializeDbPrendasForTests(context);
            


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            purchaseContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = identity
            };

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithSelectedPrendas()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;

                String[] ids = new string[1] { "1" };
                SelectedPrendasForPurchaseViewModel prendas = new() { IdsToAdd = ids };
                Prenda expectedPrenda = UtilitiesForCompras.GetPrendas(0, 1).First();
                Cliente expectedCustomer = Utilities.GetUsers(0, 1).First() as Cliente;

                IList<ItemCompraViewModel> expectedPurchaseItems = new ItemCompraViewModel[1] {
                    new ItemCompraViewModel {Cantidad=1, PrendaID = expectedPrenda.PrendaID, Nombre = expectedPrenda.Nombre,
                        PrecioPrenda = expectedPrenda.PrecioPrenda, Marca = expectedPrenda.Marca.Nombre} };
                CompraCreateViewModel expectedPurchase = new() { ItemsCompra = expectedPurchaseItems, Nombre = expectedCustomer.Name, FirstSurname = expectedCustomer.FirstSurname, SecondSurname = expectedCustomer.SecondSurname };

                // Act
                var result = controller.Create(prendas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;

                Assert.Equal(currentPurchase, expectedPurchase);

            }
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutPrenda()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;
                Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
                SelectedPrendasForPurchaseViewModel prendas = new();

                CompraCreateViewModel expectedPurchase = new()
                {
                    Nombre = customer.Name,
                    FirstSurname = customer.FirstSurname,
                    SecondSurname = customer.SecondSurname,
                    ItemsCompra = new List<ItemCompraViewModel>()
                };


                // Act
                var result = controller.Create(prendas);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentPurchase, expectedPurchase);
                Assert.Equal("You should select at least a Prenda to be purchased, please", error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            Prenda prenda = UtilitiesForCompras.GetPrendas(0, 1).First();
            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            //  var payment1 = new PayPal { Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

            //Input values
            IList<ItemCompraViewModel> purchaseItemsViewModel1 = new ItemCompraViewModel[1] {
                new ItemCompraViewModel {
                    Cantidad = 20,
                    PrendaID = prenda.PrendaID,
                    Nombre = prenda.Nombre,
                    Marca = prenda.Marca.Nombre,
                    PrecioPrenda = prenda.PrecioPrenda,
                    }
                };
            CompraCreateViewModel purchase1 = new() { 
                    Nombre = customer.Name,
                    FirstSurname = customer.FirstSurname,
                    SecondSurname = customer.SecondSurname,
                    ItemsCompra = purchaseItemsViewModel1,
                    DireccionEnvio = "Albacete",
                    Email = customer.Email,
                    Phone = customer.PhoneNumber,
                    Prefix = "+34",
                    MetodoPago = "PayPal"
                    };

            //Expected values
            IList<ItemCompraViewModel> expectedPurchaseItemsViewModel1 = new ItemCompraViewModel[1] {
                new ItemCompraViewModel { 
                    Cantidad = 20,
                    PrendaID = prenda.PrendaID,
                    Nombre = prenda.Nombre,
                    Marca = prenda.Marca.Nombre,
                    PrecioPrenda = prenda.PrecioPrenda } };
            CompraCreateViewModel expectedPurchaseVM1 = new() { Nombre = customer.Name,
                FirstSurname = customer.FirstSurname,
                SecondSurname = customer.SecondSurname,
                ItemsCompra = expectedPurchaseItemsViewModel1,
                DireccionEnvio = "Albacete",
                Email = customer.Email,
                Phone = customer.PhoneNumber,
                Prefix = "+34",
                MetodoPago = "PayPal" };
            string expetedErrorMessage1 = "No hay prendas suficientes llamadas: Camiseta, por favor selecciona una prenda menor o igual que: 10";


            //Second error: Create_Post_WithQuantity0ForPurchase

           ;

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { purchase1,  expectedPurchaseVM1, expetedErrorMessage1 }
                //,
                //new object[] { purchase2,  expectedPurchaseVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithErrors(CompraCreateViewModel purchase, CompraCreateViewModel expectedPurchaseVM, string errorMessage)
        {

            using (context)
            {
                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;

                // Act
                var result = controller.CreatePost(purchase);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                //currentPurchase.ItemsCompra[0].Cantidad = 10;

                Assert.Equal(expectedPurchaseVM, currentPurchase);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }

        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Compra expectedPurchase1 = UtilitiesForCompras.GetPurchases(0, 1).First();
            Cliente expectedCustomer1 = expectedPurchase1.Cliente;
            var expectedPayment1 = expectedPurchase1.MetodoPago as TarjetaBancaria;
            ItemCompra expectedPurchaseItem1 = expectedPurchase1.ItemsCompra.First();
            
            IList<ItemCompraViewModel> purchaseItemsViewModel1 = new ItemCompraViewModel[1] { new ItemCompraViewModel {
                    Cantidad = expectedPurchaseItem1.Cantidad, PrendaID = expectedPurchaseItem1.PrendaID,
                    Nombre=expectedPurchaseItem1.Prenda.Nombre, Marca=expectedPurchaseItem1.Prenda.Marca.Nombre,
                    PrecioPrenda=expectedPurchaseItem1.Prenda.PrecioPrenda} };
            CompraCreateViewModel purchase1 = new()
            {
                Nombre = expectedCustomer1.Name,
                FirstSurname = expectedCustomer1.FirstSurname,
                SecondSurname = expectedCustomer1.SecondSurname,
                ItemsCompra = purchaseItemsViewModel1,
                DireccionEnvio = expectedPurchase1.DireccionEnvio,
                MetodoPago = "Tarjeta Bancaria",
                CreditCardNumber = expectedPayment1.CreditCardNumber,
                CCV = expectedPayment1.CCV,
                ExpirationDate = expectedPayment1.ExpirationDate

            };
            int expectedQuantityForPurchase1 = UtilitiesForCompras.GetPrendas(0, 1).First().CantidadCompra - expectedPurchaseItem1.Cantidad;
            //Payment with Paypal
            Compra expectedPurchase2 = UtilitiesForCompras.GetPurchases(1, 1).First();
            expectedPurchase2.Id = 1;
            expectedPurchase2.ItemsCompra.First().Id = 1;
            expectedPurchase2.ItemsCompra.First().Id = 1;
            ItemCompra expectedPurchaseItem2 = expectedPurchase2.ItemsCompra.First();
            int expectedQuantityForPurchase2 = UtilitiesForCompras.GetPrendas(1, 1).First().CantidadCompra - expectedPurchaseItem2.Cantidad;
            var expectedPayment2 = expectedPurchase2.MetodoPago as PayPal;
            expectedPayment2.ID = 1;
            Cliente expectedCustomer2 = expectedPurchase2.Cliente;

            IList<ItemCompraViewModel> purchaseItemsViewModel2 = new ItemCompraViewModel[1] { new ItemCompraViewModel {
                    Cantidad = expectedPurchaseItem2.Cantidad, PrendaID = expectedPurchaseItem2.PrendaID,
                    Nombre=expectedPurchaseItem2.Prenda.Nombre, Marca=expectedPurchaseItem2.Prenda.Marca.Nombre,
                    PrecioPrenda=expectedPurchaseItem2.Prenda.PrecioPrenda} };
            CompraCreateViewModel purchase2 = new()
            {
                Nombre = expectedCustomer2.Name,
                FirstSurname = expectedCustomer2.FirstSurname,
                SecondSurname = expectedCustomer2.SecondSurname,
                ItemsCompra = purchaseItemsViewModel2,
                DireccionEnvio = expectedPurchase2.DireccionEnvio,
                MetodoPago = "PayPal",
                Phone = expectedPayment2.Phone,
                Prefix = expectedPayment2.Prefix,
                Email = expectedPayment2.Email
            };

            var allTests = new List<object[]>
            {                  //Input values   // expected values
                new object[] { purchase1,  expectedPurchase1, expectedQuantityForPurchase1},
                new object[] { purchase2,  expectedPurchase2, expectedQuantityForPurchase2}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(CompraCreateViewModel purchase, Compra expectedPurchase, int expectedQuantityForPurchase)
        {
            
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = purchaseContext;

                // Act
                var result = controller.CreatePost(purchase);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualPurchase = context.Compra.Include(p => p.ItemsCompra).
                                    FirstOrDefault(p => p.Id == expectedPurchase.Id);
                Assert.Equal(expectedPurchase, actualPurchase);

                //And that the quantity for purchase of each associated prenda has been modified accordingly 
                Assert.Equal(expectedQuantityForPurchase,
                   expectedPurchase.ItemsCompra.First().Prenda.CantidadCompra);


            }

        }

    }
}
