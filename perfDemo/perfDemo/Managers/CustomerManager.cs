
using System.Data.Entity;
using System.Linq;
using perfDemo.Models;

namespace perfDemo.Managers
{
    public interface ICustomerManager
    {
        Customer GetCustomer(int id);
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
        
        public Customer GetCustomer(int id)
        {
            return _dbContext.Set<Customer>().FirstOrDefault(x => x.Id == id);
        }
    }
}