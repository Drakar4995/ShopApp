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
using ShopApp.Models.RetiradaViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Runtime.ExceptionServices;
using ShopApp.Models.PrendaViewModels;
using ShopApp.UT.PrendasController_test;
using ShopApp.UT.RetiradasController_test;

namespace ShopApp.UT.RetiradasController_test
{
    public class Create_Test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext retiradaContext;


        public Create_Test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForPrendas.InitializeDbPrendasForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("joseAngel@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            retiradaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
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
                var controller = new RetiradasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = retiradaContext;

                String[] ids = new string[1] { "1" };
                SelectedPrendasForRetirarViewModel prendas = new() { IdsToAdd = ids };
                Prenda expectedPrenda = UtilitiesForPrendas.GetPrendas(0, 1).First();
                Gestor expectedGestor = Utilities.GetUsers(3, 1).First() as Gestor;
                int ventasSemanales = context.ItemCompra.Include(c => c.Compra)
                    .Where(pr => pr.PrendaID == expectedPrenda.PrendaID && (pr.Compra.FechaCompra.CompareTo(DateTime.Today.AddDays(-7)) > 0)).Sum(ca => ca.Cantidad);
                IList<MotivoRetiradaViewModel> expectedMotivoRetirada = new MotivoRetiradaViewModel[1] {
                    new MotivoRetiradaViewModel {Nombre=expectedPrenda.Nombre,Marca=expectedPrenda.Marca.Nombre,Precio=expectedPrenda.PrecioPrenda,PrendaID=expectedPrenda.PrendaID,
                                                    Descripcion=null,VentasSemanales=ventasSemanales} };
                RetiradaCreateViewModel expectedRetirada = new() { MotivosRetirada = expectedMotivoRetirada, GestorId = expectedGestor.Id, Descripcion = null, FechaEfectiva=new DateTime()
                    ,Titulo="Retirada"+(context.Retirada.Count()+1).ToString() };

                // Act
                var result = controller.Create(prendas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                RetiradaCreateViewModel currentRetirada = viewResult.Model as RetiradaCreateViewModel;

                Assert.Equal(currentRetirada, expectedRetirada);

            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutPrenda()
        {
            using (context)
            {

                // Arrange
                var controller = new RetiradasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = retiradaContext;
                Gestor gestor = Utilities.GetUsers(3, 1).First() as Gestor;
                SelectedPrendasForRetirarViewModel prendas = new();

                RetiradaCreateViewModel expectedRetirada = new()
                {
                    GestorId=gestor.Id,
                    Titulo="Retirada"+(context.Retirada.Count()+1).ToString(),
                    MotivosRetirada = new List<MotivoRetiradaViewModel>(),
                    Descripcion=null,
                    FechaEfectiva=new DateTime()
                };


                // Act
                var result = controller.Create(prendas);

                //Assert

                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                RetiradaCreateViewModel currentRetirada = viewResult.Model as RetiradaCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentRetirada, expectedRetirada);
                Assert.Equal("Tienes que seleccionar una prenda", error.ErrorMessage);
            }
        }


        public static IEnumerable<object[]> TestCasesForRetiradasCreatePost_WithErrors()
        {
            //Las siguientes dos pruebas sustituyen a los métodos indicados usando Theory. No usar los métodos Fact.
            //The following two tests are subtitutes of the indicated facts methods using Theory instead of Fact. Please, do not use the Fact methods.
            //First error: Create_Post_WithoutEnoughMoviesToBePurchased
            
            Prenda prenda = UtilitiesForPrendas.GetPrendas(0, 1).First();
            Gestor gestor = Utilities.GetUsers(3, 1).First() as Gestor;
            //  var payment1 = new PayPal { Email = customer.Email, Phone = customer.PhoneNumber, Prefix = "+34" };

            //Input values
        
            IList<MotivoRetiradaViewModel> motivoRetiradaViewModels1= new MotivoRetiradaViewModel[1] { new MotivoRetiradaViewModel { Descripcion="Pocas Ventas", PrendaID = prenda.PrendaID, Nombre=prenda.Nombre, Marca = prenda.Marca.Nombre, Precio=prenda.PrecioPrenda, VentasSemanales=0 } };
            RetiradaCreateViewModel retirada1 = new() { Titulo="Retirada1", Descripcion="Retirada Noviembre", GestorId=gestor.Id, MotivosRetirada = motivoRetiradaViewModels1, FechaEfectiva=new DateTime(20/11/2021) };

            //Expected values
            IList<MotivoRetiradaViewModel> expectedmotivoRetiradaViewModels1 = new MotivoRetiradaViewModel[1] { new MotivoRetiradaViewModel { Descripcion = "Pocas Ventas", PrendaID = prenda.PrendaID, Nombre = prenda.Nombre, Marca = prenda.Marca.Nombre, Precio = prenda.PrecioPrenda, VentasSemanales = 0 } };
            RetiradaCreateViewModel expectedretirada1 = new() { Titulo = "Retirada1", Descripcion = "Retirada Noviembre", GestorId = gestor.Id, MotivosRetirada = motivoRetiradaViewModels1, FechaEfectiva = new DateTime(20 / 11 / 2021) };
            string expetedErrorMessage1 = "Fecha no valida";

            var allTests = new List<object[]>
            {                  //Input values                                       // expected values
                new object[] { retirada1,  expectedretirada1, expetedErrorMessage1 }
               
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForRetiradasCreatePost_WithErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithErrors(RetiradaCreateViewModel retirada, RetiradaCreateViewModel expectedRetiradaVM, string errorMessage)
        {
            using (context)
            {
                
                // Arrange
                var controller = new RetiradasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = retiradaContext;

                // Act
                var result = controller.CreatePost(retirada);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                RetiradaCreateViewModel currentRetirada = viewResult.Model as RetiradaCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First(); ;
                Assert.Equal(expectedRetiradaVM, currentRetirada);
                Assert.Equal(errorMessage, error.ErrorMessage);


            }

        }

        public static IEnumerable<object[]> TestCasesForRetiradasCreatePost_WithoutErrors()
        {
            //Substitución similar a la vista anteriormente.
            //Same substitution as the former two tests.

            //Purchase with CreditCard
            Retirada expectedRetirada1 = UtilitiesForRetiradas.GetRetiradas(0, 1).First();
            Gestor expectedGestor1 = expectedRetirada1.Gestor;
            //var expectedPayment1 = expectedPurchase1.PaymentMethod as CreditCard;
            MotivoRetirada expectedPMotivoRetirada1 = expectedRetirada1.motivosRetirada.First();
            //int expectedQuantityForPurchase1 = UtilitiesForMovies.GetMovies(0, 1).First().QuantityForPurchase - expectedPurchaseItem1.Quantity;

            IList<MotivoRetiradaViewModel> motivoRetiradaViewModel1 = new MotivoRetiradaViewModel[1] { new MotivoRetiradaViewModel {
                    Descripcion = expectedPMotivoRetirada1.descripcion, PrendaID = expectedPMotivoRetirada1.Prenda.PrendaID,
                    Nombre= expectedPMotivoRetirada1.Prenda.Nombre, Marca = expectedPMotivoRetirada1.Prenda.Marca.Nombre,
                    Precio = expectedPMotivoRetirada1.Prenda.PrecioPrenda, VentasSemanales = 0} };
            RetiradaCreateViewModel retirada1 = new()
            {
                GestorId = expectedGestor1.Id,
                MotivosRetirada = motivoRetiradaViewModel1,
                Descripcion = expectedRetirada1.descripcion,
                FechaEfectiva = expectedRetirada1.fechaEfectiva,
                Titulo = expectedRetirada1.titulo
            };

            

            var allTests = new List<object[]>
            {                  //Input values   // expected values
                new object[] { retirada1,  expectedRetirada1}
                
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForRetiradasCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(RetiradaCreateViewModel retirada, Retirada expectedRetirada)
        {
            using (context)
            {

                // Arrange
                var controller = new RetiradasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = retiradaContext;

                // Act
                var result = controller.CreatePost(retirada);

                //Assert
                //we should check it is redirected to details
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
                Assert.Equal("Details", viewResult.ActionName);

                //we should check the purchase has been created in the database
                var actualRetirada = context.Retirada.Include(p => p.motivosRetirada).
                                    FirstOrDefault(p => p.id== expectedRetirada.id);
                Assert.Equal(expectedRetirada, actualRetirada);

                //And that the quantity for purchase of each associated movie has been modified accordingly 
                Assert.True(
                    context.Prenda.First(m => m.PrendaID == expectedRetirada.motivosRetirada.First().Prenda.PrendaID).isRetired);


            }

        }

    }
}
