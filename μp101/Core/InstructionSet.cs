using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace μp101.Core
{
    public static class InstructionSet
    {
        public static void MOV(string line,LineAssembleResult result)
        {
            var match = Regex.Match(line, @"MOV\s+(\w)\s*\,\s*(\w)");
            if(match.Success)
            {
                Register to = AssemblyUtility.IsRegister(match.Groups[1].Value);
                Register from = AssemblyUtility.IsRegister(match.Groups[2].Value);

                if (to!=null && from != null)
                {
                    to.Value = from.Value;
                    result.RegistersChanged.Add(to);
                }
                else
                {
                    result.SetError("MOV instruction work with Register to Register Only.");
                    return;
                }

            }
            else
            {
                result.SetError("MOV instruction is incorrectly formatted");
                return;
            }
        }
        public static void MVI(string line,LineAssembleResult result)
        {
            var match = Regex.Match(line, @"MVI\s+([\w])\s*\,\s*(\d{1,}H?)");
            if(match.Success)
            {
                Register to = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if(to!=null)
                {
                    bool valueHexType = match.Groups[2].Value.ToLower().IndexOf("h") > 0;
                    byte value;
                    if (valueHexType)
                    {
                        string val = match.Groups[2].Value.ToLower().Replace("h", string.Empty);
                        value = Convert.ToByte(val, 16);
                    }
                    else
                    {
                        value= Convert.ToByte(match.Groups[2].Value, 10);
                    }

                    if(value>=0&&value<=255)
                    {
                        to.Value = value;
                        result.RegistersChanged.Add(to);
                    }
                    else
                    {
                        result.SetError("MVI should utilized 8 bit data only ie from 00-FF or 0-255");
                        return;
                    }
                }
                else
                {
                    result.SetError("MVI instruction work with Register to Register Only.");
                    return;
                }
            }
            else
            {
                result.SetError("MVI instruction is incorrectly formatted");
                return;
            }
        }
        public static void ADD(string line, LineAssembleResult result)
        {
            var match = Regex.Match(line, @"ADD\s+([\w])");
            if (match.Success)
            {
                Register from = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if (from != null)
                {
                    I8085.A.Value += from.Value;
                    result.RegistersChanged.Add(I8085.A);

                }
                else
                {
                    result.SetError("MVI instruction work with Register to Register Only.");
                    return;
                }
            }
            else
            {
                result.SetError("ADD instruction is incorrectly formatted");
                return;
            }
        }
        public static void ADI(string line, LineAssembleResult result)
        {
            var match = Regex.Match(line, @"ADI\s+(\d{1,}H?)");
            if (match.Success)
            {
                bool valueHexType = match.Groups[2].Value.ToLower().IndexOf("h") > 0;
                byte value;
                if (valueHexType)
                {
                    string val = match.Groups[2].Value.ToLower().Replace("h", string.Empty);
                    value = Convert.ToByte(val, 16);
                }
                else
                {
                    value = Convert.ToByte(match.Groups[2].Value, 10);
                }
                if (value >= 0 && value <= 255)
                {
                    I8085.A.Value+ = value;
                    result.RegistersChanged.Add(I8085.A);
                }
                else
                {
                    result.SetError("MVI should utilized 8 bit data only ie from 00-FF or 0-255");
                    return;
                }
            }
            else
            {
                result.SetError("ADD instruction is incorrectly formatted");
                return;
            }
        }


    }
}
