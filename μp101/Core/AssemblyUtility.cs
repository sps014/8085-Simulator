using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace μp101.Core
{
    public static class AssemblyUtility
    {
        public static Register IsRegister(string word)
        {
            switch(word)
            {
                case "A":
                    return I8085.A;
                case "B":
                    return I8085.B;
                case "C":
                    return I8085.C;
                case "D":
                    return I8085.D;
                case "E":
                    return I8085.E;
                case "H":
                    return I8085.H;
                case "L":
                    return I8085.L;
                default:
                    return null;
            }
        }
    }
}
