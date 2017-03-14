using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PapayaX2.Database;
using PapayaX2.Helpers;
using PapayaX2.Models;
using CookComputing.XmlRpc;
using System.Text;
using System.Diagnostics;

namespace PapayaX2.Controllers
{
    public class ChangePasswordController : Controller
    {
        private PapayaEntities db = new PapayaEntities();

        private string currentAction = string.Empty;

        private string currentController = string.Empty;

        private LogHelper Logger = new LogHelper();

        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            MenuModel model = new MenuModel();
            int groupId = (User.Identity is FormsIdentity) ? int.Parse(((FormsIdentity)User.Identity).Ticket.UserData) : 0;
            model.acl = db.rs_useracl.Where(e => e.GroupId == groupId);
            model.module = db.rs_module.ToList();
            ViewData["MenuModel"] = model;
            //var systemName = db.se_settings.SingleOrDefault(s => s.Key == "SystemName");
            //ViewBag.SystemName = systemName != null ? systemName.Value : "";
            //ViewBag.Brand = new SelectList(db.se_brand.Where(s => s.FlagActive == true).OrderBy(s => s.Description), "BrandCode", "Description", (Request.Cookies["Brand"] == null) ? "" : Request.Cookies["Brand"].Value);

            currentAction = ctx.ActionDescriptor.ActionName;
            currentController = ctx.ActionDescriptor.ControllerDescriptor.ControllerName;

            Logger.Action = currentAction;
            Logger.Controller = currentController;
            Logger.Username = User.Identity.Name;
            Logger.ClientIP = Request.ServerVariables["REMOTE_ADDR"];

            if (!Request.IsAuthenticated)
            {
                ctx.Result = RedirectToAction("Login", "Home");
            }
        }

        //
        // GET: /ChangePassword/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ChangePasswordModel cp)
        {
            rs_user rs_user = new rs_user();

            try
            {
                string CurrentPassword = db.rs_user.Where(m => m.Username == User.Identity.Name).Single().Password;
                if (ModelState.IsValid)
                {
                    //if (Crypto.Hash(cp.OldPassword).Remove(32) == CurrentPassword)
                    if (UtilitiesHelper.Encrypt(cp.OldPassword) == CurrentPassword)
                    {
                        rs_user baru = new rs_user();
                        baru = db.rs_user.Where(m => m.Username == User.Identity.Name).Single();

                        //baru.Password = Crypto.Hash(cp.NewPassword).Remove(32);
                        baru.Password = UtilitiesHelper.Encrypt(cp.NewPassword);


                        db.Entry(baru).State = EntityState.Modified;
                        db.SaveChanges();

                        Logger.Log("Change Password", "User Change Password [" + baru.Username + "]");

                        TempData["Notification"] =NotificationHelper.Inform("You have successfully changed your password.");
                    }
                    else
                    {
                        TempData["Notification"] =NotificationHelper.Error("Incorrect old password!");
                    }
                }
            }
            catch (Exception)
            {
                TempData["Notification"] =NotificationHelper.Error("Change password failed!");
            }

            return View(cp);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}