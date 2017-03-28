using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PapayaX2.Database;
using PapayaX2.Helpers;
using PapayaX2.Models;
using System.Web.Security;

namespace PapayaX2.Controllers
{
    public class AssetsController : Controller
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

        // GET: Assets From Simple text box search
        public ActionResult Index(string search, int page = 1)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                ViewBag.search = search;

                if (page < 1)
                {
                    page = 1;
                }

                int Skip = (page - 1) * Configuration.RowPerPage;
                int Take = Configuration.RowPerPage;

                if (search != null && search.Length > 0)
                {
                    var rs_assets = db.rs_assets.Where(s => s.SerialNumber != null).ToList();

                    int iss = 0;
                    foreach (var item in search.ToLower().Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        rs_assets = rs_assets.Where(s => s.SerialNumber.ToLower().Contains(item) ||
                                                                s.Accessories.ToLower().Contains(item) ||
                                                                s.Brand.ToLower().Contains(item) ||
                                                                s.Desciption.ToLower().Contains(item) ||
                                                                s.HardwareOpt.ToLower().Contains(item) ||
                                                                s.HardwareVer.ToLower().Contains(item) ||
                                                                s.MaterialNo.ToLower().Contains(item) ||
                                                                s.Model.ToLower().Contains(item) ||
                                                                s.PurchasePO.ToLower().Contains(item) ||
                                                                s.Remarks.ToLower().Contains(item) ||
                                                                s.SoftwareOpt.ToLower().Contains(item) ||
                                                                s.SoftwareVer.ToLower().Contains(item) ||
                                                                s.TrackingNo.ToLower().Contains(item)).ToList();

                    }

                    if (rs_assets.Distinct().Count() > 0)
                    {
                        int TotalPageSearch = Convert.ToInt32(Math.Ceiling((double)rs_assets.Distinct().Count() / Configuration.RowPerPage));
                        int PageSearch = page;

                        if (PageSearch > TotalPageSearch)
                        {
                            PageSearch = TotalPageSearch;
                            Skip = (PageSearch - 1) * Configuration.RowPerPage;
                        }

                        PaginationHelper phSearch = new PaginationHelper(TotalPageSearch, Url.Content("~/Assets/Index/?page={0}" + "&search=" + search));
                        ViewBag.Pagination = phSearch.render_pagination(PageSearch);
                      
                        return View(rs_assets.Distinct().OrderBy(s => s.TrackingNo).ToList().Skip(Skip).Take(Take));
                    }
                    else
                        return View(rs_assets.Distinct().OrderBy(s => s.TrackingNo).ToList());

                }

                int TotalPage = Convert.ToInt32(Math.Ceiling((double)db.rs_assets.Count() / Configuration.RowPerPage));

