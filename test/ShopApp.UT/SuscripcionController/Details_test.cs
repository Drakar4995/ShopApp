﻿using Microsoft.AspNetCore.Mvc;
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

namespace ShopApp.UT.SuscripcionController
{
    public class Details_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext suscripcionContext;

        public Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            UtilitiesForSuscripcion.InitializeDbSuscripcionForTests(context);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            suscripcionContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            suscripcionContext.User = identity;

        }


        public static IEnumerable<object[]> TestCasesFor_Suscripcion_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Suscripcion_notfound_withoutId))]
        public async Task Details_Suscripcion_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new SuscripcionsController(context);
                controller.ControllerContext.HttpContext = suscripcionContext;


                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        public async Task Details_Purchase_found()
        {
            // Arrange
            using (context)
            {
                var expectedSuscripcion = UtilitiesForSuscripcion.GetSuscripcion(0, 1).First();
                var controller = new SuscripcionsController(context);
                controller.ControllerContext.HttpContext = suscripcionContext;

                // Act
                var result = await controller.Details(expectedSuscripcion.Id);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Suscripcion;
                Assert.Equal(expectedSuscripcion, model);

            }
        }
    }
}
