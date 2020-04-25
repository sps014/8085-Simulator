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
        public static string Code { get; private set; } = null;
        private static List<string> Lines=new List<string>();

        public static Dictionary<string, int> LabelsCollection { get; set; } 
        = new Dictionary<string, int>();
        
        private static LineAssembleResult CurrentResult = null;

        public static void LoadToAssembly(string code)
        {
            Code = code;
            Lines.Clear();
            Lines.AddRange(code.Split(new char[] { '\n' }));
            CurrentResult = null;
            LabelsCollection = new Dictionary<string, int>();
        }
        public static List<LineAssembleResult> ExecuteRemaining()
        {
            var allResults = new List<LineAssembleResult>();
            if(Code==null)
            {
                allResults.Add(new LineAssembleResult() 
                { Result = AssembleOutcome.Failed, ErrorMessage = "No code loaded in assembly." });
                return allResults;
            }

            CurrentResult = CurrentResult == null ? null :CurrentResult.IsHalt?null:CurrentResult;
            int beginFrom = CurrentResult == null ? 0 : CurrentResult.FutureLineNumber;
            LineAssembleResult result;
            do
            {
                result = ExecuteLine(Lines[beginFrom], beginFrom);
                if (result == null)
                {
                    break;
                }
                if(result.FutureLineNumber>=Lines.Count)
                {
                    break;
                }
                if(result.Result==AssembleOutcome.Failed)
                {
                    allResults.Add(result);
                    break;
                }
                beginFrom = result.FutureLineNumber;
                CurrentResult = result;
                allResults.Add(result);

            } while (!result.IsHalt);

            return allResults;
        }
        public static LineAssembleResult ExecuteSingle()
        {
            CurrentResult = CurrentResult == null ? null : CurrentResult.IsHalt ? null : CurrentResult;
            int beginFrom = CurrentResult == null ? 0 : CurrentResult.FutureLineNumber;
            LineAssembleResult result;
            result = ExecuteLine(Lines[beginFrom], beginFrom);
            if (result == null)
            {
                return null;
            }
            if (result.FutureLineNumber >= Lines.Count)
            {
                return null;
            }
            if(result.Result!=AssembleOutcome.Failed)
            {
                CurrentResult = result;
            }
            return result;
        }

        private static LineAssembleResult ExecuteLine(string line,int lineNumber=0)
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                return new LineAssembleResult();
            }

            LineAssembleResult result = new LineAssembleResult();
            result.LineNumber = lineNumber;

            string word = FetchMainWord(ref line,result);

            if(word==null)
                return result;

            ProcessWord(word,line, result);


            return result;
        }
        private static void ExtraCharacterChecks( string s,LineAssembleResult res)
        {
            if (res == null)
                return;

            if(res.Result==AssembleOutcome.Success)
            {
                if(!string.IsNullOrWhiteSpace(s))
                {
                    res.SetError("Unknown characters:" + s);
                }
            }
        }
        private static string FetchMainWord(ref string line,LineAssembleResult result)
        {
            ExtractLabel(ref line, result.LineNumber);


            var match =Regex.Match(line, @"\s*(\w+)\s*");
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
        private static void ExtractLabel(ref string line,int number)
        {
            var match = Regex.Match(line, @"\b([\w]*)\s*\:");
            if(match.Success)
            {
                var gp = match.Groups[1].Value.ToUpper();
                line = line.Replace(match.Groups[0].Value, string.Empty);

                if (!LabelsCollection.ContainsKey(gp))
                {
                    LabelsCollection.Add(gp, number);
                }
            }
        }
        private static void ProcessWord(string word,string line,LineAssembleResult assembleResult)
        {
            string upperW = word.ToUpper();
            if (MnmonicsExecuter.ContainsKey(upperW))
            {
                MnmonicsExecuter[upperW](ref line, assembleResult);
            }
            else
            {
                assembleResult.SetError("Unidentified instruction :" + word);
            }
            ExtraCharacterChecks(line, assembleResult);
        }
        public static void Reset()
        {
            Code = null;
            LabelsCollection = new Dictionary<string, int>();
            Lines.Clear();

            Parallel.For(0, I8085.MemorySize, (i) =>
            {
                I8085.Memory[i].Data = 0;
            });

            I8085.A.Value = 0;
            I8085.B.Value = 0;
            I8085.C.Value = 0;
            I8085.D.Value = 0;
            I8085.E.Value = 0;
            I8085.L.Value = 0;
            I8085.H.Value = 0;
            I8085.PC.Value = 0;
            I8085.PC_Upper.Value = 0;
            I8085.SP.Value = 0;
            I8085.SP_Upper.Value = 0;
            I8085.PSW.Value = 0;
            I8085.PSW_Upper.Value = 0;

            I8085.Flag_AC.Value = false;
            I8085.Flag_Z.Value = false;
            I8085.Flag_C.Value = false;
            I8085.Flag_P.Value = false;
            I8085.Flag_S.Value = false;


        }

        public delegate void Processable(ref string line, LineAssembleResult result);

        public enum ExecutionType
        {
            StepByStep,
            InOneGo
        }

    }
}
