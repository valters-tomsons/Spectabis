using System;
using System.Net;

namespace Spectabis_WPF.Domain.Scraping {
    // Exception Fix: "The request was aborted: Could not create SSL/TLS secure channel."
    // https://stackoverflow.com/a/50977774/1148434
    // Put in a using clause it will set some globals and when disposed will set everything back to normal
    public class SecureTLSDumbfuckery : IDisposable {
        private readonly SecurityProtocolType _oldSecurityProtocol;
        private readonly bool _oldExpect100Continue;

        public SecureTLSDumbfuckery() {
            _oldSecurityProtocol = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12
               | SecurityProtocolType.Ssl3;

            _oldExpect100Continue = ServicePointManager.Expect100Continue;
            ServicePointManager.Expect100Continue = true;
        }

        public void Dispose() {
            ServicePointManager.SecurityProtocol = _oldSecurityProtocol;
            ServicePointManager.Expect100Continue = _oldExpect100Continue;
        }
    }
}
