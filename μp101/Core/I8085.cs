using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace μp101.Core
{
    public static class I8085
    {
        public static Memory[] Memory = new Memory[65536];
    }
    public class Register
    {
        public string Name { get; }
        public Memory Value { get; set; }
        public Register Pair { get; } = null;
        public Register(string n,Memory v,Register pair=null)
        {
            Name = n;
            Value = v;
            Pair = pair;
        }
    }
    public struct Memory
    {
        public byte Data { get; set; }
        public string Hex => Data.ToString("X2");
    }
}
