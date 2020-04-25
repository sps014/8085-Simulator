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
                case "M":
                    return I8085.L;
                default:
                    return null;
            }
        }
        public static void AddAdjustFlags(byte value1,byte value2,byte c = 0)
        {
            string val1 = Convert.ToString(value1, 2).PadLeft(8, '0'); 
            string val2 = Convert.ToString(value2, 2).PadLeft(8, '0');

            string sum = "";
            for(int i=7;i>=0;i--)
            {
                byte a = (byte)(val1[i] == '0' ? 0 : 1);
                byte b = (byte)(val2[i] == '0' ? 0 : 1);
                var res=AddBit(a, b, c);
                c = res.Item2;
                if(i==4)
                {
                    I8085.Flag_AC.Value = Convert.ToBoolean(res.Item2);
                }
                sum = res.Item1 + sum;
            }
            
            I8085.Flag_Z.Value = (sum == "00000000")?true:false;
            
            I8085.Flag_C.Value = Convert.ToBoolean(c);

            I8085.Flag_S.Value=(value1+value2+c)>=0?true:false;

            I8085.Flag_P.Value=(Count1s((byte)(value1+value2+c))%2==0)?true:false;
        }
        public static void AddAdjustFlags(int value1, int value2,bool t)
        {
            string val1 = Convert.ToString(value1, 2).PadLeft(16, '0');
            string val2 = Convert.ToString(value2, 2).PadLeft(16, '0');

            string sum = "";
            for (int i = 15; i >= 0; i--)
            {
                byte a = (byte)(val1[i] == '0' ? 0 : 1);
                byte b = (byte)(val2[i] == '0' ? 0 : 1);
                var res = AddBit(a, b);
                if (i == 8)
                {
                    I8085.Flag_AC.Value = Convert.ToBoolean(res.Item2);
                }
                sum = res.Item1 + sum;
            }

            I8085.Flag_Z.Value = (sum == "00000000") ? true : false;


            I8085.Flag_S.Value = (value1 + value2 ) >= 0 ? true : false;

            I8085.Flag_P.Value = (Count1s((byte)(value1 + value2)) % 2 == 0) ? true : false;
        }

        public static void SubAdjustFlags(byte value1, byte value2, byte c = 0)
        {
            int res = value1 - value2 - c;
            var sum = Convert.ToString(res, 2).PadLeft(8,'0');

            I8085.Flag_Z.Value = (sum == "00000000") ? true : false;

            I8085.Flag_C.Value = Convert.ToBoolean(c);

            I8085.Flag_S.Value = (value1 - value2 - c) >= 0 ? true : false;

            I8085.Flag_P.Value = (Count1s((byte)(value1 - value2 - c)) % 2 == 0) ? true : false;
        }
        public static void SubAdjustFlags(int value1, int value2, bool g)
        {
            int res = value1 - value2;
            var sum = Convert.ToString(res, 2).PadLeft(8, '0');

            I8085.Flag_Z.Value = (sum == "00000000") ? true : false;


            I8085.Flag_S.Value = (value1 - value2 ) >= 0 ? true : false;

            I8085.Flag_P.Value = (Count1s((byte)(value1 - value2)) % 2 == 0) ? true : false;
        }

        /// <summary>
        /// sum , carry returns
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static (byte,byte) AddBit(byte a,byte b,byte c=0)
        {
            if (c == 0)
            {
                if (a == 0 && b == 0) 
                {
                    return (0, 0);
                }
                else if((a==0 && b==1 )|| (a == 1 && b==0))
                {
                    return (1, 0);
                }
                else
                {
                    return (0, 1);
                }
            }
            else
            {
                if (a == 0 && b == 0)
                {
                    return (1, 0);
                }
                else if ((a == 0 && b == 1) || (a == 1 && b == 0))
                {
                    return (0, 1);
                }
                else
                {
                    return (1, 1);
                }
            }
        }
        public static byte Compliment(byte b)
        {
            return (byte)(255 - b);
        }
        private static int Count1s(byte b)
        {
            string val2 = Convert.ToString(b, 2).PadLeft(8, '0');

            int c=0;
            for (int i = 7; i >= 0; i--)
            {
                if (val2[i] == '1')
                    c++;
            }
            return c;
        }
    }
}
