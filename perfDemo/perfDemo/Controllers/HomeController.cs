using System.Threading;
using System.Web.Mvc;
using StackExchange.Profiling;

namespace perfDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Doing some stuff"))
            {
                using (profiler.Step("Arbitrary Demo Thing 1"))
                {
                    Thread.Sleep(100);
                }
                using (profiler.Step("Some more arbitrary stepper code"))
                {
                    Thread.Sleep(250);
                }
            }
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}