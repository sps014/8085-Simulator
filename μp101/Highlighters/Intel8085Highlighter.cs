using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace μp101.Highlighters
{
    public static class Intel8085Highlighter
    {
        public static string GenerateMarkDownText(string raw)
        {
            StringBuilder markdown = new StringBuilder(string.Empty);

            string[] tokens=raw.Split(new char[] { ',', ' ', '\n' });

            return markdown.ToString();
        }
        private static string 
    }
}
