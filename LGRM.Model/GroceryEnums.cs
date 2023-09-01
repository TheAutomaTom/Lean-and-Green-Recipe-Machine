using System;
using System.Collections.Generic;
using System.Text;

namespace LGRM.Model
{
    public enum Kind
    {
        All = 0,
        Lean = 1,
        Green = 2,
        HealthyFat = 3,
        Condiment = 4,
    }

    [System.Flags]
    public enum UOMs : byte
    {
        W   = 1,
        V  =  2,
        C  =  4
            // eg:  6 = V & C,
            //      3 = W & C         
    }


}
