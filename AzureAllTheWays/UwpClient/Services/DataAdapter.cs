using System.Collections.Generic;
using System.Threading.Tasks;
using UwpClient.Helpers;

namespace UwpClient.Services
{
    public class DataAdapter : IDataAdapter
    {
        private WebHelper _webClientHelper;
        private SerializationHelper _serializationHelper;

        public DataAdapter()
        {
            _webClientHelper = new WebHelper();
            _serializationHelper = new SerializationHelper();
        }

        public async Task<IEnumerable<Models.Record>> ListAsync()
        {
            return null;
        }

        public async Task<IEnumerable<Models.Record>> SelectAsync(string id)
        {
            return null;
        }

        public async Task UpdateAsync(Models.Record obj)
        {

        }

        public async Task Delete(string id)
        {
        }

        public async Task ResetAsync()
        {
        }

        public async Task<string> InfoAsync()
        {
            return null;
        }
    }
}
