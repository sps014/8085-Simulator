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
                if(AssemblyUtility.IsRegister(match.Groups[1].Value)!=null
                    && AssemblyUtility.IsRegister(match.Groups[2].Value) != null)
                {

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
