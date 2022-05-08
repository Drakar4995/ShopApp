using ShopApp.Data;
using ShopApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using ShopApp.UT.PrendasController_test;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UT.RetiradasController_test
{
    public static class UtilitiesForRetiradas
    {
        public static void InitializeDbRetiradasForTests(ApplicationDbContext db)
        {
            var retiradas = GetRetiradas(0, 1);
            foreach (Retirada retirada in retiradas)
            {
                db.Retirada.Add(retirada as Retirada);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbRetiradasForTests(ApplicationDbContext db)
        {
            db.MotivosRetirada.RemoveRange(db.MotivosRetirada);
            db.Retirada.RemoveRange(db.Retirada);
            db.SaveChanges();
        }

        public static IList<Retirada> GetRetiradas(int index, int numOfRetiradas)
        {
           
            Gestor gestor = Utilities.GetUsers(3, 1).First() as Gestor;
            var allRetiradas = new List<Retirada>();
            Retirada retirada;
            Prenda prenda;
            MotivoRetirada motivoRetirada;
            //int quantity = 2;

            for (int i = 1; i < 3; i++)
            {
                prenda = UtilitiesForPrendas.GetPrendas(i - 1, 1).First();
                //movie.QuantityForPurchase = movie.QuantityForPurchase - quantity;
                retirada = new Retirada
                {
                    id = i,
                    titulo = "Retirada" + i.ToString(),
                    Gestor = gestor,
                    descripcion = "Retirada mes noviembre",
                    gestorId = gestor.Id,
                    fechaEfectiva = System.DateTime.Now.AddMonths(1),
                    motivosRetirada = new List<MotivoRetirada>()
                };
                motivoRetirada = new MotivoRetirada
                {
                    id = i,
                    Prenda = prenda,
                    descripcion = "Pocas ventas",
                    Retirada = retirada,
                    retiradaId = retirada.id

                };
                retirada.motivosRetirada.Add(motivoRetirada);
                //purchase.TotalPrice = purchaseItem.Quantity * purchaseItem.Movie.PriceForPurchase;
                allRetiradas.Add(retirada);

            }

            return allRetiradas.GetRange(index, numOfRetiradas);
        }

    }
}
