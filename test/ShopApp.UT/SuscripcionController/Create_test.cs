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
using ShopApp.Models.SuscripcionsViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Runtime.ExceptionServices;
using ShopApp.Models.NewsletterViewModels;
using ShopApp.UT.NewsLetterController_test;
using ShopApp.UT.SuscripcionController;

namespace ShopApp.UT.SuscripcionController
{
    public class Create_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext suscripcioncontext;


        public Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForNewsLetter.InitializeDbNewsLetterForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            suscripcioncontext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = identity
            };

        }
        
                [Fact]
                public void Create_Get_WithSelectedNewsletter()
                {
                    using (context)
                    {

                        // Arrange
                        var controller = new SuscripcionsController(context);
                        //simulate user's connection
                        controller.ControllerContext.HttpContext = suscripcioncontext;

                        //String[] ids = new string[1] { "1" };
                        int[] ids = new int[1] { 1 };
                        SelectedNewsletterForSuscribeViewModel newsletters = new() { IdsToAdd = ids };
                        NewsLetter expectedNewsletter = UtilitiesForNewsLetter.GetNewsLetter(0, 1).First();
                        Cliente expectedCustomer = Utilities.GetUsers(0, 1).First() as Cliente;

                        IList<MotivoSuscripcionViewModel> expectedMotivosuscripcions = new MotivoSuscripcionViewModel[1] {
                            new MotivoSuscripcionViewModel { NewsletterId = expectedNewsletter.Id, TituloNewssletter = expectedNewsletter.Titulo, 
                                Marca = expectedNewsletter.Marca.Nombre, Categoria = expectedNewsletter.Categoria.Nombre} };
                        SuscipcionCreateViewModel expectedSuscripcion = new() { MotivoSuscripcions = expectedMotivosuscripcions, Name = expectedCustomer.Name, FirstSurname = expectedCustomer.FirstSurname, SecondSurname = expectedCustomer.SecondSurname };

                        // Act
                        var result = controller.Create(newsletters);

                        //Assert
                        ViewResult viewResult = Assert.IsType<ViewResult>(result);

                        SuscipcionCreateViewModel currentSuscripciion = viewResult.Model as SuscipcionCreateViewModel;

                        Assert.Equal(currentSuscripciion, expectedSuscripcion);

                    }
                }
        /*[Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithSelectedNewsletter()
        {
            using (context)
            {

                // Arrange
                var controller = new SuscripcionsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = suscripcioncontext;

                int[] ids = new int[1] { 1 };
                SelectedNewsletterForSuscribeViewModel prendas = new() { IdsToAdd = ids };
                NewsLetter expectedPrenda = UtilitiesForNewsLetter.GetNewsLetter(0, 1).First();
                Cliente expectedGestor = Utilities.GetUsers(1, 1).First() as Cliente;
           
                IList<MotivoSuscripcionViewModel> expectedMotivoRetirada = new MotivoSuscripcionViewModel[1] {
                    new MotivoSuscripcionViewModel {TituloNewssletter=expectedPrenda.Titulo,Marca=expectedPrenda.Marca.Nombre} };
                SuscipcionCreateViewModel expectedRetirada = new()
                {
                    MotivoSuscripcions = expectedMotivoRetirada,
                    ClienteId = expectedGestor.Id,
                    FechaCaducidad = new DateTime()
                };

                // Act
                var result = controller.Create(prendas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                SuscipcionCreateViewModel currentRetirada = viewResult.Model as SuscipcionCreateViewModel;

                Assert.Equal(currentRetirada, expectedRetirada);

            }
        }*/
        [Fact]
        public void Create_Get_WithoutMovie()
        {
            using (context)
            {

                // Arrange
                var controller = new SuscripcionsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = suscripcioncontext;
                Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
                SelectedNewsletterForSuscribeViewModel newsletters = new();
                /*SuscipcionCreateViewModel expectedSuscripcion = new()
                {
                    ClienteId = gestor.Id,
                    MotivoSuscripcions = new List<MotivoSuscripcionViewModel>(),
                    Descripcion = null,
                    FechaCaducidad = new DateTime()
                };*/


                // Act
                var result = controller.Create(newsletters);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                SuscipcionCreateViewModel expectedSuscripcion = viewResult.Model as SuscipcionCreateViewModel;
                SuscipcionCreateViewModel currentPurchase = viewResult.Model as SuscipcionCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentPurchase, expectedSuscripcion);
                Assert.Equal("You should select at least a Newsletter to be suscribe, please", error.ErrorMessage);
            }
        }
        /*[Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutPrenda()
        {
            using (context)
            {

                // Arrange
                var controller = new SuscripcionsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = suscripcioncontext;
                Cliente gestor = Utilities.GetUsers(1, 1).First() as Cliente;
                SelectedNewsletterForSuscribeViewModel prendas = new();

                SuscipcionCreateViewModel expectedSuscripcion = new()
                {
                    ClienteId = gestor.Id,
                    Titulo = "Retirada" + (context.Suscripcion.Count() + 1).ToString(),
                    MotivoSuscripcions = new List<MotivoSuscripcionViewModel>(),
                    Descripcion = null,
                    FechaCaducidad = new DateTime()
                };


                // Act
                var result = controller.Create(prendas);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                SuscipcionCreateViewModel currentRetirada = viewResult.Model as SuscipcionCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentRetirada, expectedSuscripcion);
                Assert.Equal("Tienes que seleccionar una prenda", error.ErrorMessage);
            }
        }*/

        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased

            NewsLetter newsletter = UtilitiesForNewsLetter.GetNewsLetter(0, 1).First();
            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            var allmarcas = UtilitiesForNewsLetter.GetMarcas(0, 2) as IList<Marca>;
            //  var payment1 = new PayPal { Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

            //Input values
            IList<MotivoSuscripcionViewModel> motivosuscripcion1 = new MotivoSuscripcionViewModel[1] { new MotivoSuscripcionViewModel
            {NewsletterId = newsletter.Id, TituloNewssletter = newsletter.Titulo, Marca = newsletter.Marca.Nombre,
                Categoria = newsletter.Categoria.Nombre,/*Motivo = newsletter.MotivoSuscripcions*/ } };
            SuscipcionCreateViewModel suscripcion1 = new() { Name = customer.Name, FirstSurname = customer.FirstSurname, SecondSurname = customer.SecondSurname };

            //Expected values
            IList<MotivoSuscripcionViewModel> expectedmotivosuscripcion1 = new MotivoSuscripcionViewModel[1] { new MotivoSuscripcionViewModel
            {NewsletterId = newsletter.Id, TituloNewssletter = newsletter.Titulo, Marca = newsletter.Marca.Nombre,
                Categoria = newsletter.Categoria.Nombre,/*Motivo = newsletter.MotivoSuscripcions*/ } };
            SuscipcionCreateViewModel expectedsuscripcionVM1 = new() { Marca = allmarcas,Name = customer.Name, FirstSurname = customer.FirstSurname, SecondSurname = customer.SecondSurname };
            string expetedErrorMessage1 = "Fecha no valida";


            //Second error: Create_Post_WithQuantity0ForPurchase

            ////Input values
            //IList<PurchaseItemViewModel> purchaseItemsViewModel2 = new PurchaseItemViewModel[1] { new PurchaseItemViewModel { Quantity = 0, MovieID = movie.MovieID, Title = movie.Title, Genre = movie.Genre.Name, PriceForPurchase = movie.PriceForPurchase } };
            //PurchaseCreateViewModel purchase2 = new() { Name = customer.Name, FirstSurname = customer.FirstSurname, SecondSurname = customer.SecondSurname, PurchaseItems = purchaseItemsViewModel2, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

            ////expected values
            //IList<PurchaseItemViewModel> expectedPurchaseItemsViewModel2 = new PurchaseItemViewModel[1] { new PurchaseItemViewModel { Quantity = 0, MovieID = movie.MovieID, Title = movie.Title, Genre = movie.Genre.Name, PriceForPurchase = movie.PriceForPurchase } };
            //PurchaseCreateViewModel expectedPurchaseVM2 = new() { Name = customer.Name, FirstSurname = customer.FirstSurname, SecondSurname = customer.SecondSurname, PurchaseItems = expectedPurchaseItemsViewModel2, DeliveryAddress = "Albacete", Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };
            //string expetedErrorMessage2 = "Please select at least a movie to be bought or cancel your purchase";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { suscripcion1,  expectedsuscripcionVM1, expetedErrorMessage1 }
                //,
                //,
                //new object[] { purchase2,  expectedPurchaseVM2, expetedErrorMessage2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithErrors))]
        public void Create_Post_WithErrors(SuscipcionCreateViewModel suscripcion, SuscipcionCreateViewModel expectedsuscripcionVM, string errorMessage)
        {
            using (context)
            {
                // Arrange
                var controller = new SuscripcionsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = suscripcioncontext;

                // Act
                var result = controller.CreatePost(suscripcion);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                SuscipcionCreateViewModel currentSuscripcion = viewResult.Model as SuscipcionCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedsuscripcionVM, currentSuscripcion);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }

        public static IEnumerable<object[]> TestCasesForPurchasesCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Suscripcion expectedsuscripcion1 = UtilitiesForSuscripcion.GetSuscripcion(0, 1).First();
            Cliente expectedCustomer1 = expectedsuscripcion1.Cliente;
            MotivoSuscripcion expectedPurchaseItem1 = expectedsuscripcion1.motivoSuscripcion.First();
            IList<MotivoSuscripcionViewModel> purchaseItemsViewModel1 = new MotivoSuscripcionViewModel[1] { new MotivoSuscripcionViewModel {
                   NewsletterId = expectedPurchaseItem1.newsletterId,
                    TituloNewssletter=expectedPurchaseItem1.NewsLetter.Titulo, Categoria=expectedPurchaseItem1.NewsLetter.Categoria.Nombre,
            Marca=expectedPurchaseItem1.NewsLetter.Marca.Nombre} };
            SuscipcionCreateViewModel suscripcion1 = new()
            {
                Name = expectedCustomer1.Name,
                FirstSurname = expectedCustomer1.FirstSurname,
                SecondSurname = expectedCustomer1.SecondSurname,
                MotivoSuscripcions = purchaseItemsViewModel1,
                FechaCaducidad = expectedsuscripcion1.FechaCaducidad

            };



            var allTests = new List<object[]>
            {                  //Input values   // expected values
                new object[] { suscripcion1,  expectedsuscripcion1}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPurchasesCreatePost_WithoutErrors))]
        public void Create_Post_WithoutErrors(SuscipcionCreateViewModel suscipcion, Suscripcion expectedSuscripcion)
        {
            using (context)
            {

                // Arrange
                var controller = new SuscripcionsController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = suscripcioncontext;

                // Act
                var result = controller.CreatePost(suscipcion);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualSuscripcion = context.Suscripcion.Include(p => p.motivoSuscripcion).
                                    FirstOrDefault(p => p.Id == expectedSuscripcion.Id);
                Assert.Equal(expectedSuscripcion, actualSuscripcion);

                //And that the quantity for purchase of each associated movie has been modified accordingly 



            }

        }


    }
    }