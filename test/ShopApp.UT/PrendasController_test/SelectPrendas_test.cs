using ShopApp.Controllers;
using ShopApp.Data;
using ShopApp.Models;
using ShopApp.Models.PrendaViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace ShopApp.UT.PrendasController_test
{
    public class SelectPrendas_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;

        Microsoft.AspNetCore.Http.DefaultHttpContext retirarContext;

        public SelectPrendas_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the InMemory database with test data.
            UtilitiesForPrendas.InitializeDbPrendasForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            purchaseContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            purchaseContext.User = identity;

        }
        public static IEnumerable<object[]> TestCasesForSelectPrendasForPurchase_get()
        {
            var allTests = new List<object[]>
            {


                new object[] { UtilitiesForPrendas.GetPrendas(0,4), UtilitiesForPrendas.GetMarcas(0,2), null, null, 0},
                new object[] { UtilitiesForPrendas.GetPrendas(0,0), UtilitiesForPrendas.GetMarcas(0,2), "Camiseta", null, 60},
                new object[] { UtilitiesForPrendas.GetPrendas(0,0), UtilitiesForPrendas.GetMarcas(0,2), null, UtilitiesForPrendas.GetMarcas(1,1).First().Nombre, 40},


            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectPrendasForPurchase_get))]
        public async Task SelectPrendasForPurchase_Get(List<Prenda> expectedPrendas, List<Marca> expectedMarcas, string filterNombre, string filterMarca, int filterPrecio)
        {
            using (context)
            {

                // Arrange
                var controller = new PrendasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                var expectedMarcasNames = expectedMarcas.Select(g => new { nameofMarca = g.Nombre });

                // Act
                var result = controller.SelectPrendasForPurchase(filterNombre, filterMarca, filterPrecio);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectPrendasForPurchaseViewModel model = viewResult.Model as SelectPrendasForPurchaseViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                Assert.Equal(expectedPrendas, model.Prendas);
                //check that both collections (expected and result) have the same names of Genre
                var modelMarcas = model.Marcas.Select(c => new { nameofMarca = c.Text });
                Assert.True(expectedMarcasNames.SequenceEqual(modelMarcas));
            }
        }


        [Fact]
        public void SelectPrendasForPurchase_Post_PrendasNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new PrendasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;
                //we create an array that is a list names of genres
                var expectedMarcas = UtilitiesForPrendas.GetMarcas(0, 4).Select(g => new { nameofMarca = g.Nombre });
                var expectedPrendas = UtilitiesForPrendas.GetPrendas(0, 4);

                SelectedPrendasForPurchaseViewModel selected = new SelectedPrendasForPurchaseViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectPrendasForPurchase(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectPrendasForPurchaseViewModel model = viewResult.Model as SelectPrendasForPurchaseViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedPrendas, model.Prendas);

                //check that both collections (expected and result) have the same names of Genre

                //var modelMarcas = model.Marcas.Select(g => new { nameofMarca = g.Text });
                var modelMarcas = UtilitiesForPrendas.GetMarcas(0, 4).Select(g => new { nameofMarca = g.Nombre });
                Assert.True(expectedMarcas.SequenceEqual(modelMarcas));

            }
        }

        [Fact]
        public void SelectPrendasForPurchase_Post_PrendasSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new PrendasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                String[] ids = new string[1] { "1" };
                SelectedPrendasForPurchaseViewModel prendas = new SelectedPrendasForPurchaseViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectPrendasForPurchase(prendas);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentPrendas = viewResult.RouteValues.Values.First();
                Assert.Equal(prendas.IdsToAdd, currentPrendas);

            }
        }

        public static IEnumerable<object[]> TestCasesForSelectPrendasForRetirar_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForPrendas.GetPrendas(2,2), UtilitiesForPrendas.GetMarcas(0,2), 0, null },
                new object[] { UtilitiesForPrendas.GetPrendas(2,1), UtilitiesForPrendas.GetMarcas(0,2), 2, null },
                new object[] { UtilitiesForPrendas.GetPrendas(3,1), UtilitiesForPrendas.GetMarcas(0,2), 0, "Adidas"}
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectPrendasForRetirar_get))]
        public async Task SelectPrendasForRetirar_Get(List<Prenda> expectedPrendas, List<Marca> expectedMarcas, int filterVentas, string filterMarca)
        {
            using (context)
            {

                // Arrange
                var controller = new PrendasController(context);
                controller.ControllerContext.HttpContext = retirarContext;

                var expectedMarcaSelectList = expectedMarcas.Select(g => new { marcaNombre = g.Nombre });
                // Act
                var result = controller.SelectPrendasForRetirar(filterVentas, filterMarca);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectPrendasForRetirarViewModel model = viewResult.Model as SelectPrendasForRetirarViewModel;

                var modem = model.Marcas.ToList();
                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                Assert.Equal(expectedPrendas, model.Prendas);
                // We need to use Comparer to compare both collections
                var modelMarcas = model.Marcas.Select(m => new { marcaNombre = m.Text });
                Assert.True(expectedMarcaSelectList.SequenceEqual(modelMarcas));


            }
        }

        [Fact]
        public void SelectPrendasForRetirar_Post_PrendasNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new PrendasController(context);
                controller.ControllerContext.HttpContext = retirarContext;
                var expectedMarcas = UtilitiesForPrendas.GetMarcas(0, 2).Select(g => new { nombreMarca = g.Nombre });
                var expectedPrendas = UtilitiesForPrendas.GetPrendas(2, 2);
                var expectedMarcaSelectList = expectedMarcas.Select(g => new { marcaNombre = g.nombreMarca });
                SelectedPrendasForRetirarViewModel selected = new SelectedPrendasForRetirarViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectPrendasForRetirar(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectPrendasForRetirarViewModel model = viewResult.Model as SelectPrendasForRetirarViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedPrendas, model.Prendas);
                var modelMarcas = model.Marcas.Select(m => new { marcaNombre = m.Text });
                Assert.True(expectedMarcaSelectList.SequenceEqual(modelMarcas));
            }
        }

        [Fact]
        public void SelectPrendasForRetirar_Post_PrendasSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new PrendasController(context);
                controller.ControllerContext.HttpContext = retirarContext;

                String[] ids = new string[1] { "1" };
                SelectedPrendasForRetirarViewModel prendas = new SelectedPrendasForRetirarViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectPrendasForRetirar(prendas);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentPrendas = viewResult.RouteValues.Values.First();
                Assert.Equal(prendas.IdsToAdd, currentPrendas);

            }

        }
    }

}
