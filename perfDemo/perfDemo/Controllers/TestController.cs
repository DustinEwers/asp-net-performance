using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Configuration;
using System.Web.SessionState;
using perfDemo.Attributes;
using StackExchange.Profiling;

namespace perfDemo.Controllers
{
    //[SessionState(SessionStateBehavior.Disabled)]
    public class TestController : Controller
    {
        private const int NumberOfWorkCycles = 5;
        private const int SleepTime = 250;
        
        public ActionResult TestAsync()
        {

            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Making Calls"))
            {
                using (profiler.Step("Async Work"))
                {
                    AsyncWork(SleepTime);
                }

                using (profiler.Step("Sync Work"))
                {
                    SyncWork(SleepTime);
                }
            }

            return View();
        }

        private void AsyncWork(int seconds)
        {
            var work = new List<Task>();

            for (var i = 0; i < NumberOfWorkCycles; i++)
            {
                var task = Task.Run(() => Thread.Sleep(seconds));
                work.Add(task);
            }

            Task.WaitAll(work.ToArray());
        }

        private void SyncWork(int seconds)
        {
            for (var i = 0; i < NumberOfWorkCycles; i++)
            {
                Thread.Sleep(seconds);
            }
        }
        
        public ActionResult SessionLockTest()
        {
            return View();
        }

        [NoCache]
        [HttpPost]
        public JsonResult SomeWork()
        {
            Session["foo"] = "bar";

            Thread.Sleep(500);
            return Json(new {Message = "Boom!"});
        }
        

        #region "Serializer Test"
        // GET: Test
        //public ActionResult TestSerializer()
        //{

        //    return View();
        //}

        //public class Foo
        //{
        //    public string Name { get; set; }
        //    public string Quest { get; set; }
        //    public string FavoriteColor { get; set; }

        //    public List<Bar> Bars { get; set; }
        //}

        //public class Bar
        //{
        //    public string Description { get; set; }
        //    public string Color { get; set; }
        //    public Foo Foo { get; set; }
        //}

        //public class ComplexEntity
        //{
        //    public List<Foo> Foos { get; set; }
        //    public List<Bar> Bars { get; set; }
        //}
        #endregion

        public enum AsyncValue
        {
            Async, 
            Sync
        }
    }
}