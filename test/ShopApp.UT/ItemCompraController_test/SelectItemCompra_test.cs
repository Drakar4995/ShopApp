using ShopApp.Controllers;
using ShopApp.Data;
using ShopApp.Models;
using ShopApp.Models.ItemCompraViewModels;
using ShopApp.UT.DevolucionesController_test;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace ShopApp.UT.ItemCompraController_test
{
    public class SelectItemCompra_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;

        

        public SelectItemCompra_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the InMemory database with test data.
            UtilitiesForItemCompra.InitializeDbItemCompraForTests(context);
            //context.ItemDevolucion.Add(UtilitiesForItemCompra.GetItemDevolucion(0, 1).First());
            //UtilitiesForItemCompra.InitializeDbItemDevolucionesforTest(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            purchaseContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            purchaseContext.User = identity;

        }
        public static IEnumerable<object[]> TestCasesForSelectItemCompraForDevolucion_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForItemCompra.GetItemsCompra(0,3), UtilitiesForItemCompra.GetMarcas(0,4), null, null, 1},
                new object[] { UtilitiesForItemCompra.GetItemsCompra(1,1), UtilitiesForItemCompra.GetMarcas(0,4), "Camisa", null,1},
                new object[] { UtilitiesForItemCompra.GetItemsCompra(2,1), UtilitiesForItemCompra.GetMarcas(0,4), null, "Puma",1},
             };

            return allTests;
        }
       
        [Theory]
        [MemberData(nameof(TestCasesForSelectItemCompraForDevolucion_get))]
        public async Task SelectItemComprasForDevolucion_Get(List<ItemCompra> expectedItemCompras, List<Marca> expectedMarcas, string filterNombre, string filterMarca, int id)
        {
            using (context)
            {
                // Arrange

                var controller = new ItemComprasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                var expectedMarcasNames = expectedMarcas.Select(g => new { nameofMarca = g.Nombre });

                // Act
                var result = controller.SelectPrendasForDevolucion(id, filterMarca, filterNombre);
                
                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectPrendasForDevolucionViewModel model = viewResult.Model as SelectPrendasForDevolucionViewModel;
                model.id = 1;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                /*
                */
                Assert.Equal(expectedItemCompras, model.ItemCompras);
                //check that both collections (expected and result) have the same names of Genre
                var modelMarcas = model.Marcas.Select(c => new { nameofMarca = c.Text });
                //var marca = model.Marcas.ToList();
                Assert.True(expectedMarcasNames.SequenceEqual(modelMarcas));
            }
        }


        [Fact]
        public void SelectItemCompraForDevolucion_Post_ItemCompraNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new ItemComprasController(context);
                
                controller.ControllerContext.HttpContext = purchaseContext;
                //we create an array that is a list names of genres
                var expectedMarcas = UtilitiesForItemCompra.GetMarcas(0,4).Select(g => new { nameofMarca = g.Nombre });
                
                var expectedItemsCompra = UtilitiesForItemCompra.GetItemsCompra(0,3);
                
                SelectedPrendasForDevolucionViewModel selected = new SelectedPrendasForDevolucionViewModel { IdsToAdd = null};
                selected.id = 1;
                
                // Act
                var result = controller.SelectPrendasForDevolucion(selected);
                
                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectPrendasForDevolucionViewModel model = viewResult.Model as SelectPrendasForDevolucionViewModel;
                
                
        
                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedItemsCompra, model.ItemCompras);

                //check that both collections (expected and result) have the same names of Genre

                //var modelMarcas = model.Marcas.Select(g => new { nameofMarca = g.Text });
                var modelMarcas = UtilitiesForItemCompra.GetMarcas(0, 4).Select(g => new { nameofMarca = g.Nombre });
                Assert.True(expectedMarcas.SequenceEqual(modelMarcas));

            }
        }

        [Fact]
        public void SelectItemCompraForDevolucion_Post_ItemCompraSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new ItemComprasController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                String[] ids = new string[1] { "1" };
                SelectedPrendasForDevolucionViewModel prendas = new SelectedPrendasForDevolucionViewModel { IdsToAdd = ids };

                // Act
                 var result = controller.SelectPrendasForDevolucion(prendas);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentPrendas = viewResult.RouteValues.Values.First();
                Assert.Equal(prendas.IdsToAdd, currentPrendas);

            }
        }
        
    }

}
