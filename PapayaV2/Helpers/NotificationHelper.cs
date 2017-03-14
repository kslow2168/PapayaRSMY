using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapayaX2.Helpers
{
    public class NotificationHelper
    {
        private static string prefix = "<div id=\"lightbox-panel\">";
        private static string content = "<div class=\"content\"><p style=\"color:black;padding:12px;font-size:13px\">";
        private static string suffix = "</p><p align=\"right\" style=\"position:relative;top:8px;\"><a id=\"ok-close-panel\" href=\"#\" class=\"others\" style=\"margin-right:5px;\">OK</a></p></div></div><div id=\"lightbox\"></div>";
        //<div id="lightbox-panel">  
        //    <h2>Lightbox Panel</h2>
        //    <div>
        //       <p id="notif">@Html.Raw(ViewBag.Notification)</p>  
        //       <p align="center">  
        //         <a id="close-panel" href="#" class="others">Close this window</a>  
        //       </p>
        //    </div>
        //</div> 
        //<div id="lightbox"></div>

        public static string Error(string message)
        {
            //string prefix = "<div style=\"background-color:red;color:white;font-size:11pt;border:1px solid black;padding:5px 5px 5px 5px;\">";
            //string suffix = "</div>";
            string header = "<h3 style=\"padding: 0px 0px 0px 10px;margin: 0px\">Error</h3><a id=\"close-panel\" href=\"#\" class=\"close\"> </a>";

            return prefix + header + content + message + suffix;
        }

        public static string Warning(string message)
        {
            //string prefix = "<div style=\"background-color:yellow;color:black;font-size:11pt;border:1px solid red;padding:5px 5px 5px 5px;\">";
            //string suffix = "</div>";
            string header = "<h3 style=\"padding: 0px 0px 0px 10px;margin: 0px\">Warning</h3><a id=\"close-panel\" href=\"#\" class=\"close\"> </a>";

            return prefix + header + content + message + suffix;
        }

        public static string Inform(string message)
        {
            //string prefix = "<div style=\"background-color:#000036;color:white;font-size:11pt;border:1px solid blue;padding:5px 5px 5px 5px;\">";
            //string suffix = "</div>";
            string header = "<h3 style=\"padding: 0px 0px 0px 10px;margin: 0px\">Information</h3><a id=\"close-panel\" href=\"#\" class=\"close\"> </a>";

            return prefix + header + content + message + suffix;
        }
    }
}