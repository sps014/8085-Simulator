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

                Console.WriteLine("A:->"+I8085.A.Value.Hex);

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
    }
}
