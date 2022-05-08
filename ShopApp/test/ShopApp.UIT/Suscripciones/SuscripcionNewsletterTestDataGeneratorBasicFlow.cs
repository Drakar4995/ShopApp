using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UIT.Suscripciones
{
    public class SuscripcionNewsletterTestDataGeneratorBasicFlow : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {"Suscripcion1", "Descripcion1", "Motivo1", "05/06/2022"}
        //new object[] {"Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "peter@uclm.com", "967", "673240"},
    };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}

