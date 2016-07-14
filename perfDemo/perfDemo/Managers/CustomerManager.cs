
using System.Data.Entity;
using System.Linq;
using perfDemo.Models;
using System.Threading.Tasks;

namespace perfDemo.Managers
{
    public interface ICustomerManager
    {
        Task<Customer> GetCustomerAsync(int id);
    }

    public class CustomerManager: ICustomerManager
    {
        private readonly DbContext _dbContext;

        public CustomerManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CustomerManager()
        {
            _dbContext = new ApplicationDbContext();
        }
        
        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}