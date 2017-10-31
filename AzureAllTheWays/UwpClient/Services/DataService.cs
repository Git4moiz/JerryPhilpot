using System;
using System.Linq;
using System.Text;

namespace UwpClient.Services
{
    public class DataService
    {
        private IDataAdapter _adapter;

        public DataService(IDataAdapter adapter)
        {
            _adapter = adapter;
        }
    }
}
