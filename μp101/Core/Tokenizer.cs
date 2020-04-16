using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace μp101.Core
{
    public class LineAssembleResult
    {
        public List<Memory> MemoriesChanged { get; set; }
        public List<Register> RegistersChanged { get; set; }
        public AssembleOutcome Result { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    public enum AssembleOutcome
    {
        Success,
        Failed
    }
}
