using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEEL.LinguisticProcessor.Splitters
{
    public class DigitSplitter : ISplitter
    {
        public string Split(string input)
        {
            return String.Join( " ", Regex.Split(input, @"\d+") );
        }
    }
}
