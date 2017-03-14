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
    public class UserController : Controller
    {
        
        private PapayaEntities db = new PapayaEntities();

        private string currentAction = string.Empty;

        private string currentController = string.Empty;

        private LogHelper Logger = new LogHelper();

        protected string UserType = "User";

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
        // GET: /User/

        public ActionResult Index()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                var rs_user = db.rs_user.Where(e => e.Username != "papaya" && e.UserType == UserType);
                return View(rs_user.ToList());
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
                    List<rs_user> rs_user = new List<rs_user>();

                    foreach (var item in search.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        rs_user.AddRange(db.rs_user.Where(s => s.Username != "papaya" && s.UserType == UserType &&
                                                    (
                                                        s.Username.Contains(item) ||
                                                        s.FullName.Contains(item) ||
                                                        s.MobileNumber.Contains(item) ||
                                                        s.Email.Contains(item)
                                                    )
                                                    ));
                    }

                    return View(rs_user.Distinct());
                }

                var all_rs_user = db.rs_user.Include("rs_company").Where(e => e.Username != "papaya" && e.UserType == UserType);
                return View(all_rs_user.ToList());
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_user rs_user = db.rs_user.Single(s => s.UserId == id);
                return View(rs_user);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                //ViewBag.CompanyId = new SelectList(db.rs_company.Where(c => c.FlagActive == true).OrderBy(c => c.Name), "CompanyId", "Name");
                ViewBag.GroupId = new SelectList(db.rs_user_group.Where(g => g.FlagActive == true).OrderBy(g => g.Name), "GroupId", "Name");
                //ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username");
                //ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username");
                rs_user user = new rs_user();
                user.UserType = UserType;
                return View(user);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(rs_user rs_user)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_user current = db.rs_user.SingleOrDefault(m => m.Username == rs_user.Username);
                    if (current != null)
                    {
                        TempData["Notification"] =NotificationHelper.Error("Username '" + rs_user.Username + "' already exist.");
                    }
                    else
                    {
                        try
                        {
                            rs_user.Password = UtilitiesHelper.Encrypt(rs_user.Password);
                            rs_user.IsBackEnd = true;
                            rs_user.UserEntry = User.Identity.Name;
                            rs_user.DateEntry = DateTime.Now;

                            db.rs_user.Add(rs_user);
                            db.SaveChanges();

                            Logger.Log("Add", "New User [Username: " + rs_user.Username + "]");

                            TempData["Notification"] = NotificationHelper.Inform("New User [Username: " + rs_user.Username + "]");
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["Notification"] =NotificationHelper.Error(ex.Message);
                        }
                    }
                }

                //ViewBag.CompanyId = new SelectList(db.rs_company.Where(c => c.FlagActive == true).OrderBy(c => c.Name), "CompanyId", "Name", rs_user.CompanyId);
                ViewBag.GroupId = new SelectList(db.rs_user_group.Where(g => g.FlagActive == true).OrderBy(g => g.Name), "GroupId", "Name", rs_user.GroupId);

                return View(rs_user);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_user rs_user = db.rs_user.Single(s => s.UserId == id);
                //ViewBag.CompanyId = new SelectList(db.rs_company.Where(c => c.FlagActive == true).OrderBy(c => c.Name), "CompanyId", "Name", rs_user.CompanyId);
                ViewBag.GroupId = new SelectList(db.rs_user_group.Where(g => g.FlagActive == true).OrderBy(g => g.Name), "GroupId", "Name", rs_user.GroupId);

                return View(rs_user);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(rs_user rs_user)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_user current = db.rs_user.SingleOrDefault(m => m.Username == rs_user.Username && m.UserId != rs_user.UserId);
                    if (current != null)
                    {
                        TempData["Notification"] =NotificationHelper.Error("Username '" + rs_user.Username + "' already exist.");
                    }
                    else
                    {
                        try
                        {
                            rs_user.UserUpdate = User.Identity.Name;
                            rs_user.DateUpdate = DateTime.Now;
                            rs_user.IsBackEnd = true;

                            db.Entry(rs_user).State = EntityState.Modified;
                            db.SaveChanges();

                            Logger.Log("Edit", "Edit User [ID:" + rs_user.UserId + "Username:" + rs_user.Username + "]");

                            TempData["Notification"] = NotificationHelper.Inform("Edit User [ID:" + rs_user.UserId + ", Username:" + rs_user.Username + "]");
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["Notification"] =NotificationHelper.Error(ex.Message);
                        }
                    }
                }
                //ViewBag.CompanyId = new SelectList(db.rs_company.Where(c => c.FlagActive == true).OrderBy(c => c.Name), "CompanyId", "Name", rs_user.CompanyId);
                ViewBag.GroupId = new SelectList(db.rs_user_group.Where(g => g.FlagActive == true).OrderBy(g => g.Name), "GroupId", "Name", rs_user.GroupId);
                //ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username", rs_user.UserEntry);
                //ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username", rs_user.UserUpdate);
                return View(rs_user);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_user rs_user = db.rs_user.Single(s => s.UserId == id);
                return View(rs_user);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                try
                {
                    rs_user rs_user = db.rs_user.Single(s => s.UserId == id);

                    Logger.Log("Delete", "Delete User [ID:" + rs_user.UserId + "Username:" + rs_user.Username + "]");
                    TempData["Notification"] = NotificationHelper.Inform("Delete User [ID:" + rs_user.UserId + ", Username:" + rs_user.Username + "]");

                    db.rs_user.Remove(rs_user);
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

        //
        // GET: /User/Reset/5

        public ActionResult Reset(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_user rs_user = db.rs_user.Where(s => s.UserId == id).Single();
                //ViewBag.CompanyId = new SelectList(db.rs_company.Where(c => c.FlagActive == true).OrderBy(c => c.Name), "CompanyId", "Name", rs_user.CompanyId);
                ViewBag.GroupId = new SelectList(db.rs_user_group.Where(g => g.FlagActive == true).OrderBy(g => g.Name), "GroupId", "Name", rs_user.GroupId);
                //ViewBag.UserEntry = new SelectList(db.rs_user, "UserId", "Username", rs_user.UserEntry);
                //ViewBag.UserUpdate = new SelectList(db.rs_user, "UserId", "Username", rs_user.UserUpdate);
                return View(rs_user);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //
        // POST: /User/Reset/5

        [HttpPost]
        public ActionResult Reset(rs_user rs_user)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                try
                {
                    rs_user baru = new rs_user();
                    baru = db.rs_user.Where(m => m.UserId == rs_user.UserId).Single();

                    //baru.Password = Crypto.Hash("123456", "sha256").Remove(32);
                    baru.Password = UtilitiesHelper.Encrypt("123456");
                    baru.UserUpdate = User.Identity.Name;
                    baru.DateUpdate = DateTime.Now;

                    db.Entry(baru).State = EntityState.Modified;
                    db.SaveChanges();
                    //db.rs_user.ApplyCurrentValues(baru);
                    //db.SaveChanges();
                    //db.rs_user.Attach(baru);
                    //db.ObjectStateManager.ChangeObjectState(baru, EntityState.Modified);
                    //db.SaveChanges();

                    Logger.Log("Reset", "Reset User Password [Username:" + rs_user.Username + "]");

                    TempData["Notification"] = NotificationHelper.Inform("Reset User Password [Username:" + rs_user.Username + "]");
                }
                catch (Exception ex)
                {
                    TempData["Notification"] =NotificationHelper.Error(ex.Message);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        public ActionResult Upload()
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

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (file != null && file.ContentLength > 0)
                {
                    if (Path.GetExtension(file.FileName).ToLower() != ".xls")
                    {
                        TempData["Notification"] =NotificationHelper.Error("Only XLS files are supported.");
                    }
                    else
                    {
                        try
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                            file.SaveAs(path);

                            DateTime start_time = DateTime.Now;
                            ParserInfoHelper PI_Helper = new ParserInfoHelper();
                            int recordProcessed = 0;
                            int recordWarning = 0;
                            int recordInserted = 0;
                            int recordError = 0;
                            string Summary = string.Empty;

                            Summary += "Filename : " + fileName + "<br><br>";

                            ExcelHelper helper = new ExcelHelper();
                            ArrayList data = helper.Parser(path, new String[] { "User", "1", "1", User.Identity.Name, DateTime.Now.ToString("dd-mm-yyyy HH:mm:ss") }, "Sheet1");

                            foreach (ArrayList row in data)
                            {
                                row[1] = UtilitiesHelper.Encrypt(row[1].ToString());
                            }

                            int totalRows = data.Count;

                            if (totalRows > 0)
                            {
                                helper.RowProcessed += (s, t) => recordProcessed += 1;
                                helper.RowInserted += (s, t) =>
                                {
                                    recordInserted += 1;
                                    PI_Helper.AddInfo(t.Message);
                                };
                                helper.RowWarning += (s, t) =>
                                {
                                    recordWarning += 1;
                                    PI_Helper.AddWarning(t.Message);
                                };
                                helper.RowError += (s, t) =>
                                {
                                    recordError += 1;
                                    PI_Helper.AddError(t.Message);
                                };

                                helper.Import(data, "rs_user", new string[] { "Username", "Password", "FullName", "UserType", "IsBackEnd", "FlagActive", "UserEntry", "DateEntry" }, null, new List<string>(new string[] { "1" }));
                            }

                            Summary += "Record Processed : " + recordProcessed + "<br>";
                            Summary += "Record Inserted : " + recordInserted + "<br>";
                            Summary += "Record Error : " + recordError + "<br>";
                            Summary += "Record Warning : " + recordWarning + "<br><br>";
                            Summary += "Elapsed Time: " + DateTime.Now.Subtract(start_time).Minutes + " minutes";
                            PI_Helper.Summary = Summary;

                            Logger.Log("Upload", "User upload successful.");

                            TempData["Notification"] = PI_Helper.Display();

                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["Notification"] =NotificationHelper.Error(ex.Message);
                        }
                    }
                }
                else
                {
                    TempData["Notification"] =NotificationHelper.Error("No file is selected.");
                }

                return View();
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        //public ActionResult Print(int id)
        //{
        //    if (AclHelper.hasAccess(User, currentAction, currentController))
        //    {
        //        try
        //        {
        //            ZebraPrinterProxy proxy = new ZebraPrinterProxy();
        //            proxy.Url = "http://localhost:9191";

        //            rs_user rs_user = db.rs_user.Single(s => s.UserId == id);
        //            sp_get_zebra_mapping_for_printing_result mapping = db.sp_get_zebra_mapping_for_printing("User Label").FirstOrDefault();

        //            XmlRpcStruct param = new XmlRpcStruct();
        //            param["darkness"] = mapping.Darkness.Value.ToString();
        //            param["username"] = rs_user.Username;
        //            param["name"] = rs_user.FullName;
        //            param["password"] = UtilitiesHelper.Decrypt(rs_user.Password);

        //            if (proxy.Print(mapping.IpAddress, mapping.Port.Value, mapping.TemplateCode, param))
        //            {
        //                TempData["Notification"] = NotificationHelper.Inform("Label is being printed.");
        //            }
        //            else
        //            {
        //                TempData["Notification"] = NotificationHelper.Inform("Failed to print label.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["Notification"] = NotificationHelper.Error(ex.Message);
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return RedirectToAction("NotAuthenticated", "Home");
        //    }
        //}

        //
        // GET: /User/DownloadTemplate

        public ActionResult DownloadTemplate()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                try
                {
                    ArrayList data = new ArrayList();
                    ArrayList headerRow = new ArrayList();
                    headerRow.Add("Username");
                    headerRow.Add("Password");
                    headerRow.Add("Full Name");
                    data.Add(headerRow);

                    ExcelHelper helper = new ExcelHelper();
                    var file = helper.GetExcelBytes(data, 0, 0, new List<int>() { 2 });

                    return File(file, "application/vnd.ms-excel", "User upload file template.xls");
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