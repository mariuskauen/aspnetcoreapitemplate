using System.Collections.Generic;
using System.Threading.Tasks;
using soapApi.Models;
using soapApi.ViewModels;

namespace soapApi.Data
{
    public interface IDataRepository
    {
         Task<Value> GetValueAsync(int id);
         Task<List<Value>> GetAllValuesAsync();
    }
}