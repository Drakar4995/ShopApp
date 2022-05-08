using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ShopApp.Design;

namespace ShopApp.Design
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class UsuarioApp : IdentityUser
    {
        public virtual string Name
        {
            get;
            set;
        }

        [Display(Name = "First Surname")]
        public virtual string FirstSurname
        {
            get;
            set;
        }

        [Display(Name = "Second Surname")]
        public virtual string SecondSurname
        {
            get;
            set;
        }




        public override bool Equals(object obj)
        {
            return obj is UsuarioApp user &&
               Id == user.Id &&
                   Email == user.Email &&
                   PhoneNumber == user.PhoneNumber &&
               EqualityComparer<DateTimeOffset?>.Default.Equals(LockoutEnd, user.LockoutEnd) &&
               LockoutEnabled == user.LockoutEnabled &&
               Name == user.Name &&
               FirstSurname == user.FirstSurname &&
               SecondSurname == user.SecondSurname;
                  
        }

    }
}

