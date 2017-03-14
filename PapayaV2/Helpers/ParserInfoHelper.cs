using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapayaX2.Helpers
{
    public class ParserInfoHelper
    {
        public string Summary { get; set; }

        public List<string> Error { get; set; }

        public List<string> Warning { get; set; }

        public List<string> Info { get; set; }

        public ParserInfoHelper()
        {
            Error = new List<string>();
            Warning = new List<string>();
            Info = new List<string>();
            Summary = string.Empty;
        }

        public string Display()
        {
            //string Script = "<script type=\"text/javascript\">"
            //    + "$(document).ready(function() {"
            //    + "$(\".tab_content\").hide();"
            //    + "$(\"ul.tabs li:first\").addClass(\"active\").show();"
            //    + "$(\".tab_content:first\").show();"
            //    + "$(\"ul.tabs li\").click(function() {"
            //    +    "$(\"ul.tabs li\").removeClass(\"active\");"
            //    +    "$(this).addClass(\"active\");"
            //    +    "$(\".tab_content\").hide();"
            //    +    "var activeTab = $(this).find(\"a\").attr(\"href\");"
            //    +    "$(activeTab).fadeIn();"
            //    +    "return false;"
            //    +     "});"
            //    + "});"
            //    + "</script>";

            int numInfo = 0;
            int numError = 0;
            int numWarning = 0;

            string error_HTML = string.Empty;
            if (this.Error != null)
            {
                if (this.Error.Count > 0)
                {
                    numError = this.Error.Count;
                    //error_HTML += "<ul>";
                    foreach (string err in this.Error)
                    {
                        //error_HTML += "<li style=\"color:red;\">" + err + "</li>";
                        error_HTML += err + "<br>";
                    }
                    //error_HTML += "</ul>";
                }
                else
                {
                    error_HTML = "No error found in parsing process !";
                }
            }
            else
            {
                error_HTML = "No error found in parsing process !";
            }


            string warning_HTML = string.Empty;
            if (this.Warning != null)
            {
                if (this.Warning.Count > 0)
                {
                    numWarning = this.Warning.Count;
                    //warning_HTML += "<ul>";
                    foreach (string warning in this.Warning)
                    {
                        //warning_HTML += "<li style=\"color:orange;\">" + warning + "</li>";
                        warning_HTML += warning + "<br>";
                    }
                    //warning_HTML += "</ul>";
                }
                else
                {
                    warning_HTML = "No warning found in parsing process !";
                }
            }
            else
            {
                warning_HTML = "No warning found in parsing process !";
            }

            string info_HTML = string.Empty;
            if (this.Info != null)
            {
                if (this.Info.Count > 0)
                {
                    numInfo = this.Info.Count;
                    //info_HTML += "<ul>";
                    foreach (string info in this.Info)
                    {
                        //info_HTML += "<li style=\"color:blue;\">" + info + "</li>";
                        info_HTML += info + "<br>";
                    }
                    //info_HTML += "</ul>";
                }
                else
                {
                    info_HTML = "No relevant information found in parsing process !";
                }
            }
            else
            {
                info_HTML = "No relevant information found in parsing process !";
            }

            //string tabHeader_HTML = "<ul class=\"tabs\">"
            //    + "<li><a href=\"#tab_summary\">Summary</a></li>"
            //    + "<li><a href=\"#tab_info\">Info(" + numInfo + ")</a></li>"
            //    + "<li><a href=\"#tab_error\">Error(" + numError + ")</a></li>"
            //    + "<li><a href=\"#tab_warning\">Warning(" + numWarning + ")</a></li>"
            //    + "</ul>";

            string tabHeader_HTML = "<ul id=\"settings-tabs\" style=\"width:690px;margin:1em 0px 0px 0px;padding:5px 0px 0px 0px\">"
                + "<li class=\"selected\" style=\"width:100px\"><a href=\"#summary\">Summary</a></li>"
                + "<li style=\"width:100px\"><a href=\"#info\">Info(" + numInfo + ")</a></li>"
                + "<li style=\"width:100px\"><a href=\"#error\">Error(" + numError + ")</a></li>"
                + "<li style=\"width:100px\"><a href=\"#warning\">Warning(" + numWarning + ")</a></li>"
                + "</ul>";

            //string tabContainer_HTML = "<div class=\"tab_container\">"
            //    + "<div id=\"tab_summary\" class=\"tab_content\">"
            //    + this.Summary
            //    + "</div>"
            //    + "<div id=\"tab_info\" class=\"tab_content\">"
            //    + info_HTML
            //    + "</div>"
            //    + "<div id=\"tab_error\" class=\"tab_content\">"
            //    + error_HTML
            //    + "</div>"
            //    + "<div id=\"tab_warning\" class=\"tab_content\">"
            //    + warning_HTML
            //    + "</div>"
            //    + "</div>";
            string tabContent_HTML = "<div id=\"settings-tabs-content\" style=\"height:200px;width:690px;overflow-x:hidden;overflow-y:auto;color:#0C3E61\">"
                + "<div id=\"settings-tabs-content-container\" style=\"width:2800px\">"
                + "<div id=\"summary\" style=\"width:700px\">"
                + "<p style=\"padding:0px 0px 0px 10px\">" + this.Summary + "</p>"
                + "</div>"
                + "<div id=\"info\" style=\"width:700px\">"
                + "<p style=\"padding:0px 0px 0px 10px\">" + info_HTML + "</p>"
                + "</div>"
                + "<div id=\"error\" style=\"width:700px\">"
                + "<p style=\"padding:0px 0px 0px 10px\">" + error_HTML + "</p>"
                + "</div>"
                + "<div id=\"warning\" style=\"width:700px\">"
                + "<p style=\"padding:0px 0px 0px 10px\">" + warning_HTML + "</p>"
                + "</div>"
                + "</div>"
                + "</div>";

            string prefix = "<div id=\"lightbox-panel\" style=\"width:700px;top:150px;left:38%\">";
            string header = "<h3 style=\"padding: 0px 0px 0px 10px;margin: 0px\">Information</h3><a id=\"close-panel\" href=\"#\" class=\"close\" style=\"left:680px\"> </a>";
            string suffix = "<p align=\"right\" style=\"position:relative;top:8px;\"><a id=\"ok-close-panel\" href=\"#\" class=\"others\" style=\"margin-right:5px;\">OK</a></p></div></div><div id=\"lightbox\"></div>";

            return prefix + header + "<div class=\"content\" style=\"padding:0px 5px 0px 5px;height:303px\">" + tabHeader_HTML + tabContent_HTML + suffix;
        }

        public void AddError(string err)
        {
            this.Error.Add(err);
        }

        public void AddWarning(string warn)
        {
            this.Warning.Add(warn);
        }

        public void AddInfo(string info)
        {
            this.Info.Add(info);
        }

    }
}