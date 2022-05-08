using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ShopApp.UIT.Retirat_Test
{
    public class Retirar_Prendas_BasicFlow : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {"Monte Olimpo",DateTime.Today.AddDays(2).ToString(), "Motivo1"},
    };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

