using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPvPHelper
{
    internal class BitHelper
    {
        public static byte SetBit(byte b, int bitIndex, bool value)
        {
            if ((bitIndex < 0) || (bitIndex > 7))
            {
                throw new ArgumentOutOfRangeException("bitIndex", bitIndex, "bitNumber must be 0..7");
            }

            if (value)
                b |= (byte)(1 << bitIndex);
            else
                b &= (byte)~(1 << bitIndex);

            return b;
        }
        public static bool IsBitSet(byte value, int bitNumber)
        {
            if ((bitNumber < 0) || (bitNumber > 7))
            {
                throw new ArgumentOutOfRangeException("bitNumber", bitNumber, "bitNumber must be 0..7");
            }

            return ((value & (1 << bitNumber)) != 0);
        }
    }
}
