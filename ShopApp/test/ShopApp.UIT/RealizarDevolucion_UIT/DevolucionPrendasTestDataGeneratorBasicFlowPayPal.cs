using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.UIT.RealizarDevolucion_UIT
{
    class DevolucionPrendasTestDataGeneratorBasicFlowPayPal : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
       {
         new object[] {

            "Domicilio",//tipoRecogida
            "Calle 1",//domicilio
            "motivoP1", //MotivoPrenda1
            "motivoP2",//MotivoPrenda2
            "PayPal",//MetodoPago
            null,//Number credit card
            null,//CCV
            null,//Fecha de caducidad
            "peter@uclm.com",//Email
            "967", //Prefix
            "673240"},//Phone
        };


        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

