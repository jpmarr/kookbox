using Microsoft.AspNetCore.Mvc;

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
            return Content(
                "<html>" +
                    "<head>" +
                        "<script src=\"https://unpkg.com/react@15/dist/react.min.js\" type=\"text/javascript\"></script>" +
                        "<script src=\"https://unpkg.com/react-dom@15/dist/react-dom.min.js\" type=\"text/javascript\"></script>" +
                        "<script src=\"/scripts/app.js\" type=\"text/javascript\"></script>" +
                    "</head>" +
                    "<body>" +
                        "<div id=\"app\"></div>" +
                        "<script>" +
                            "ReactDOM.render(React.createElement(KookboxApp, { test: \"Hello\" }, null), document.getElementById(\"app\"));" +
                        "</script>" +
                    "</body>" +
                "</html>", "text/html");
        }
    }
}
