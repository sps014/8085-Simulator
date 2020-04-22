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
                {"ADD",InstructionSet.ADD },
                {"ADI",InstructionSet.ADI },
                {"HLT",InstructionSet.HLT }
            };
        private static string Code = null;
        private static string[] Lines;

        public static Dictionary<string, int> LabelsCollection { get; set; } 
        = new Dictionary<string, int>();
        
        private static LineAssembleResult CurrentResult = null;

        public static void LoadToAssembly(string code)
        {
            Code = code;
            Lines = code.Split("\r\n");
            CurrentResult = null;
            LabelsCollection = new Dictionary<string, int>();
        }
        public static IEnumerable<LineAssembleResult> ExecuteRemaining()
        {
            int beginFrom = CurrentResult == null ? 0 : CurrentResult.FutureLineNumber;
            LineAssembleResult result;
            do
            {
                result = ExecuteLine(Lines[beginFrom], beginFrom);
                if (result == null)
                {
                    break;
                }
                if(result.FutureLineNumber>=Lines.Length)
                {
                    break;
                }
                beginFrom = result.FutureLineNumber;

                yield return result;

            } while (!result.IsHalt);
        }

        public static LineAssembleResult ExecuteLine(string line,int lineNumber=0)
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                return new LineAssembleResult();
            }

            LineAssembleResult result = new LineAssembleResult();
            result.LineNumber = lineNumber;

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

        public enum ExecutionType
        {
            StepByStep,
            InOneGo
        }

    }
}
