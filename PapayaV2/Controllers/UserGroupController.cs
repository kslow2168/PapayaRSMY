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
    public class UserGroupController : Controller
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
        // GET: /UserGroup/

        public ActionResult Index()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                return View(db.rs_user_group.ToList());
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
                    List<rs_user_group> rs_user_group = new List<rs_user_group>();

                    foreach (var item in search.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        rs_user_group.AddRange(db.rs_user_group.Where(s => s.Name.Contains(item) ||
                                                        s.Description.Contains(item)
                                                    ));
                    }

                    return View(rs_user_group.Distinct());
                }

                return View(db.rs_user_group.ToList());
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /UserGroup/Details/5

        public ActionResult Details(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_user_group rs_user_group = db.rs_user_group.Single(g => g.GroupId == id);
                return View(rs_user_group);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /UserGroup/Create

        public ActionResult Create()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                return View();
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /UserGroup/Create

        [HttpPost]
        public ActionResult Create(rs_user_group rs_user_group)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_user_group current = db.rs_user_group.Where(s => s.Name == rs_user_group.Name).SingleOrDefault();
                    if (current != null)
                    {
                        TempData["Notification"] =NotificationHelper.Error("User group already exist.");
                    }
                    else
                    {
                        try
                        {
                            rs_user_group.DateEntry = DateTime.Now;
                            rs_user_group.UserEntry = User.Identity.Name;

                            db.rs_user_group.Add(rs_user_group);
                            db.SaveChanges();

                            Logger.Log("Add", "New Group [Name: " + rs_user_group.Name + "]");

                            TempData["Notification"] = NotificationHelper.Inform("New Group [Name: " + rs_user_group.Name + "]");

                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["Notification"] =NotificationHelper.Error(ex.Message);
                        }
                    }
                }

                return View(rs_user_group);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /UserGroup/Edit/5

        public ActionResult Edit(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_user_group rs_user_group = db.rs_user_group.Single(s => s.GroupId == id);
                return View(rs_user_group);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /UserGroup/Edit/5

        [HttpPost]
        public ActionResult Edit(rs_user_group rs_user_group)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_user_group current = db.rs_user_group.SingleOrDefault(s => s.Name == rs_user_group.Name && s.GroupId != rs_user_group.GroupId);
                    if (current != null)
                    {
                        TempData["Notification"] =NotificationHelper.Error("User group already exist.");
                    }
                    else
                    {
                        try
                        {
                            rs_user_group.UserUpdate = User.Identity.Name;
                            rs_user_group.DateUpdate = DateTime.Now;


                            db.Entry(rs_user_group).State = EntityState.Modified;
                            db.SaveChanges();

                            //db.rs_user_group.Attach(rs_user_group);
                            //db.ObjectStateManager.ChangeObjectState(rs_user_group, EntityState.Modified);
                            //db.SaveChanges();

                            Logger.Log("Edit", "Edit Group [ID:" + rs_user_group.GroupId + "]");

                            TempData["Notification"] = NotificationHelper.Inform("Edit Group [ID:" + rs_user_group.GroupId + "]");

                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["Notification"] =NotificationHelper.Error(ex.Message);
                        }
                    }
                }

                return View(rs_user_group);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /UserGroup/Delete/5

        public ActionResult Delete(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_user_group rs_user_group = db.rs_user_group.Single(s => s.GroupId == id);
                return View(rs_user_group);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /UserGroup/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                try
                {
                    rs_user_group rs_user_group = db.rs_user_group.Single(s => s.GroupId == id);

                    Logger.Log("Delete", "Delete Group [ID:" + rs_user_group.GroupId + "]");
                    TempData["Notification"] = NotificationHelper.Inform("Delete Group [ID:" + rs_user_group.GroupId + "]");

                    db.rs_user_group.Remove(rs_user_group);
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

        public ActionResult Manage(int id, string name)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                ViewData["id"] = id;
                ViewData["name"] = name;

                AclManageModel aclmm = new AclManageModel();

                List<rs_action> acl = new List<rs_action>();
                List<rs_useracl> rs_useracl = db.rs_useracl.Where(s => s.GroupId == id).ToList();

                List<rs_action> rs_action = new List<rs_action>();

                if (User.Identity.Name.Equals("papaya"))
                {
                    rs_action = db.rs_action.OrderBy(s => s.ModuleId).ToList();
                }
                else
                {
                    int? userGroupID = db.rs_user.FirstOrDefault(s => s.Username == User.Identity.Name).GroupId;
                    var listOfActions = db.rs_useracl.Where(s => s.GroupId == userGroupID).Select(s => s.ActionId);
                    rs_action = db.rs_action.Where(s => listOfActions.Contains(s.ActionId)).OrderBy(s => s.ModuleId).ToList();
                }

                foreach (rs_action action in rs_action)
                {
                    acl.Add(action);
                }

                aclmm.user_acl = rs_useracl;
                aclmm.action = acl;

                //aclmm.user_acl = db.rs_useracl.Where(e => e.GroupId == id).ToList();


                return View(aclmm);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        [HttpPost]
        public ActionResult Manage(FormCollection fc)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                int group_id = Convert.ToInt32(fc["group_id"]);

                List<int> user_acl = db.rs_useracl.Where(e => e.GroupId == group_id).Select(e => e.UserAclId).ToList();
                foreach (var x in user_acl)
                {
                    rs_useracl deleted = db.rs_useracl.Where(e => e.UserAclId == x).SingleOrDefault();
                    db.rs_useracl.Remove(deleted);
                }

                if (fc["input"] != null)
                {
                    string[] action_ids = fc["input"].Split(',');
                    foreach (string action_id in action_ids)
                    {
                        rs_useracl rs_useracl = new rs_useracl();
                        rs_useracl.GroupId = group_id;
                        rs_useracl.ActionId = Convert.ToInt32(action_id);
                        rs_useracl.FlagActive = true;
                        rs_useracl.DateEntry = DateTime.Now;
                        rs_useracl.UserEntry = User.Identity.Name;

                        db.rs_useracl.Add(rs_useracl);
                    }
                }

                db.SaveChanges();

                TempData["Notification"] = NotificationHelper.Inform("ACL has been set for Group ID : " + group_id);
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