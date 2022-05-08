using ShopApp.Data;
using ShopApp.Models;
using ShopApp.UT.NewsLetterController_test;
using ShopApp.UT.SuscripcionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UT.SuscripcionController
{
    public static class UtilitiesForSuscripcion
    {
        public static void InitializeDbSuscripcionForTests(ApplicationDbContext db)
        {
            var suscripcions = GetSuscripcion(0, 1);
            foreach (Suscripcion suscripcion in suscripcions)
            {
                db.Suscripcion.Add(suscripcion as Suscripcion);
            }
            db.SaveChanges();

        }

        public static void ReInitializeDbSuscripcionForTests(ApplicationDbContext db)
        {
            db.MotivosSuscripcion.RemoveRange(db.MotivosSuscripcion);
            db.Suscripcion.RemoveRange(db.Suscripcion);
            db.SaveChanges();
        }

        public static IList<Suscripcion> GetSuscripcion(int index, int numOfSuscripciones)
        {

            Cliente customer = Utilities.GetUsers(0, 1).First() as Cliente;
            var allSuscripcions = new List<Suscripcion>();
            Suscripcion suscripcion;
            NewsLetter newsLetter;
            MotivoSuscripcion motivoSuscripcion;
            int quantity = 2;

            for (int i = 1; i < 3; i++)
            {
                newsLetter = UtilitiesForNewsLetter.GetNewsLetter(i - 1, 1).First();
                suscripcion = new Suscripcion
                {
                    Id = i,
                    Cliente = customer,
                    clienteID = customer.Id,
                    Titulo = null,
                    Descripcion = null,
                    motivo = null,
                    FechaCaducidad = new DateTime(2022,01,01),
                    motivoSuscripcion = new List<MotivoSuscripcion>()
                };
                motivoSuscripcion = new MotivoSuscripcion
                {
                    Id = i,
                    NewsLetter = newsLetter,
                    newsletterId = newsLetter.Id,
                    Suscripcion = suscripcion,
                    suscripcionId = suscripcion.Id

                };
                suscripcion.motivoSuscripcion.Add(motivoSuscripcion);
                allSuscripcions.Add(suscripcion);

            }

            return allSuscripcions.GetRange(index, numOfSuscripciones);
        }

    }
}
