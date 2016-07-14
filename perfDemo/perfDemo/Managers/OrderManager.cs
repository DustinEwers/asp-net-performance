using System.Collections.Generic;
using System.Linq;
using perfDemo.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace perfDemo.Managers
{
    public interface IOrderManager
    {
        List<Order> GetOrdersForCustomer(int customerId);
        Task<List<Order>> GetOrdersForCustomerAsync(int customerId);
    }

    public class OrderManager : IOrderManager
    {
        private readonly DbContext _dbContext;
        
        public OrderManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public OrderManager()
        {
            _dbContext = new ApplicationDbContext();
        }

        // This is what *not* to do
        public List<Order> GetOrdersForCustomer(int customerId)
        {
            return _dbContext.Set<Order>()
                .Where(x => x.CustomerId == customerId)
                .Include(x => x.Customer)
                .Include(x => x.OrderLines)
                .Include(x => x.OrderLines.Select(y => y.Product))
                .Include(x => x.OrderLines.Select(y => y.Product.ProductOptions))
                .ToList();
        }

        // This is better. Generates less SQL and runs faster. 
        public async Task<List<Order>> GetOrdersForCustomerAsync(int customerId)
        {
            var orders =  await _dbContext.Set<Order>()
                .Where(x => x.CustomerId == customerId)
                .Select(x =>
                    new
                    {
                        Shipping = x.Shipping,
                        Tax = x.Tax,
                        SubTotal = x.SubTotal,
                        OrderLines = x.OrderLines.Select(y => new
                        {
                            Price = y.Price,
                            Product = new {Name = y.Product.Name}
                        })
                    }
                ).ToListAsync();
                                
                // You can also just map this directly to a view model or other non-entity object 
                return orders.Select(x =>
                    new Order
                    {
                        Shipping = x.Shipping,
                        Tax = x.Tax,
                        SubTotal = x.SubTotal,
                        OrderLines = x.OrderLines.Select(y => new OrderLine
                        {
                            Price = y.Price,
                            Product = new Product { Name = y.Product.Name }
                        }).ToList()
                    }
                )
                .ToList();
        }
    }
}