using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapayaX2.Database;

namespace PapayaX2.Helpers
{
    public class LogHelper
    {
        public string Username { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string ClientIP { get; set; }

        public void Log(string operation, string description)
        {
            PapayaEntities db = new PapayaEntities();

            rs_syslog syslog = new rs_syslog();
            syslog.Action = Action;
            syslog.Controller = Controller;
            syslog.Operation = operation;
            syslog.Description = description;
            syslog.User = Username;
            syslog.ClientIP = ClientIP;
            syslog.LogTime = DateTime.Now;

            db.rs_syslog.Add(syslog);
            db.SaveChanges();
        }
    }
}