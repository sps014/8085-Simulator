using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace μp101.Core
{
    public static class Assembler
    {
        private static Dictionary<string, Processable> 
            MnmonicsExecuter = new Dictionary<string, Processable>()
            {
                {"MOV",InstructionSet.MOV },
                {"MVI",InstructionSet.MVI },
                {"ADD",InstructionSet.ADD }
            };

        public static LineAssembleResult ExecuteLine(string line)
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                return new LineAssembleResult();
            }

            LineAssembleResult result = new LineAssembleResult();

            string word = FetchMainWord(line,result);

            if(word==null)
                return result;

            ProcessWord(word, line, result);

            return result;
        }
        private static string FetchMainWord(string line,LineAssembleResult result)
        {
            
            var match=Regex.Match(line, @"\s*(\w+)\s*");
            if(match.Success)
            {
                if(match.Groups[1].Value.Length>=1)
                {
                    return match.Groups[1].Value;
                }
            }

            result.Result = AssembleOutcome.Failed;
            result.ErrorMessage = "Invalid instruction found , can not  be processed.";

            return null;
        }
        private static void ProcessWord(string word,string line,LineAssembleResult assembleResult)
        {
            if(MnmonicsExecuter.ContainsKey(word.ToUpper()))
            {
                MnmonicsExecuter[word](line.ToUpper(), assembleResult);
            }
            else
            {
                assembleResult.SetError("Unidentified keyword:" + word);
            }
        }

        public delegate void Processable(string line, LineAssembleResult result);

    }
}
