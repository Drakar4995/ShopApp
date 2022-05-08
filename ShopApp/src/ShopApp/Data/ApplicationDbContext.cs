using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Text;



namespace ShopApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Prenda> Prenda { get; set; }
        public DbSet<Marca> Marca { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public DbSet<MotivoRetirada> MotivosRetirada { get; set; }
        public DbSet<Retirada> Retirada { get; set; }
        public DbSet<ItemCompra> ItemCompra { get; set; }
        public DbSet<MetodoPago> MetodoPago { get; set; }
        public DbSet<NewsLetter> NewsLetter { get; set; }
        public DbSet<UsuarioApp> UserApp { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Devolucion> Devolucion { get; set; }
        public DbSet<ItemDevolucion> ItemDevolucion { get; set; }
        public DbSet<MotivoSuscripcion> MotivosSuscripcion { get; set; }
        public DbSet<Suscripcion> Suscripcion { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Table Per Hierarchy
            //https://docs.microsoft.com/en-us/ef/core/modeling/inheritance
            builder.Entity<MetodoPago>()
                .HasDiscriminator<string>("PaymentMethod_type")
                .HasValue<MetodoPago>("PaymentMethod")
                .HasValue<TarjetaBancaria>("PaymentMethod_CreditCard")
                .HasValue<PayPal>("PaymentMethod_PayPal");

            //Alternate Keys
            //https://docs.microsoft.com/en-us/ef/core/modeling/keys?tabs=data-annotations
            builder.Entity<Prenda>().HasAlternateKey(m => new { m.Nombre });
            builder.Entity<ItemCompra>().HasAlternateKey(pi => new { pi.Id, pi.CompraID });
            builder.Entity<Marca>().HasAlternateKey(g => new { g.Nombre });
            builder.Entity<NewsLetter>().HasAlternateKey(m => new { m.Titulo });
            builder.Entity<MotivoSuscripcion>().HasAlternateKey(pi => new { pi.Id, pi.newsletterId });

        }

        

    }
}