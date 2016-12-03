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
                        "<script src=\"https://cdnjs.cloudflare.com/ajax/libs/react/0.14.6/react.min.js\" type=\"text/javascript\"></script>" +
                        "<script src=\"https://cdnjs.cloudflare.com/ajax/libs/react/0.14.6/react-dom.min.js\" type=\"text/javascript\"></script>" +
                        "<script src=\"/scripts/KookboxClient.js\" type=\"text/javascript\"></script>" +
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
