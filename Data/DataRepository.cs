using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using soapApi.Models;
using soapApi.ViewModels;

namespace soapApi.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly DataContext _context;
        public DataRepository(DataContext context)
        {
            _context = context;

        }

        public async Task<List<Value>> GetAllValuesAsync()
        {
            return await _context.Values.ToListAsync();
        }

        public async Task<Value> GetValueAsync(int id)
        {
           return await _context.Values.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}