                if (TotalPage > 0)
                {
                    int Page = page;

                    if (Page > TotalPage && TotalPage > 0)
                    {
                        Page = TotalPage;
                        Skip = (Page - 1) * Configuration.RowPerPage;
                    }
                    
                    var all_items = db.rs_assets.OrderBy(s => s.TrackingNo).ToList().Skip(Skip).Take(Take);

                    PaginationHelper ph = new PaginationHelper(TotalPage, Url.Content("~/Assets/Index/?page={0}" + "&search=" + search));
                    ViewBag.Pagination = ph.render_pagination(Page);

                    return View(all_items.ToList());
                }
                else
                    return View(db.rs_assets.OrderBy(s => s.TrackingNo).ToList());
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }


        }

        // GET: Assets from Advanced fields search
        public ActionResult IndexAdv(FormCollection search, int page = 1)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                ViewBag.search = search;

                if (page < 1)
                {
                    page = 1;
                }

                int Skip = (page - 1) * Configuration.RowPerPage;
                int Take = Configuration.RowPerPage;

                if (search != null && search.Count > 0)
                {
                    var rs_assets = db.rs_assets.Where(s => s.SerialNumber != null).ToList();

                    int iss = 0;

                    for (int i = 0; i < search.Count; i++)
                    {
                        if (search.AllKeys[i] == "SerialNumber")
                        {
                            rs_assets = rs_assets.Where(s => s.SerialNumber.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "Accessories")
                        {
                            rs_assets = rs_assets.Where(s => s.Accessories.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "Brand")
                        {
                            rs_assets = rs_assets.Where(s => s.Brand.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "Desciption")
                        {
                            rs_assets = rs_assets.Where(s => s.Desciption.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "HardwareOpt")
                        {
                            rs_assets = rs_assets.Where(s => s.HardwareOpt.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "HardwareVer")
                        {
                            rs_assets = rs_assets.Where(s => s.HardwareVer.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "MaterialNo")
                        {
                            rs_assets = rs_assets.Where(s => s.MaterialNo.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "Model")
                        {
                            rs_assets = rs_assets.Where(s => s.Model.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "PurchasePO")
                        {
                            rs_assets = rs_assets.Where(s => s.PurchasePO.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "SoftwareOpt")
                        {
                            rs_assets = rs_assets.Where(s => s.SoftwareOpt.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "SoftwareVer")
                        {
                            rs_assets = rs_assets.Where(s => s.SoftwareVer.ToLower().Contains(search.Get(i))).ToList();
                        }
                        else if (search.AllKeys[i] == "TrackingNo")
                        {
                            rs_assets = rs_assets.Where(s => s.TrackingNo.ToLower().Contains(search.Get(i))).ToList();
                        }

                    }

                    if (rs_assets.Distinct().Count() > 0)
                    {
                        int TotalPageSearch = Convert.ToInt32(Math.Ceiling((double)rs_assets.Distinct().Count() / Configuration.RowPerPage));
                        int PageSearch = page;

                        if (PageSearch > TotalPageSearch)
                        {
                            PageSearch = TotalPageSearch;
                            Skip = (PageSearch - 1) * Configuration.RowPerPage;
                        }

                        PaginationHelper phSearch = new PaginationHelper(TotalPageSearch, Url.Content("~/Assets/Index/?page={0}" + "&search=" + search));
                        ViewBag.Pagination = phSearch.render_pagination(PageSearch);

                        return View(rs_assets.Distinct().OrderBy(s => s.TrackingNo).ToList().Skip(Skip).Take(Take));
                    }
                    else
                        return View(rs_assets.Distinct().OrderBy(s => s.TrackingNo).ToList());

                }

                int TotalPage = Convert.ToInt32(Math.Ceiling((double)db.rs_assets.Count() / Configuration.RowPerPage));

                if (TotalPage > 0)
                {
                    int Page = page;

                    if (Page > TotalPage && TotalPage > 0)
                    {
                        Page = TotalPage;
                        Skip = (Page - 1) * Configuration.RowPerPage;
                    }

                    var all_items = db.rs_assets.OrderBy(s => s.TrackingNo).ToList().Skip(Skip).Take(Take);

                    PaginationHelper ph = new PaginationHelper(TotalPage, Url.Content("~/Assets/Index/?page={0}" + "&search=" + search));
                    ViewBag.Pagination = ph.render_pagination(Page);

                    return View(all_items.ToList());
                }
                else
                    return View(db.rs_assets.OrderBy(s => s.TrackingNo).ToList());
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }


        }

        // GET: Assets/SystemDetails/5
        public ActionResult SystemDetails(int? id)
        {
            SystemModel model = null;
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                rs_assets rs_assets = db.rs_assets.Find(id);
                if (rs_assets == null)
                {
                    return HttpNotFound();
                }
                if (rs_assets.IsSystem)
                {
                    model = AssetHelper.GetSystemModel(rs_assets.AssetId);
                }
                else
                {
                    model.System = rs_assets;
                    model.Assets = null;
                }
                return View(rs_assets);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: Assets/Details/5 
        public ActionResult Details(int? id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ViewBag.OriginLocId = new SelectList(db.rs_locations, "LocationId", "LocationName");
                ViewBag.CurrentLocId = new SelectList(db.rs_locations, "LocationId", "LocationName");
                ViewBag.OwnerId = new SelectList(db.rs_user, "UserId", "Username");
                ViewBag.DivId = new SelectList(db.rs_division, "DivId", "DivisionNo");
                ViewBag.OwnerShipId = new SelectList(db.rs_ownership, "OwnerShipId", "OwnerType");
                ViewBag.Availability = new SelectList(db.rs_assetstatus, "StatusId", "Status");
                if (AclHelper.IsAdmin(User.Identity.Name))
                {
                    ViewBag.SubAssetId = new SelectList(db.rs_assets.Where(x => x.IsSystem == false), "AssetId", "Model");

                }
                else
                {
                    ViewBag.SubAssetId = new SelectList(db.rs_assets.Where(x => x.IsSystem == false && x.OwnedBy == AclHelper.GetUserId(User.Identity.Name)), "AssetId", "Model");


                }
                SystemModel system = new SystemModel();

                system = AssetHelper.GetSystemModel((int)id);

                if (system.System == null)
                {
                    return HttpNotFound();
                }
                system.SystemId = system.System.AssetId;

                if (TempData["errorMessage"] != null)
                {
                    TempData["Notification"] =NotificationHelper.Inform(TempData["errorMessage"].ToString());

                    TempData.Remove("errorMessage");
                }
                system.Step = 3;
                return View("Assets", system);

            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: Assets/Create
        public ActionResult Create()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                ViewBag.OriginLocId = new SelectList(db.rs_locations, "LocationId", "LocationName");
                ViewBag.CurrentLocId = new SelectList(db.rs_locations, "LocationId", "LocationName");
                ViewBag.OwnerId = new SelectList(db.rs_user, "UserId", "Username");
                ViewBag.DivId = new SelectList(db.rs_division, "DivId", "DivisionNo");
                ViewBag.OwnerShipId = new SelectList(db.rs_ownership, "OwnerShipId", "OwnerType");
                ViewBag.Availability = new SelectList(db.rs_assetstatus, "StatusId", "Status");

                if (AclHelper.IsAdmin(User.Identity.Name))
                {
                    ViewBag.SubAssetId = new SelectList(db.rs_assets.Where(x => x.IsSystem == false), "AssetId", "Model");

                }
                else
                {
                    ViewBag.SubAssetId = new SelectList(db.rs_assets.Where(x => x.IsSystem == false && x.OwnedBy == AclHelper.GetUserId(User.Identity.Name)), "AssetId", "Model");


                }
                SystemModel model = new SystemModel();

                model.Step = 1;
                return View("Assets", model);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // POST: Assets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SystemModel model, HttpPostedFileBase file)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    model.Step = 2;

                    model.System.CreatedDate = DateTime.Now;
                    model.System.UpdatedDate = DateTime.Now;
                    model.System.CurrentLocId = model.CurrentLocId;
                    model.System.OriginLocId = model.OriginLocId;
                    model.System.OwnedBy = model.OwnerId;
                    model.System.DivId = model.DivId;
                    model.System.OwnerShipId = model.OwnerShipId;
                    model.System.Availability = model.Availability;
                      ViewBag.Availability = new SelectList(db.rs_assetstatus, "StatusId", "Status");

                    string ownerShip = db.rs_ownership.Find(model.OwnerShipId).OwnerType;
                    int divNo = db.rs_division.Find(model.DivId).DivisionNo;

                    model.System.TrackingNo = AssetHelper.GenerateTrackingNo(ownerShip,
                        divNo, model.System.IsSystem, model.System.PurchaseDate);

                    if (AssetHelper.IsImage(file))
                    {
                        file.SaveAs(HttpContext.Server.MapPath("~/AssetsImg/")
                                                              + file.FileName);
                        model.System.ImageLink = file.FileName;
                    }

                    db.rs_assets.Add(model.System);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.OriginLocId = new SelectList(db.rs_locations, "LocationId", "LocationName", model.System.OriginLocId);
                ViewBag.CurrentLocId = new SelectList(db.rs_locations, "LocationId", "LocationName", model.System.CurrentLocId);
                ViewBag.OwnerId = new SelectList(db.rs_user, "UserId", "Username");
                ViewBag.DivId = new SelectList(db.rs_division, "DivId", "DivisionNo");
                ViewBag.OwnerShipId = new SelectList(db.rs_ownership, "OwnerShipId", "OwnerType");
                ViewBag.Availability = new SelectList(db.rs_assetstatus, "StatusId", "Status");

                return View("Assets", model);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: Assets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                rs_assets rs_assets = db.rs_assets.Find(id);
                if (rs_assets == null)
                {
                    return HttpNotFound();
                }
                SystemModel model = new SystemModel();

                model.System = rs_assets;
                ViewBag.OriginLocId = new SelectList(db.rs_locations, "LocationId", "LocationName", rs_assets.OriginLocId);
                ViewBag.CurrentLocId = new SelectList(db.rs_locations, "LocationId", "LocationName", rs_assets.CurrentLocId);
                ViewBag.OwnerId = new SelectList(db.rs_user, "UserId", "Username", rs_assets.OwnedBy);
                ViewBag.DivId = new SelectList(db.rs_division, "DivId", "DivisionNo", rs_assets.DivId);
                ViewBag.OwnerShipId = new SelectList(db.rs_ownership, "OwnerShipId", "OwnerType", rs_assets.OwnerShipId);
                ViewBag.Availability = new SelectList(db.rs_assetstatus, "StatusId", "Status", rs_assets.Availability);
                return View(model);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SystemModel model)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {

                    model.System.UpdatedDate = DateTime.Now;
                    model.System.CurrentLocId = model.CurrentLocId;
                    model.System.OriginLocId = model.OriginLocId;
                    model.System.OwnedBy = model.OwnerId;
                    model.System.DivId = model.DivId;
                    model.System.OwnerShipId = model.OwnerShipId;
                    model.System.Availability = model.Availability;
                    db.Entry(model.System).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.OriginLocId = new SelectList(db.rs_locations, "LocationId", "LocationName", model.System.OriginLocId);
                ViewBag.CurrentLocId = new SelectList(db.rs_locations, "LocationId", "LocationName", model.System.CurrentLocId);
                ViewBag.OwnerId = new SelectList(db.rs_user, "UserId", "Username", model.System.OwnedBy);
                ViewBag.DivId = new SelectList(db.rs_division, "DivId", "DivisionNo", model.System.DivId);
                ViewBag.OwnerShipId = new SelectList(db.rs_ownership, "OwnerShipId", "OwnerType", model.System.OwnerShipId);
                ViewBag.Availability = new SelectList(db.rs_assetstatus, "StatusId", "Status", model.System.Availability);
                return View(model);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: Assets/Delete/5 Delete Assets
        public ActionResult Delete(int? id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                rs_assets rs_assets = db.rs_assets.Find(id);
                if (rs_assets == null)
                {
                    return HttpNotFound();
                }
                return View(rs_assets);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_assets rs_assets = db.rs_assets.Find(id);

                //Delete any attached accessories
                List<rs_accessories> acclist = db.rs_accessories.Where(x => x.AssetId == id).ToList();
                foreach (rs_accessories acc in acclist)
                {
                    db.rs_accessories.Remove(acc);
                    db.SaveChanges();
                }

                //Delete any related relationship
                List<rs_assets_rel> relist = db.rs_assets_rel.Where(x => x.AssetId == id || x.SysId == id).ToList();
                foreach (rs_assets_rel rel in relist)
                {
                    db.rs_assets_rel.Remove(rel);
                    db.SaveChanges();
                }

                db.rs_assets.Remove(rs_assets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // POST: Assets/Delete/5 Delete Relationship
        //[HttpPost, ActionName("DeleteRel")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteRel(int id, int parentId)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_assets_rel rel = db.rs_assets_rel.FirstOrDefault(x => x.AssetId == id && x.SysId == parentId);
                db.rs_assets_rel.Remove(rel);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // POST: Assets/Delete/5 Delete Accessories
        //[HttpPost, ActionName("DeleteAcc")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteAcc(int id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                rs_accessories acc = db.rs_accessories.Find(id);
                db.rs_accessories.Remove(acc);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        /*
       * Function to add sub system.
       * 
       */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAsset(SystemModel model, HttpPostedFileBase file)
        {

            rs_assets rs_assets = model.SubAsset;
            JSonResponse result = new JSonResponse();
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_assets system = db.rs_assets.Find(model.System.AssetId);
                    if (system != null)
                    {
                        rs_assets.CreatedDate = DateTime.Now;
                        rs_assets.UpdatedDate = DateTime.Now;
                        rs_assets.CurrentLocId = model.CurrentLocId;
                        rs_assets.OriginLocId = model.OriginLocId;
                        rs_assets.OwnedBy = model.OwnerId;
                        rs_assets.DivId = model.DivId;
                        rs_assets.OwnerShipId = model.OwnerShipId;
                        rs_assets.Availability = model.Availability;
                        string ownerShip = db.rs_ownership.Find(model.OwnerShipId).OwnerType;
                        int divNo = db.rs_division.Find(model.DivId).DivisionNo;

                        rs_assets.TrackingNo = AssetHelper.GenerateTrackingNo(ownerShip, divNo,
                            rs_assets.IsSystem, rs_assets.PurchaseDate);

                        if (AssetHelper.IsImage(file))
                        {
                            file.SaveAs(HttpContext.Server.MapPath("~/AssetsImg/")
                                                                  + file.FileName);
                            rs_assets.ImageLink = file.FileName;
                        }
                        db.rs_assets.Add(rs_assets);
                        db.SaveChanges();

                        rs_assets_rel rel = new rs_assets_rel();
                        rel.AssetId = rs_assets.AssetId;
                        rel.SysId = model.System.AssetId;
                        db.rs_assets_rel.Add(rel);
                        db.SaveChanges();

                        return RedirectToAction("Details", new { id = model.System.AssetId });
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = model.System.AssetId });
                    }
                }
                else
                {
                    return RedirectToAction("NotAuthenticated", "Home");
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }

        }

        /*
        * Function to add sub system with existing sub asset.
        * 
        */
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AttachSubAsset(FormCollection collection)
        {
            int SystemId = 0;
            int SubAssetId = 0;
            
            Int32.TryParse(collection.Get("SubAssetId").ToString(), out SubAssetId);
            Int32.TryParse(collection.Get("SystemId").ToString(), out SystemId);

            JSonResponse result = new JSonResponse();
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (SubAssetId != 0 && SystemId != 0)
                {

                    rs_assets rs_asset = db.rs_assets.Find(SubAssetId);
                    rs_assets system = db.rs_assets.Find(SystemId);

                    if (system != null && rs_asset != null)
                    {
                        //check and only allow to attach if asset not attached before
                        rs_assets_rel exist = db.rs_assets_rel.FirstOrDefault(x => x.AssetId == SubAssetId && x.SysId == SystemId);
                        if (exist == null)
                        {
                            rs_assets_rel rel = new rs_assets_rel();
                            rel.AssetId = rs_asset.AssetId;
                            rel.SysId = SystemId;
                            db.rs_assets_rel.Add(rel);
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Selected Sub Asset already attached");
                        }

                        return RedirectToAction("Details", new { id = SystemId });
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = SystemId });
                    }
                }
                else
                {
                    return RedirectToAction("NotAuthenticated", "Home");
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }

        }

        /*
        * Function to add sub system.
        * 
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccessories(SystemModel model)
        {
            rs_accessories acc = model.Accessories;
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_assets system = db.rs_assets.Find(model.System.AssetId);
                    if (system != null)
                    {
                        acc.AssetId = model.System.AssetId;
                        db.rs_accessories.Add(acc);
                        db.SaveChanges();

                        return RedirectToAction("Details", new { id = model.System.AssetId });
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = model.System.AssetId });
                    }
                }
                else
                {
                    return RedirectToAction("NotAuthenticated", "Home");
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region JSON WEB API

        /*
         * Function to add sub system.
         * 
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddAssetJson(SystemModel model, HttpPostedFileBase file)
        {

            rs_assets rs_assets = model.SubAsset;
            JSonResponse result = new JSonResponse();
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_assets system = db.rs_assets.Find(model.System.AssetId);
                    if (system != null)
                    {
                        rs_assets.CurrentLocId = model.CurrentLocId;
                        rs_assets.OriginLocId = model.OriginLocId;
                        rs_assets.OwnedBy = model.OwnerId;
                        rs_assets.DivId = model.DivId;
                        rs_assets.OwnerShipId = model.OwnerShipId;
                        rs_assets.CreatedDate = DateTime.Now;
                        rs_assets.UpdatedDate = DateTime.Now;

                        string ownerShip = db.rs_ownership.Find(model.OwnerShipId).OwnerType;
                        int divNo = db.rs_division.Find(model.DivId).DivisionNo;

                        rs_assets.TrackingNo = AssetHelper.GenerateTrackingNo(ownerShip, divNo,
                            rs_assets.IsSystem, rs_assets.PurchaseDate);


                        if (AssetHelper.IsImage(file))
                        {
                            file.SaveAs(HttpContext.Server.MapPath("~/AssetsImg/")
                                                                  + file.FileName);
                            rs_assets.ImageLink = file.FileName;
                        }
                        db.rs_assets.Add(rs_assets);
                        db.SaveChanges();

                        rs_assets_rel rel = new rs_assets_rel();
                        rel.AssetId = rs_assets.AssetId;
                        rel.SysId = model.System.AssetId;
                        db.rs_assets_rel.Add(rel);
                        db.SaveChanges();

                        result.Success = true;
                        result.Message = "Success";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "System Asset does not exists!";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "Invalid Parameters!";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                result.Success = false;
                result.Message = "Unauthenticated Access!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetAssetDetails(int assetId)
        {
            JsonAssetModel model = new JsonAssetModel();
            rs_assets asset = db.rs_assets.Find(assetId);

            if (asset != null)
            {
                model.Brand = asset.Brand;
                model.Model = asset.Model;
                model.Description = asset.Desciption;
                model.SerialNumber = asset.SerialNumber;
                model.OwnerShip = asset.rs_ownership.OwnerType;
                model.Owner = asset.rs_user.FullName;
                model.ImagePath = asset.ImageLink;
                model.Success = true;

            }
            else
            {
                model.Success = false;
                model.ErrMsg = "Asset Not Found!";
            }

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddAsset2([Bind(Include = "ParentId,Brand,Model,Desciption,SerialNumber,MaterialNo,HardwareOpt,SoftwareOpt,HardwareVer,SoftwareVer,LicenseExpiry,Accessories,Remarks,PurchaseDate,PurchasePrice,DecomDate,DecomReason,Tagged,Damaged,LastCalibrated,CalibrationCycle,CreatedDate,UpdatedDate,CreatedBy,OwnedBy,TrackingNo,ReadyToSell,Availability,OriginLocId,CurrentLocId,PurchasePO,DepreciationFormula,ImageLink,ViewedStats,BookingStats,FeaturedOrder,Featured,IsSystem")] rs_assets rs_assets, int SysId)
        {
            JSonResponse result = new JSonResponse();
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_assets system = db.rs_assets.Find(SysId);
                    if (system != null)
                    {
                        db.rs_assets.Add(rs_assets);
                        db.SaveChanges();

                        rs_assets_rel rel = new rs_assets_rel();
                        rel.AssetId = rs_assets.AssetId;
                        rel.SysId = SysId;
                        db.rs_assets_rel.Add(rel);
                        db.SaveChanges();

                        result.Success = true;
                        result.Message = "Success";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "System Asset does not exists!";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "Invalid Parameters!";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                result.Success = false;
                result.Message = "Unauthenticated Access!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Helpers
 


     
        #endregion
    }
}
