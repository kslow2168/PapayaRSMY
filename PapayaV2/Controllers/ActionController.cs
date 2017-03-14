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
    public class ActionController : Controller
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
        // GET: /Action/

        public ActionResult Index()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                var rs_action = db.rs_action.Include("rs_module").OrderBy(m => m.ModuleId);
                return View(rs_action.ToList());
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
                    List<rs_action> rs_action = new List<rs_action>();

                    foreach (var item in search.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        rs_action.AddRange(db.rs_action.Where(s => s.Name.Contains(item) ||
                                                    s.rs_module.Name.Contains(item)
                                                    ));
                    }

                    return View(rs_action.Distinct());
                }

                var all_rs_action = db.rs_action.Include("rs_module").OrderBy(m => m.ModuleId);
                return View(all_rs_action.ToList());
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /Action/Details/5

        public ActionResult Details(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_action rs_action = db.rs_action.Single(s => s.ActionId == id);
                return View(rs_action);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /Action/Create

        public ActionResult Create()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                ViewBag.ModuleId = new SelectList(db.rs_module.Where(m => m.FlagActive == true).OrderBy(m => m.Name), "ModuleId", "Name");
                //ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username");
                //ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username");
                return View();
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /Action/Create

        [HttpPost]
        public ActionResult Create(rs_action rs_action)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_action current = db.rs_action.Where(m => m.ModuleId == rs_action.ModuleId && m.Name.ToLower() == rs_action.Name.ToLower()).SingleOrDefault();
                    if (current != null)
                    {
                        TempData["Notification"] =NotificationHelper.Error("Action already exist.");
                    }
                    else
                    {
                        try
                        {
                            rs_action.UserEntry = User.Identity.Name;
                            rs_action.DateEntry = DateTime.Now;

                            db.rs_action.Add(rs_action);
                            db.SaveChanges();

                            Logger.Log("Add", "New Action [" + rs_action.Name + "]");

                            TempData["Notification"] = NotificationHelper.Inform("New Action [" + rs_action.Name + "]");
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["Notification"] =NotificationHelper.Error(ex.Message);
                        }
                    }
                }

                ViewBag.ModuleId = new SelectList(db.rs_module.Where(m => m.FlagActive == true).OrderBy(m => m.Name), "ModuleId", "Name", rs_action.ModuleId);
                //ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username", rs_action.UserEntry);
                //ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username", rs_action.UserUpdate);
                return View(rs_action);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }

        }

        //
        // GET: /Action/Edit/5

        public ActionResult Edit(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_action rs_action = db.rs_action.Single(s => s.ActionId == id);
                ViewBag.ModuleId = new SelectList(db.rs_module.Where(m => m.FlagActive == true).OrderBy(m => m.Name), "ModuleId", "Name", rs_action.ModuleId);
                //ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username", rs_action.UserEntry);
                //ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username", rs_action.UserUpdate);
                return View(rs_action);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /Action/Edit/5

        [HttpPost]
        public ActionResult Edit(rs_action rs_action)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_action current = db.rs_action.SingleOrDefault(m => m.ModuleId == rs_action.ModuleId &&
                                                                m.Name.ToLower() == rs_action.Name.ToLower() &&
                                                                m.ActionId != rs_action.ActionId
                                                                );
                    if (current != null)
                    {
                        TempData["Notification"] =NotificationHelper.Error("Action already exist.");
                    }
                    else
                    {
                        try
                        {
                            rs_action.UserUpdate = User.Identity.Name;
                            rs_action.DateUpdate = DateTime.Now;

                            db.rs_action.Attach(rs_action);
                            db.Entry(rs_action).State = EntityState.Modified;
                            //db.ObjectStateManager.ChangeObjectState(rs_action, EntityState.Modified);
                            db.SaveChanges();

                            Logger.Log("Edit", "Edit Action - [ID:" + rs_action.ActionId + ", Name:" + rs_action.Name + "]");

                            TempData["Notification"] = NotificationHelper.Inform("Edit Action - [ID:" + rs_action.ActionId + ", Name:" + rs_action.Name + "]");
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["Notification"] =NotificationHelper.Error(ex.Message);
                        }

                    }
                }

                ViewBag.ModuleId = new SelectList(db.rs_module.Where(m => m.FlagActive == true).OrderBy(m => m.Name), "ModuleId", "Name", rs_action.ModuleId);
                //ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username", rs_action.UserEntry);
                //ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username", rs_action.UserUpdate);
                return View(rs_action);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }

        }

        //
        // GET: /Action/Delete/5

        public ActionResult Delete(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_action rs_action = db.rs_action.Single(s => s.ActionId == id);
                return View(rs_action);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /Action/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                try
                {
                    rs_action rs_action = db.rs_action.Single(s => s.ActionId == id);

                    Logger.Log("Delete", "Delete Action - [ID:" + rs_action.ActionId + ", Name:" + rs_action.Name + "]");
                    TempData["Notification"] = NotificationHelper.Inform("Delete Action - [ID:" + rs_action.ActionId + ", Name:" + rs_action.Name + "]");

                    db.rs_action.Remove(rs_action);
                    db.SaveChanges();
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
    }
}
