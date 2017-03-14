using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using PapayaX2.Database;

namespace PapayaX2.Helpers
{
    public class AclHelper
    {
        public static bool hasAccess(IPrincipal user, string action, string controller)
        {
            PapayaEntities db = new PapayaEntities();
            if (user.Identity.Name.Equals("papaya"))
            {
                return true;
            }
            else
            {
                int groupId = (user.Identity is FormsIdentity) ? int.Parse(((FormsIdentity)user.Identity).Ticket.UserData) : 0;
                rs_useracl acl = db.rs_useracl.FirstOrDefault(e => e.GroupId == groupId &&
                                                     e.rs_action.Name == action &&
                                                     e.FlagActive == true &&
                                                     e.rs_action.rs_module.Controller == controller);
                if (acl == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static int GetUserId(string username)
        {
            PapayaEntities db = new PapayaEntities();
            return db.rs_user.FirstOrDefault(e => e.Username == username).UserId;
        }

        public static bool IsAdmin(string username)
        {
            PapayaEntities db = new PapayaEntities();
            return db.rs_user.FirstOrDefault(e => e.Username == username).rs_user_group.Name.ToLower().Contains("admin");

        }

    }
}