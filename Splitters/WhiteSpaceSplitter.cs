using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SEEL.LinguisticProcessor.Splitters
{
    public class WhiteSpaceSplitter : ISplitter
    {
        public string Split(string input)
        {
            return String.Join( " ", Regex.Split(input, @"\s+", RegexOptions.Compiled).Where(s => s != string.Empty).ToArray() );
        }
    }
}
