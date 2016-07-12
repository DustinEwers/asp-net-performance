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
        

        #region "GZip Test"

        [HttpGet]
        public ActionResult GZipTest()
        {
            return View();
        }
        
        [HttpPost]
        [NoCache]
        [CompressContent]
        public JsonResult GZipCompression()
        {
            return Json(GetGzipTestResult());
        }

        [HttpPost]
        [NoCache]
        public JsonResult DefaultCompression()
        {
            return Json(GetGzipTestResult());
        }

        private object GetGzipTestResult()
        {
            return new
            {
                Author = "Steven Moffat",
                Keywords = "timey-wimey stuff, time",
                Misc = "I don\'t want to go. I\'m Dr. James McCrimmon from the township of Balamory. I\'ll tell you what, then: don\'t...step on any butterflies. What have butterflies ever done to you? I\'m the Doctor, I can save the world with a kettle and some string! And look! I\'m wearing a vegetable! I\'d call you a genius, except I\'m in the room. Allons-y! Oh, yes. Harmless is just the word: that\'s why I like it! Doesn\'t kill, doesn\'t wound, doesn\'t maim. But I\'ll tell you what it does do: it is very good at opening doors!\r\n\r\nGoodbye...my Sarah Jane! Don\'t you think she looks tired? There was a war. A Time War. The Last Great Time War. My people fought a race called the Daleks, for the sake of all creation. And they lost. We lost. Everyone lost. They\'re all gone now. My family. My friends. Even that sky. Allons-y! What? What?! WHAT?! River, you know my name. You whispered my name in my ear! There\'s only one reason I would ever tell anyone my name. There\'s only one time I could... Yeah? Well I\'m the Lord of Time. I don\'t want to go.\r\n\r\nYou can spend the rest of your life with me, but I can\'t spend the rest of mine with you. I have to live on. Alone. That\'s the curse of the Time Lords. Oh, yes. Harmless is just the word: that\'s why I like it! Doesn\'t kill, doesn\'t wound, doesn\'t maim. But I\'ll tell you what it does do: it is very good at opening doors! What? What?! WHAT?! There\'s something else I\'ve always wanted to say: Allons-y, Alonso! I\'m Dr. James McCrimmon from the township of Balamory. I\'m sorry. I\'m so sorry. River, you know my name. You whispered my name in my ear! There\'s only one reason I would ever tell anyone my name. There\'s only one time I could...\r\n\r\nOh, yes. Harmless is just the word: that\'s why I like it! Doesn\'t kill, doesn\'t wound, doesn\'t maim. But I\'ll tell you what it does do: it is very good at opening doors! Blimey, trying to make an Ood laugh... Aw, I wanted to be ginger! I\'ve never been ginger! And you, Rose Tyler! Fat lot of good you were! You gave up on me! Ooh, that\'s rude. Is that the sort of man I am now? Am I rude? Rude and not ginger. You need to get yourself a better dictionary. When you do, look up \'genocide\'. You\'ll find a little picture of me there, and the caption\'ll read \'Over my dead body\'.\r\n\r\nI\'m Dr. James McCrimmon from the township of Balamory. Sweet, maybe... Passionate, I suppose... But don\'t ever mistake that for nice. I\'m the Doctor, I can save the world with a kettle and some string! And look! I\'m wearing a vegetable! It is! It\'s the city of New New York! Strictly speaking, it\'s the fifteenth New York since the original, so that makes it New-New-New-New-New-New-New-New-New-New-New-New-New-New-New New York. I\'d call you a genius, except I\'m in the room. People assume that time is a strict progression of cause-and-effect... but actually, from a non-linear, non-subjective viewpoint, it\'s more like a big ball of wibbly-wobbly... timey-wimey... stuff.",
                Text = "People assume that time is a strict progression of cause to effect, but *actually* from a non-linear, non-subjective viewpoint - it\'s more like a big ball of wibbly wobbly... time-y wimey... stuff."
            };
        }
       
        #endregion

        public enum AsyncValue
        {
            Async, 
            Sync
        }
    }
}