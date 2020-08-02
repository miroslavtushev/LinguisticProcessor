using System;
using System.Net;

namespace SEEL.LinguisticProcessor
{
    /// <summary>
    /// Extends the functionality of the basic WebClient class by adding a timeout parameter
    /// </summary>
    class MyWebClient : WebClient
    {
        public int Timeout { get; set; }

        public MyWebClient() : this(60000) { }

        public MyWebClient(int timeout)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }
}
