using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEEL.LinguisticProcessor.Exceptions
{
    public class NoReleasesFoundException : Exception
    {
        public NoReleasesFoundException()
        {
        }

        public NoReleasesFoundException(string message)
            : base(message)
        {
        }

        public NoReleasesFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
