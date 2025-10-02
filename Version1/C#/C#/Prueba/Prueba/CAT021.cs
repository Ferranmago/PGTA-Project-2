using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba
{
    public class CAT021
    {
        public int Length;
        public byte[] DataRecord;

        public override string ToString()
        {
            return $"CAT021 | Length={Length} | DataRecord={BitConverter.ToString(DataRecord)}";
        }
    }
}
