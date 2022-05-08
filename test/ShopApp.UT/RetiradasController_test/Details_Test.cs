using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ShopApp.Controllers;
using ShopApp.Data;
using ShopApp.Models;

namespace ShopApp.UT.RetiradasController_test
{
    public class Details_Test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext retiradaContext;

        public Details_Test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForRetiradas.InitializeDbRetiradasForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("joseAngel@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            retiradaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            retiradaContext.User = identity;

        }

        public static IEnumerable<object[]> TestCasesFor_Retirada_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Retirada_notfound_withoutId))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task Details_Retirada_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new RetiradasController(context);
                controller.ControllerContext.HttpContext = retiradaContext;


                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task Details_Retirada_found()
        {
            // Arrange
            using (context)
            {
                var expectedRetirada = UtilitiesForRetiradas.GetRetiradas(0, 1).First();
                var controller = new RetiradasController(context);
                controller.ControllerContext.HttpContext = retiradaContext;

                // Act
                var result = await controller.Details(expectedRetirada.id);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Retirada;
                Assert.Equal(expectedRetirada, model);

            }
        }


    }
}
