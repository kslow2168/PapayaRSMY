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

namespace PapayaX2.Controllers
{
    public class ModuleController : Controller
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

            ViewBag.ModuleName = db.rs_module.FirstOrDefault(s => s.Controller == currentController).Name;

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
        // GET: /Module/

        public ActionResult Index()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                var rs_module = db.rs_module.Include("rs_module2").OrderBy(m => m.rs_module2.Name).ThenBy(m => m.ModuleOrder);
                return View(rs_module.ToList());
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                ViewBag.search = search;

                if (search != null && search.Length > 0)
                {
  
                    List<rs_module> rs_module = new List<rs_module>();

                    foreach (var item in search.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        rs_module.AddRange(db.rs_module.Where(s => s.Name.Contains(item) ||
                                                    s.rs_module2.Name.Contains(item) ||
                                                    s.Controller.Contains(item)
                                                    ));
                    }

                    return View(rs_module.OrderBy(m => m.rs_module2 != null ? m.rs_module2.Name : "").ThenBy(m => m.ModuleOrder).Distinct());
                }

                var all_rs_module = db.rs_module.Include("rs_module2").OrderBy(m => m.rs_module2.Name).ThenBy(m => m.ModuleOrder);
                return View(all_rs_module.ToList());
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /Module/Details/5

        public ActionResult Details(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_module rs_module = db.rs_module.Single(s => s.ModuleId == id);
                return View(rs_module);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /Module/Create

        public ActionResult Create()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                ViewBag.ParentModuleId = new SelectList(db.rs_module.Where(m => m.FlagActive == true).OrderBy(m => m.Name), "ModuleId", "Name");
                ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username");
                ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username");
                return View();
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /Module/Create

        [HttpPost]
        public ActionResult Create(rs_module rs_module)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_module current = db.rs_module.Where(m => m.Name == rs_module.Name && m.Controller == rs_module.Controller).SingleOrDefault();
                    if (current != null)
                    {
                        TempData["Notification"] =NotificationHelper.Error("Module already exist.");
                    }
                    else
                    {
                        try
                        {
                            rs_module.UserEntry = User.Identity.Name;
                            rs_module.DateEntry = DateTime.Now;

                            db.rs_module.Add(rs_module);
                            db.SaveChanges();

                            Logger.Log("Add", "New Module [Name:" + rs_module.Name + "]");

                            TempData["Notification"] = NotificationHelper.Inform("New Module [Name:" + rs_module.Name + "]");
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["Notification"] =NotificationHelper.Error(ex.Message);
                        }
                    }
                }

                ViewBag.ParentModuleId = new SelectList(db.rs_module.Where(m => m.FlagActive == true).OrderBy(m => m.Name), "ModuleId", "Name", rs_module.ParentModuleId);
                ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username", rs_module.UserEntry);
                ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username", rs_module.UserUpdate);
                return View(rs_module);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /Module/Edit/5

        public ActionResult Edit(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_module rs_module = db.rs_module.Single(s => s.ModuleId == id);
                ViewBag.ParentModuleId = new SelectList(db.rs_module.Where(m => m.FlagActive == true).OrderBy(m => m.Name), "ModuleId", "Name", rs_module.ParentModuleId);
                ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username", rs_module.UserEntry);
                ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username", rs_module.UserUpdate);
                return View(rs_module);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /Module/Edit/5

        [HttpPost]
        public ActionResult Edit(rs_module rs_module)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_module current = db.rs_module.SingleOrDefault(m => m.Name == rs_module.Name
                                                                    && m.Controller == rs_module.Controller
                                                                    && m.ModuleId != rs_module.ModuleId);
                    if (current != null)
                    {
                        TempData["Notification"] =NotificationHelper.Error("Module already exist.");
                    }
                    else
                    {
                        try
                        {
                            rs_module.UserUpdate = User.Identity.Name;
                            rs_module.DateUpdate = DateTime.Now;

                            db.Entry(rs_module).State = EntityState.Modified;

                            //System.Data.Objects.ObjectStateEntry stateEntry = null;
                            //if (db.ObjectStateManager.TryGetObjectStateEntry(db.CreateEntityKey("rs_module", rs_module), out stateEntry))
                            //{
                            //    // object is attached                                
                            //    db.rs_module.ApplyCurrentValues(rs_module);
                            //}
                            //else
                            //{
                            //    db.rs_module.Attach(rs_module);
                            //    db.ObjectStateManager.ChangeObjectState(rs_module, EntityState.Modified);
                            //}

                            db.SaveChanges();

                            Logger.Log("Edit", "Edit Module [ID:" + rs_module.ModuleId + ", Name:" + rs_module.Name + "]");

                            TempData["Notification"] = NotificationHelper.Inform("Edit Module [ID:" + rs_module.ModuleId + ", Name:" + rs_module.Name + "]");
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["Notification"] =NotificationHelper.Error(ex.Message);
                        }

                    }
                }

                ViewBag.ParentModuleId = new SelectList(db.rs_module.Where(m => m.FlagActive == true).OrderBy(m => m.Name), "ModuleId", "Name", rs_module.ParentModuleId);
                ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username", rs_module.UserEntry);
                ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username", rs_module.UserUpdate);
                return View(rs_module);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /Module/Delete/5

        public ActionResult Delete(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_module rs_module = db.rs_module.Single(s => s.ModuleId == id);
                return View(rs_module);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /Module/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                try
                {
                    rs_module rs_module = db.rs_module.Single(s => s.ModuleId == id);
                    Logger.Log("Delete", "Delete Module [ID:" + rs_module.ModuleId + ", Name:" + rs_module.Name + "]");
                    List<rs_action> rs_action = db.rs_action.Where(m => m.ModuleId == id).ToList();
                    foreach (rs_action action in rs_action)
                    { // delete all the action within this module
                        Logger.Log("Delete", "Delete Action [ID:" + action.ActionId + ", Name:" + action.Name + "]");
                        List<rs_useracl> rs_useracl = db.rs_useracl.Where(m => m.ActionId == action.ActionId).ToList();
                        foreach (rs_useracl useracl in rs_useracl)
                        {  // delete all the acl within this module
                            Logger.Log("Delete", "Delete ACL [ID:" + useracl.UserAclId + ", Action:" + useracl.rs_action.Name + ", Module:" + useracl.rs_action.rs_module.Name + "]");
                            db.rs_useracl.Remove(useracl);
                            db.SaveChanges();
                        }
                        db.rs_action.Remove(action);
                        db.SaveChanges();
                    }

                    db.rs_module.Remove(rs_module);
                    db.SaveChanges();

                    TempData["Notification"] = NotificationHelper.Inform("Delete Module [ID:" + rs_module.ModuleId + ", Name:" + rs_module.Name + "]");
                }
                catch (Exception ex)
                {
                    TempData["Notification"] = NotificationHelper.Error(ex.Message);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //public System.Data.Objects.ObjectStateEntry entry { get; set; }
    }
}
