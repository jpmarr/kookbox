using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Net.Http.Headers;

namespace kookbox.http
{
    /// <summary>
    /// Main controller for serving the UI. All comms from UI back is web socket based so this is 
    /// just basically serving out the static scaffolding of the app
    /// </summary>
    public class AppController : Controller
    {
        [HttpGet("/")]
        public ActionResult Default()
        {
            return Content($"<html><head></head><body><p>Hello There from {Directory.GetCurrentDirectory()}</p><img src='images/_MG_4167.jpg' /></body></html>", "text/html");
        }
    }
}
