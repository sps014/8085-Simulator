using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace μp101.Core
{
    public static class Tokenizer
    {
        public static Token[] GetTokens(string raw)
        {
            List<Token> tokens=new List<Token>();
            char[] whitespaces = new char[] { ' ','\t','\n' };
            for(int i=0;i<raw.Length;i++)
            {
                
                if(whitespaces.Contains(raw[i]))
                {
                    Token t = new Token();


                    tokens.Add(t);
                }
            }
            return tokens.ToArray();
        }
    }
    public class Token
    {
        public string TokenValue { get; set; }
        public TokenType Type { get; set; }
    }

    public enum TokenType
    {
        HexNumber,
        Number,
        WhiteSpace,
        NewLine,
        Comma,
        Word
    }
}
