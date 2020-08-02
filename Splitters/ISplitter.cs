using System;
using System.Collections.Generic;
using System.Text;

namespace SEEL.LinguisticProcessor.Splitters
{
    public interface ISplitter
    {
        string Split(string input);
    }
}
