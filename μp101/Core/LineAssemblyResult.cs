using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace μp101.Core
{
    public class LineAssembleResult
    {
        public List<Memory> MemoriesChanged { get; set; } = new List<Memory>();
        public List<Register> RegistersChanged { get; set; } = new List<Register>();
        public List<Flag> FlagsChanged { get; set; } = new List<Flag>();
        public AssembleOutcome Result { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    public enum AssembleOutcome
    {
        Success,
        Failed
    }
}
