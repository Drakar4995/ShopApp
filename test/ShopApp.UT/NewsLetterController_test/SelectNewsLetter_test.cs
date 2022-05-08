using ShopApp.Controllers;
using ShopApp.Data;
using ShopApp.Models;
using ShopApp.Models.NewsletterViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShopApp.UT.NewsLetterController_test
{
   public class SelectNewsLetter_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext suscripcionContext;

        public SelectNewsLetter_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the InMemory database with test data.
            UtilitiesForNewsLetter.InitializeDbNewsLetterForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            suscripcionContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            suscripcionContext.User = identity;

        }
        public static IEnumerable<object[]> TestCasesForSelectNewsletterForSuscribe_get()
        {
            
       
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForNewsLetter.GetNewsLetter(0,4), UtilitiesForNewsLetter.GetMarcas(0,2), null , null },
                new object[] { UtilitiesForNewsLetter.GetNewsLetter(0,3), UtilitiesForNewsLetter.GetMarcas(0,2), "Sport", null},
                new object[] { UtilitiesForNewsLetter.GetNewsLetter(3,1), UtilitiesForNewsLetter.GetMarcas(0,2), null, "Puma"},

        };


            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectNewsletterForSuscribe_get))]
        public async Task SelectNewsletterForSuscribe_get(List<NewsLetter> expectedNewsLetter, List<Marca> expectedMarca,  String filterCategoria, String filterMarca)
        {
            using (context)
            {

                // Arrange
                var controller = new NewsLettersController(context);
                controller.ControllerContext.HttpContext = suscripcionContext;

                var expectedMarcasNames = expectedMarca.Select(g => new { nameofMarca = g.Nombre });

                // Act
                var result = controller.SelectNewsletterForSuscribe(filterCategoria, filterMarca);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectNewsletterForSuscribeViewModel model = viewResult.Model as SelectNewsletterForSuscribeViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                Assert.Equal(expectedNewsLetter, model.Newsletters);
                //check that both collections (expected and result) have the same names of Genre
                var modelMarcas = model.marcaNombres.Select(g => new { nameofMarca = g.Text });
                Assert.True(expectedMarcasNames.SequenceEqual(modelMarcas));
            }
        }


        [Fact]

        public void SelectNewsletterForSuscribe_Post_NewsLettersNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new NewsLettersController(context);
                controller.ControllerContext.HttpContext = suscripcionContext;
                //we create an array that is a list names of genres
                var expectedMarcas = UtilitiesForNewsLetter.GetMarcas(0, 2).Select(g => new { nameofMarca = g.Nombre });
                var expectedNewsLetter = UtilitiesForNewsLetter.GetNewsLetter(0, 4);

                SelectedNewsletterForSuscribeViewModel selected = new SelectedNewsletterForSuscribeViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectNewsletterForSuscribe(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectNewsletterForSuscribeViewModel model = viewResult.Model as SelectNewsletterForSuscribeViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedNewsLetter, model.Newsletters);

                //check that both collections (expected and result) have the same names of Genre
                var modelMarcas = model.marcaNombres.Select(g => new { nameofMarca = g.Text });
                Assert.True(expectedMarcas.SequenceEqual(modelMarcas));

            }
        }

        [Fact]
        public void SelectNewsletterForSuscribe_Post_NewsLetterSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new NewsLettersController(context);
                controller.ControllerContext.HttpContext = suscripcionContext;

                int[] ids = new int[1] { 1 };
                SelectedNewsletterForSuscribeViewModel newsletter = new SelectedNewsletterForSuscribeViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectNewsletterForSuscribe(newsletter);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentNewsletters = viewResult.RouteValues.Values.First();
                Assert.Equal(newsletter.IdsToAdd, currentNewsletters);

            }
        }
    }


}
