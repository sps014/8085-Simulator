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
        public static Memory[] Memory { get; } = new Memory[65536];
        public const long MemorySize = 65536;
        public static Register A { get; } = new Register("A");
        public static Register B { get; } = new Register("B");
        public static Register C { get; } = new Register("C", B);
        public static Register D { get; } = new Register("D");
        public static Register E { get; } = new Register("E", D);
        public static Register H { get; } = new Register("H");
        public static Register L { get; } = new Register("L", H);
        public static Register PC_Upper { get; set; } = new Register("PC");
        public static Register PC { get; set; } = new Register("PC", PC_Upper);
    }
    public class Register
    {
        public string Name { get; }
        public Memory Value { get; set; }
        public Register Pair { get; } = null;
        public Register(string n, Register pair = null, Memory v = new Memory())
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
        public void FromHex(string str)
        {
            if(str.Length==2)
            {
                Data = Convert.ToByte(str, 16);
            }
        }
    }
    public struct Flag
    {
        public string Name { get; }
        public bool Value { get; set; }
        public Flag(string n, bool v = false)
        {
            Name = n;
            Value = v;
        }
    }
}
