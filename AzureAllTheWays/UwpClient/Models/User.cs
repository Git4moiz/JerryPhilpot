using Windows.Security.Credentials;
using Windows.Storage.Streams;

namespace UwpClient.Models
{
    internal class User
    {
        public string Id { get; set; }
        public WebAccountState State { get; set; }
        public string UserName { get; set; }
        public IRandomAccessStreamReference Picture { get; set; }
    }
}