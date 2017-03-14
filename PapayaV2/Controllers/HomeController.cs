using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PapayaX2.Helpers;
using PapayaX2.Models;
using PapayaX2.Database;

namespace PapayaX2.Controllers
{
    public class HomeController : Controller
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

            if (!Request.IsAuthenticated && currentAction != "Login")
            {
                ctx.Result = RedirectToAction("Login", "Home");
            }
            
        }

        //
        // GET: /Login/

        public ActionResult Index(String priv)
        {
            if (Request.IsAuthenticated)
            {
                var url = Request.Url;
                string[] path = url.Segments;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            if (Request.IsAuthenticated)
            {
                Logger.Log("Logout", "Logout user [" + User.Identity.Name + "]");
                FormsAuthentication.SignOut();

                if (Request.Cookies["ExpandedMenu"] != null)
                {
                    var c = new HttpCookie("ExpandedMenu");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }
            }

            return RedirectToAction("Login");
        }
        //
        // GET: /Login/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var url = Request.Url;

            if (ModelState.IsValid)
            {
                if (model.Username != null && model.Password != null)
                {
                    //string verifier = Crypto.Hash(model.Password,"sha256").Remove(32);
                    string verifier = UtilitiesHelper.Encrypt(model.Password);
                    rs_user loginUser = db.rs_user.FirstOrDefault(m => m.Username == model.Username && m.Password == verifier && m.FlagActive == true && m.IsBackEnd == true);
                    if (loginUser != null)
                    {
                        //FormsAuthentication.SetAuthCookie(loginUser.Username, false);

                        string userData = (loginUser.GroupId == null) ? "0" : loginUser.GroupId.ToString();

                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                            loginUser.Username,
                            DateTime.Now,
                            DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                            false,
                            userData);

                        // Encrypt the ticket.
                        string encTicket = FormsAuthentication.Encrypt(ticket);

                        // Create the cookie.
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                        //FormsAuthentication.SetAuthCookie(model.Username, true);
                        Logger.Username = model.Username;
                        Logger.Log("Login", "Logged in with user [" + model.Username + "]");

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Logger.Username = model.Username;
                        Logger.Log("Login", "Failed to login, user [" + model.Username + "]");
                        TempData["Notification"] =NotificationHelper.Error("Username and Password is incorrect");
                    }
                }
                else
                {
                    TempData["Notification"] =NotificationHelper.Warning("Please provide correct Username and Password");
                }

            }

            return View(model);
        }

        public ActionResult NotAuthenticated()
        {
            return View();
        }
    }
}