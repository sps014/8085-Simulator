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
        public static int M 
        { 
            get
            {
                return Convert.ToInt32(H.Value.ToString().PadLeft(2,'0') + L.Value.ToString().PadLeft(2,'0'),16);
            }
            set
            {
                if (value < 0 || value >= MemorySize)
                    return;

                var val=value.ToString("X").PadLeft(4, '0');
                H.Value = (byte)Convert.ToInt32(val.Substring(0, 2),16);
                L.Value = (byte)Convert.ToInt32(val.Substring(2, 2), 16);
            }
        }
        public static Register PC_Upper { get; } = new Register("PC");
        public static Register PC { get; } = new Register("PC", PC_Upper);
        public static Register SP_Upper { get; } = new Register("SP");
        public static Register SP { get; } = new Register("SP", SP_Upper);
        public static Register PSW_Upper { get; } = new Register("PSW");
        public static Register PSW { get; } = new Register("PSW", PSW_Upper);
        public static Flag Flag_AC { get; } = new Flag("AC");
        public static Flag Flag_S { get; } = new Flag("S");
        public static Flag Flag_Z { get; } = new Flag("Z");
        public static Flag Flag_P { get; } = new Flag("P");
        public static Flag Flag_C { get; } = new Flag("C");

    }
    public class Register
    {
        public string Name { get; }
        public byte Value { get; set; }
        public Register Pair { get; } = null;
        public string Hex => Value.ToString("X2");

        public void FromHex(string str)
        {
            Value = Memory.FromHex_2(str);
        }
        public Register(string n, Register pair = null,byte val=0)
        {
            Name = n;
            Value = val;
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
        public static byte FromHex_2(string str)
        {
            if (str.Length == 2)
            {
                return Convert.ToByte(str, 16);
            }
            return 0;
        }
    }
    public class Flag
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
