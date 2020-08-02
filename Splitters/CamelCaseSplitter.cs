using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SEEL.LinguisticProcessor.Splitters
{
    public class CamelCaseSplitter : ISplitter
    {
        private static WhiteSpaceSplitter m_whiteSpaceSplitter = new WhiteSpaceSplitter();

        public string Split(string input)
        {
            // split the camel case and then apply white space splitting
            return Regex.Replace(input, @"((?<=[A-Z])([A-Z])(?=[a-z]))|((?<=[a-z]+)([A-Z]))", @" $0", RegexOptions.Compiled).Trim();
        }
    }
}
