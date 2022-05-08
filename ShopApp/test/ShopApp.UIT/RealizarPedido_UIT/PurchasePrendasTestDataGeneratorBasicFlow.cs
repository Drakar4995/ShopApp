using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ShopApp.UIT.RealizarPedido_UIT
{
    public class PurchasePrendasTestDataGeneratorBasicFlow : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
    {
        new object[] { "Calle Evergreen Terrace 742, Albacete", "2", "1", "CreditCard", "1234567890123456", "123", DateTime.Today.AddYears(2).ToString(), null, null, null},
        new object[] { "Calle Evergreen Terrace 742, Albacete", "2", "1", "PayPal", null, null, null, "peter@uclm.com", "967", "673240"},
    };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}