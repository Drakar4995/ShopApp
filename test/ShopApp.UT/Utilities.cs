using ShopApp.Data;
using ShopApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UT
{
    public static class Utilities
    {
        public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()

        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("ShopApp")
                        .UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }
        public static void InitializeDbCustomersForTests(ApplicationDbContext db)
        {

            db.Users.Add(GetUsers(0, 1).First());
            db.Users.Add(GetUsers(1, 1).First());
            db.Users.Add(GetUsers(2, 1).First());
            db.Users.Add(GetUsers(3, 1).First());
            db.SaveChanges();
        }

        public static void ReInitializeDbUsersForTests(ApplicationDbContext db)
        {
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
        }

        public static IList<UsuarioApp> GetUsers(int index, int numOfUsers)
        {
            var allUsers = new List<UsuarioApp>
                {
                   new Cliente { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595",  Email = "peter@uclm.com", Name = "Peter", FirstSurname = "Jackson", SecondSurname = "García" },
                   new Cliente { Id = "2", UserName = "peter2@uclm.com", PhoneNumber = "2967959595", Email = "peter2@uclm.com", Name = "Peter2", FirstSurname = "Jackson2", SecondSurname = "García2" },
                   new Cliente { Id = "3", UserName = "peter3@uclm.com", PhoneNumber = "3967959595", Email = "peter3@uclm.com", Name = "Peter3", FirstSurname = "Jackson3", SecondSurname = "García3" },
                   new Gestor { Id = "4", UserName = "joseAngel@uclm.com", PhoneNumber = "967959593", Email = "joseAngel@uclm.com", Name = "JoseAngel", FirstSurname = "Jackson", SecondSurname = "García" }
        };
            //return from the list as much instances as specified in numOfGenres
            return allUsers.GetRange(index, numOfUsers);
        }
    }
}