using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace μp101.Core
{
    public static class Intel8085Highlighter
    {
        private const string NumberColor = "yellow";
        public static string GenerateMarkDownText(string raw)
        {
            StringBuilder markdown = new StringBuilder("<div>");

            markdown.Append(raw);

            HighlightNumbers(ref markdown);

            markdown.Append("</div>");
            return markdown.ToString();
        }
        private static void HighlightNumbers(ref StringBuilder sb)
        {
            var matches=Regex.Matches(sb.ToString(), @"\s\d{1,}H?",RegexOptions.IgnoreCase);
            foreach(Match m in matches)
            {
                sb = sb.Replace(m.Groups[0].Value, GenerateStyle(m.Groups[0].Value, NumberColor));
            }
        }

        private static string GenerateStyle(string value,string color)
        {
            return $"<span style='color:{color}'>{value}</span>";
            
        }
    }
}
