using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace Sim8085.Core
{
    public class LineAssembleResult
    {
        public int LineNumber { get; set; }
        public int FutureLineNumber { get; set; }

        public List<Memory> MemoriesChanged { get; set; } = new List<Memory>();
        public List<Register> RegistersChanged { get; set; } = new List<Register>();
        public AssembleOutcome Result { get; set; } = AssembleOutcome.Success;
        public string ErrorMessage { get; set; } = "";
        //public int Bytes  { get;set; }
        //public List<string> OpCode { get; set; } = new List<string>();
        public bool IsHalt { get; set; } = false;
        public void SetError(string err)
        {
            Result = AssembleOutcome.Failed;
            ErrorMessage = err;
        }
    }
    
    public enum AssembleOutcome
    {
        Success,
        Failed
    }
}
