using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpClient.Services
{
    public class SettingsService
    {
        public string AuthProviderEndpoint { get; } = "graph.windows.net";
        public string AuthAppEndpoint { get; } = "http://myapp.com";
        public string AuthAppScope { get; } = string.Empty;
        public string AuthClientId { get; } = Guid.Empty.ToString();
        public string AuthTenant { get; } = "developertenant.onmicrosoft.com";
    }
}
