using System.Collections.Generic;
using System.Threading.Tasks;

namespace UwpClient.Services
{
    public interface IDataAdapter
    {
        Task Delete(string id);
        Task<string> InfoAsync();
        Task<IEnumerable<Models.Record>> ListAsync();
        Task ResetAsync();
        Task<IEnumerable<Models.Record>> SelectAsync(string id);
        Task UpdateAsync(Models.Record obj);
    }
}
