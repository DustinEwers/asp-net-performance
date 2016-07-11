using System.Collections.Generic;
using System.Web.Mvc;
using perfDemo.Managers;
using perfDemo.Models;

namespace perfDemo.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderManager _manager;
        private readonly ICustomerManager _customerManager;

        public OrderController(IOrderManager manager, ICustomerManager customerManager)
        {
            _manager = manager;
            _customerManager = customerManager;
        }

        public OrderController()
        {
            _customerManager = new CustomerManager();
            _manager = new OrderManager();
        }

        // GET: Order
        public ActionResult Index()
        {
            var vm = new OrderSearchCriteriaViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(OrderSearchCriteriaViewModel criteria)
        {
            criteria.Customer = _customerManager.GetCustomer(criteria.CustomerId);

            var items = _manager.GetOrdersForCustomer(criteria.CustomerId);
            criteria.Orders = items;

            //criteria.Customer = _customerManager.GetCustomer(criteria.CustomerId);

            return View(criteria);
        }
    }

    public class OrderSearchCriteriaViewModel
    {
        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem> CustomerOptions => new List<SelectListItem>
        {
            new SelectListItem {Text = "Rose Tyler", Value = "1"},
            new SelectListItem {Text = "Amy Pond", Value = "2"}
        };

        public List<Order> Orders { get; set; }

        public Customer Customer { get; set; }
    }
}