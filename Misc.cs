using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockFishPortApp_12._0
{
    public class Misc
    {
        public const string Version = "12";
    }

    public class PRNG
    {
        UInt64 s;

        public UInt64 rand64()
        {

            s ^= s >> 12; s ^= s << 25; s ^= s >> 27;
            return s * 2685821657736338717L;
        }

        public PRNG(UInt64 seed)
        {
            s = seed;
            Debug.Assert(seed!=0);
        }

        public UInt64 rand()
        {
            return rand64();
        }
        
        public UInt64 sparse_rand()
        {
            return rand64() & rand64() & rand64();
        }
    }
}
