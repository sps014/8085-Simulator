using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace μp101.Core
{
    public static class Assembler
    {
        private static Dictionary<string, Processable> MnmonicsExecuter = new Dictionary<string, Processable>();
        public static LineAssembleResult ExecuteLine(string line)
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                return new LineAssembleResult();
            }

            LineAssembleResult result = new LineAssembleResult();

            string word = FetchMainWord(line,result);

            return result;
        }
        private static string FetchMainWord(string line,LineAssembleResult result)
        {
            
            var match=Regex.Match(line, @"\s*(\w+)\s*");
            if(match.Success)
            {
                if(match.Groups[0].Value.Length>=1)
                {
                    return match.Groups[0].Value;
                }
            }

            result.Result = AssembleOutcome.Failed;
            result.ErrorMessage = "Invalid instruction found , can not  be processed.";

            return null;
        }

        public delegate void Processable(string line, LineAssembleResult result);

    }
}
