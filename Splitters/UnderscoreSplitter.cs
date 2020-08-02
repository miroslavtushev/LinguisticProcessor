using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SEEL.LinguisticProcessor.Splitters
{
    public class UnderscoreSplitter : ISplitter
    {
        public string Split(string input)
        {
            return String.Join( " ", Regex.Split(input, @"_+", RegexOptions.Compiled).Where(s => s != string.Empty).ToArray() );
        }
    }
}
