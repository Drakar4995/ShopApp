using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShopApp.UIT.RealizarDevolucion_UIT
{
    public class DevolucionPrendasTestDataGeneratorBasicFlow : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
       {
         new object[] {
            "Correos", //tipoRecogida
            null, //domicilio
            "motivoP1", //MotivoPrenda1
            "motivoP2",//MotivoPrenda2
            "CreditCard",//MetodoPago
            "1234567890123456",//Number credit card
            "123",//CCV
            DateTime.Today.AddYears(2).ToString(),//Fecha de caducidad
            null,//Emai
            null,//Prefix
            null} };/*, //Phone
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
        */
     

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
     }
}

