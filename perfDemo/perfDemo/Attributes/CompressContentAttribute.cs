using System.Web;
using System.Web.Mvc;

namespace perfDemo.Attributes
{
    /// <summary>
    /// Attribute that can be added to controller methods to force content
    /// to be GZip encoded if the client supports it
    /// </summary>
    public class CompressContentAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Adds gzip compression to the result of an action method
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GZipContent();
        }

        /// <summary>
        /// Determines if GZip is supported
        /// </summary>
        /// <returns></returns>
        public static bool IsGZipSupported()
        {
            var acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

            return !string.IsNullOrEmpty(acceptEncoding) &&
                   (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate"));
        }

        /// <summary>
        /// Sets up the current page or handler to use GZip through a Response.Filter
        /// IMPORTANT:  
        /// You have to call this method before any output is generated!
        /// </summary>
        public static void GZipContent()
        {
            var response = HttpContext.Current.Response;

            if (IsGZipSupported())
            {
                var acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

                if (acceptEncoding.Contains("gzip"))
                {
                    response.Filter = new System.IO.Compression.GZipStream(response.Filter,
                                                System.IO.Compression.CompressionMode.Compress);
                    response.Headers.Remove("Content-Encoding");
                    response.AppendHeader("Content-Encoding", "gzip");
                }
                else
                {
                    response.Filter = new System.IO.Compression.DeflateStream(response.Filter,
                                                System.IO.Compression.CompressionMode.Compress);
                    response.Headers.Remove("Content-Encoding");
                    response.AppendHeader("Content-Encoding", "deflate");
                }


            }

            // Allow proxy servers to cache encoded and unencoded versions separately
            response.AppendHeader("Vary", "Content-Encoding");
        }
    }
}