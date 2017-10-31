using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
namespace UwpClient.Helpers
{
    public class WebHelper
    {
        static HttpClient _http;
        const string applicationJson = "application/json";

        private HttpClient Client()
        {
            if (_http != null)
            {
                return _http;
            }
            _http = new HttpClient();
            var header = new HttpMediaTypeWithQualityHeaderValue(applicationJson);
            _http.DefaultRequestHeaders.Accept.Add(header);
            return _http;
        }

        public async Task<string> GetAsync(Uri path)
            => await (await Client().GetAsync(path)).Content.ReadAsStringAsync();

        public async Task<HttpResponseMessage> PutAsync(Uri path, string payload)
            => await Client().PutAsync(path, ToContent(payload));

        public async Task<HttpResponseMessage> PostAsync(Uri path, string payload)
            => await Client().PostAsync(path, ToContent(payload));

        HttpStringContent ToContent(string payload)
            => new HttpStringContent(payload, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

        public async Task<HttpResponseMessage> DeleteAsync(Uri path)
            => await Client().DeleteAsync(path);
    }
}
