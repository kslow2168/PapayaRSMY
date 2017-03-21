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
using System.Globalization;
using System.Threading;

namespace PapayaX2.Controllers
{
    public class BookingsController : Controller
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

            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [HttpPost]
        public ActionResult Index(BookSearchModel model, string search)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                List<rs_bookings> bookings = new List<rs_bookings>();
                bookings = db.rs_bookings.ToList();

                ViewBag.search = search;

                if (model != null)
                {
                    if (model.From != null && model.To != null)
                    {
                        model.To = model.To.Date.Add(new TimeSpan(23, 59, 59));

                        bookings = bookings.Where(t => (t.StartDate >= model.From && t.EndDate < model.To)).ToList();
                    }
                    if (model.AssetId != 0)
                    {
                        bookings = bookings.Where(t => t.AssetId == model.AssetId).ToList();
                    }
                    if (model.BookedBy != 0)
                    {
                        bookings = bookings.Where(t => t.ResquestorId == model.BookedBy).ToList();
                    }

                    if (search != null && search.Length > 0)
                    {

                        List<rs_bookings> searched = new List<rs_bookings>();

                        foreach (var item in search.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            searched.AddRange(bookings.Where(s => s.Remarks.Contains(item) || s.Purpose.Contains(item)
                            || s.rs_loan_form.Purpose.Contains(item) || s.rs_loan_form.Company.Contains(item)
                            || s.rs_loan_form.ContactPersion.Contains(item) || s.rs_loan_form.Email.Contains(item)
                            || s.rs_loan_form.Address.Contains(item) || s.rs_assets.Brand.Contains(item)
                            || s.rs_assets.Model.Contains(item) || s.rs_assets.Desciption.Contains(item)
                            || s.rs_assets.MaterialNo.Contains(item) || s.rs_assets.SerialNumber.Contains(item)
                            || s.rs_assets.TrackingNo.Contains(item)));
                        }

                        model.Bookings = searched.Distinct().ToList();
                        return View("Index", model);
                    }

                    model.Bookings = bookings;
                }
                else
                {
                    model = new BookSearchModel();
                    model.Bookings = bookings;
                }

                return View("Index", model);

            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                rs_bookings rs_bookings = db.rs_bookings.Find(id);
                if (rs_bookings == null)
                {
                    return HttpNotFound();
                }
                return View(rs_bookings);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: Bookings/Create New Booking
        public ActionResult Book(int AssetId)
        {
            BookData model = new BookData();
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (AssetId != 0)
                {
                    rs_assets asset = db.rs_assets.Find(AssetId);
                    if (asset != null)
                    {
                        if (asset.Availability != 1)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Asset not available for booking");
                            return RedirectToAction("Index");
                        }

                        if (asset.IsSystem)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("This function is not for booking a System");
                            return RedirectToAction("Index");
                        }

                        List<rs_bookings> bookings = db.rs_bookings.Where(x => x.AssetId == AssetId && !x.Returned).ToList();
                        List<string> bookdate = GetBookedDates(AssetId);
                        string bookDateStr = "";

                        foreach (string str in bookdate)
                        {
                            if (bookDateStr == "") bookDateStr = "'" + str + "'";
                            else bookDateStr = bookDateStr + ",'" + str + "'";
                        }

                        model.BookedDateStr = bookDateStr;

                        if (AclHelper.IsAdmin(User.Identity.Name))
                        {
                            ViewBag.ResquestorId = new SelectList(db.rs_user, "UserId", "Username");
                            model.IsBookOnBehalf = true;
                        }
                        ViewBag.LoanLocationId = new SelectList(db.rs_locations, "LocationId", "LocationName");

                        model.StartDate = DateTime.Today;
                        model.EndDate = DateTime.Today;
                        model.Asset = asset;
                        model.Bookings = bookings;
                        model.AssetId = AssetId;

                        return View("Book", model);
                    }
                    else
                    {
                        TempData["Notification"] = NotificationHelper.Inform("Asset Not found");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Notification"] = NotificationHelper.Inform("No Asset Selected!");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // POST:  Bookings/Book
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(BookData model)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_assets asset = db.rs_assets.Find(model.AssetId);
                    if (asset != null)
                    {
                        if (asset.Availability != 1)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Asset not available for booking");
                            return RedirectToAction("Index");
                        }

                        if (asset.IsSystem)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("This function is not for booking a System");
                            return RedirectToAction("Index");
                        }

                        if (CheckDateAvailable(model.StartDate, model.EndDate, model.AssetId))
                        {
                            rs_bookings newBooking = new rs_bookings();
                            newBooking.Approved = true;
                            newBooking.ApproverId = asset.OwnedBy;
                            newBooking.AssetId = model.AssetId;
                            newBooking.BookAt = DateTime.Now;
                            newBooking.CreatedBy = AclHelper.GetUserId(User.Identity.Name);
                            newBooking.Damaged = asset.Damaged;
                            newBooking.EndDate = model.EndDate;
                            newBooking.StartDate = model.StartDate;
                            newBooking.UpdatedAt = newBooking.BookAt;
                            newBooking.Extended = false;
                            newBooking.ExtendedDate = null;
                            //newBooking.LoanFormId = 8; //Always put new booking into dummy form
                            newBooking.LoanLocationId = model.LoanLocationId;
                            newBooking.Purpose = model.BookPurpose;
                            newBooking.Remarks = model.Remarks;
                            if (AclHelper.IsAdmin(User.Identity.Name))
                            {
                                newBooking.ResquestorId = model.ResquestorId;
                            }
                            else
                            {
                                newBooking.ResquestorId = AclHelper.GetUserId(User.Identity.Name);
                            }
                            newBooking.Returned = false;
                            newBooking.ReturnLocationId = 0;
                            newBooking.VerifyBy = asset.OwnedBy;

                            db.rs_bookings.Add(newBooking);
                            db.SaveChanges();

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking date crashed, please recheck the date!");

                            List<rs_bookings> bookings = db.rs_bookings.Where(x => x.AssetId == model.AssetId && !x.Returned).ToList();
                            List<string> bookdate = GetBookedDates(model.AssetId);
                            string bookDateStr = "";

                            foreach (string str in bookdate)
                            {
                                if (bookDateStr == "") bookDateStr = "'" + str + "'";
                                else bookDateStr = bookDateStr + ",'" + str + "'";
                            }

                            model.BookedDateStr = bookDateStr;

                            if (AclHelper.IsAdmin(User.Identity.Name))
                            {
                                ViewBag.ResquestorId = new SelectList(db.rs_user, "UserId", "Username");
                                model.IsBookOnBehalf = true;
                            }
                            ViewBag.LoanLocationId = new SelectList(db.rs_locations, "LocationId", "LocationName");

                            model.StartDate = DateTime.Today;
                            model.EndDate = DateTime.Today;
                            model.Asset = asset;
                            model.Bookings = bookings;
                            model.AssetId = asset.AssetId;

                            return View("Book", model);
                        }
                    }
                    else
                    {
                        TempData["Notification"] = NotificationHelper.Inform("Asset Not found");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Book", new { AssetId = model.AssetId });
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: Bookings/Create New Booking
        public ActionResult BookSystem(int SystemId)
        {
            SystemBookData model = new SystemBookData();
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (SystemId != 0)
                {
                    rs_assets asset = db.rs_assets.Find(SystemId);
                    if (asset != null)
                    {
                        if (asset.Availability != 1)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Asset not available for booking");
                            return RedirectToAction("Index");
                        }

                        if (!asset.IsSystem)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Selected Asset is not a system!");
                            return RedirectToAction("Index");
                        }

                        model = AssetHelper.GetSystemBookModel(SystemId);
                        List<int> ids = new List<int>();

                        foreach (SystemAsset ass in model.Assets)
                        {
                            ids.Add(ass.SubAsset.AssetId);
                        }

                        //List<rs_bookings> bookings = db.rs_bookings.Where(x => x.AssetId == AssetId && !x.Returned).ToList();
                        List<string> bookdate = GetBookedDates(ids);

                        string bookDateStr = "";

                        foreach (string str in bookdate)
                        {
                            if (bookDateStr == "") bookDateStr = "'" + str + "'";
                            else bookDateStr = bookDateStr + ",'" + str + "'";
                        }

                        model.BookedDateStr = bookDateStr;

                        if (AclHelper.IsAdmin(User.Identity.Name))
                        {
                            ViewBag.ResquestorId = new SelectList(db.rs_user, "UserId", "Username");
                            model.IsBookOnBehalf = true;
                        }
                        ViewBag.LoanLocationId = new SelectList(db.rs_locations, "LocationId", "LocationName");

                        model.StartDate = DateTime.Today;
                        model.EndDate = DateTime.Today;
                        model.System = asset;
                        //model.Bookings = bookings;
                        model.SystemId = SystemId;
                        TempData["System"] = asset;
                        TempData["SubAssets"] = model.Assets;
                        TempData.Keep("SubAssets");
                        TempData.Keep("System");
                        return View("BookSystem", model);
                    }
                    else
                    {
                        TempData["Notification"] = NotificationHelper.Inform("Asset Not found");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Notification"] = NotificationHelper.Inform("No Asset Selected!");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // POST:  Bookings/Book
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookSystem(SystemBookData model)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            try
            {
                model.Assets = (List<SystemAsset>)TempData["SubAssets"];

                model.System = (rs_assets)TempData["System"];
            }
            catch { }

            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (true)
                {
                    rs_assets asset = db.rs_assets.Find(model.SystemId);
                    if (asset != null)
                    {
                        if (asset.Availability != 1)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Asset not available for booking");
                            return RedirectToAction("Index");
                        }

                        if (!asset.IsSystem)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Selected Asset is not a system!");
                            return RedirectToAction("Index");
                        }

                        List<int> ids = new List<int>();

                        foreach (SystemAsset ass in model.Assets)
                        {
                            ids.Add(ass.SubAsset.AssetId);
                        }

                        if (CheckDateAvailable(model.StartDate, model.EndDate, ids))
                        {
                            foreach (SystemAsset ass in model.Assets)
                            {
                                if (ass.IsSelected)
                                {
                                    rs_bookings newBooking = new rs_bookings();
                                    newBooking.Approved = true;
                                    newBooking.ApproverId = ass.SubAsset.OwnedBy;
                                    newBooking.AssetId = ass.SubAsset.AssetId;
                                    newBooking.BookAt = DateTime.Now;
                                    newBooking.CreatedBy = AclHelper.GetUserId(User.Identity.Name);
                                    newBooking.Damaged = ass.SubAsset.Damaged;
                                    newBooking.EndDate = model.EndDate;
                                    newBooking.StartDate = model.StartDate;
                                    newBooking.UpdatedAt = newBooking.BookAt;
                                    newBooking.Extended = false;
                                    newBooking.ExtendedDate = null;
                                    ///newBooking.LoanFormId = 8; //Always put new booking into dummy form
                                    newBooking.LoanLocationId = model.LoanLocationId;
                                    newBooking.Purpose = model.BookPurpose;
                                    newBooking.Remarks = model.Remarks;
                                    if (AclHelper.IsAdmin(User.Identity.Name))
                                    {
                                        newBooking.ResquestorId = model.ResquestorId;
                                    }
                                    else
                                    {
                                        newBooking.ResquestorId = AclHelper.GetUserId(User.Identity.Name);
                                    }
                                    newBooking.Returned = false;
                                    newBooking.ReturnLocationId = 0;
                                    newBooking.VerifyBy = ass.SubAsset.OwnedBy;

                                    db.rs_bookings.Add(newBooking);
                                    db.SaveChanges();
                                }
                            }
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking date crashed, please recheck the date!");

                            ids = new List<int>();

                            foreach (SystemAsset ass in model.Assets)
                            {
                                ids.Add(ass.SubAsset.AssetId);
                            }

                            //List<rs_bookings> bookings = db.rs_bookings.Where(x => x.AssetId == AssetId && !x.Returned).ToList();
                            List<string> bookdate = GetBookedDates(ids);

                            string bookDateStr = "";

                            foreach (string str in bookdate)
                            {
                                if (bookDateStr == "") bookDateStr = "'" + str + "'";
                                else bookDateStr = bookDateStr + ",'" + str + "'";
                            }

                            model.BookedDateStr = bookDateStr;

                            if (AclHelper.IsAdmin(User.Identity.Name))
                            {
                                ViewBag.ResquestorId = new SelectList(db.rs_user, "UserId", "Username");
                                model.IsBookOnBehalf = true;
                            }
                            ViewBag.LoanLocationId = new SelectList(db.rs_locations, "LocationId", "LocationName");

                            model.StartDate = DateTime.Today;
                            model.EndDate = DateTime.Today;
                            model.System = asset;
                            //model.Bookings = bookings;
                            model.SystemId = asset.AssetId;

                            return View("BookSystem", model);
                        }
                    }
                    else
                    {
                        TempData["Notification"] = NotificationHelper.Inform("Asset Not found");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("BookSystem", new { SystemId = model.SystemId });
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }


        // GET: Bookings/Create New Booking
        public ActionResult BookExtension(int bookingId)
        {
            BookData model = new BookData();
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (bookingId != 0)
                {
                    rs_bookings book = db.rs_bookings.Find(bookingId);
                    if (book != null)
                    {

                        if (book.Returned)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking already returned!");
                            return RedirectToAction("Index");
                        }

                        if (!book.Approved)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking not approved!");
                            return RedirectToAction("Index");
                        }
                        if (book.Extended)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking already extended!");
                            return RedirectToAction("Index");
                        }
                        //if (book.StartDate > DateTime.Today)
                        //{
                        //    TempData["Notification"] = NotificationHelper.Inform("Booking not yet started");
                        //    return RedirectToAction("Index");
                        //}

                        rs_assets asset = db.rs_assets.Find(book.AssetId);
                        if (asset != null)
                        {
                            if (asset.IsSystem)
                            {
                                TempData["Notification"] = NotificationHelper.Inform("This function is not for booking a System");
                                return RedirectToAction("Index");
                            }
                            List<rs_bookings> bookings = db.rs_bookings.Where(x => x.AssetId == book.AssetId && !x.Returned).ToList();
                            List<string> bookdate = GetExtensibleDates(bookingId);
                            string bookDateStr = "";

                            foreach (string str in bookdate)
                            {
                                if (bookDateStr == "") bookDateStr = "'" + str + "'";
                                else bookDateStr = bookDateStr + ",'" + str + "'";
                            }

                            model.BookedDateStr = bookDateStr;

                            if (AclHelper.IsAdmin(User.Identity.Name))
                            {
                                ViewBag.ResquestorId = new SelectList(db.rs_user, "UserId", "Username");
                                model.IsBookOnBehalf = true;
                            }
                            ViewBag.LoanLocationId = new SelectList(db.rs_locations, "LocationId", "LocationName");
                            model.Remarks = book.Remarks;
                            model.BookPurpose = book.Purpose;
                            model.StartDate = book.StartDate;
                            model.EndDate = book.EndDate;
                            model.Asset = asset;
                            model.Bookings = bookings;
                            model.AssetId = book.AssetId;
                            model.BookId = bookingId;
                            return View("BookExtension", model);
                        }
                        else
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Asset Not found");
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Notification"] = NotificationHelper.Inform("Booking Not found");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Notification"] = NotificationHelper.Inform("No Asset Selected!");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }


        // POST:  Bookings/BookExtension
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookExtension(BookData model)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_bookings book = db.rs_bookings.Find(model.BookId);
                    if (book != null)
                    {
                        if (book.Returned)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking already returned!");
                            return RedirectToAction("Index");
                        }

                        if (!book.Approved)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking not approved!");
                            return RedirectToAction("Index");
                        }

                        if (book.Extended)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking already extended!");
                            return RedirectToAction("Index");
                        }

                        rs_assets asset = db.rs_assets.Find(book.AssetId);
                        if (asset != null)
                        {

                            if (asset.IsSystem)
                            {
                                TempData["Notification"] = NotificationHelper.Inform("This function is not for booking a System");
                                return RedirectToAction("Index");
                            }

                            if (CheckDateAvailable(model.StartDate, model.EndDate, model.AssetId))
                            {
                                book.EndDate = model.EndDate;
                                book.Extended = true;
                                book.ExtendedDate = DateTime.Now;
                                book.UpdatedAt = DateTime.Now;

                                db.Entry(book).State = EntityState.Modified;
                                db.SaveChanges();

                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["Notification"] = NotificationHelper.Inform("Booking date crashed, please recheck the date!");
                                List<rs_bookings> bookings = db.rs_bookings.Where(x => x.AssetId == book.AssetId && !x.Returned).ToList();
                                List<string> bookdate = GetExtensibleDates(model.BookId);
                                string bookDateStr = "";

                                foreach (string str in bookdate)
                                {
                                    if (bookDateStr == "") bookDateStr = "'" + str + "'";
                                    else bookDateStr = bookDateStr + ",'" + str + "'";
                                }

                                model.BookedDateStr = bookDateStr;

                                if (AclHelper.IsAdmin(User.Identity.Name))
                                {
                                    ViewBag.ResquestorId = new SelectList(db.rs_user, "UserId", "Username");
                                    model.IsBookOnBehalf = true;
                                }
                                ViewBag.LoanLocationId = new SelectList(db.rs_locations, "LocationId", "LocationName");

                                model.StartDate = book.StartDate;
                                model.EndDate = book.EndDate;
                                model.Asset = asset;
                                model.Bookings = bookings;
                                model.AssetId = book.AssetId;
                                model.BookId = book.BookId;
                                model.Remarks = book.Remarks;
                                model.BookPurpose = book.Purpose;
                                return View("BookExtension", model);
                            }
                        }
                        else
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Asset Not found");
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Notification"] = NotificationHelper.Inform("Booking Not found");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Book", new { AssetId = model.AssetId });
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }


        #region ReturnItem
        // GET: Bookings/Create New Booking
        private ActionResult CommonReturn(int BookId, bool OnBehalf)
        {
            ReturnData model = new ReturnData();
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (BookId != 0)
                {
                    rs_bookings booking = db.rs_bookings.Find(BookId);
                    if (booking != null)
                    {
                        if (booking.ResquestorId != AclHelper.GetUserId(User.Identity.Name) && !OnBehalf)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Selected booking is not belong to you!");
                            return RedirectToAction("Index");
                        }

                        if (booking.Returned)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Selected booking already returned!");
                            return RedirectToAction("Index");
                        }

                        //if (booking.Approved)
                        //{
                        //    TempData["Notification"] = NotificationHelper.Inform("Selected booking is not approved!");
                        //    return RedirectToAction("Index");
                        //}


                        ViewBag.LoanLocationId = new SelectList(db.rs_locations, "LocationId", "LocationName");

                        return View("Return", model);
                    }
                    else
                    {
                        TempData["Notification"] = NotificationHelper.Inform("Booking Not found");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Notification"] = NotificationHelper.Inform("No Asset Selected!");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        private ActionResult CommonReturn(ReturnData model, bool OnBehalf)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    rs_bookings booking = db.rs_bookings.Find(model.BookId);
                    if (booking != null)
                    {
                        int userId = AclHelper.GetUserId(User.Identity.Name);
                        if (booking.ResquestorId != userId && !OnBehalf)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Selected booking is not belong to you!");
                            return RedirectToAction("Index");
                        }

                        if (booking.Returned)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Selected booking already returned!");
                            return RedirectToAction("Index");
                        }

                        bool ret = ReturnAsset(userId, booking.BookId, model.LocationId, model.Remarks);

                        if (ret)
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking returned, wait for validation");
        
                        }
                        else
                        {
                            TempData["Notification"] = NotificationHelper.Inform("Booking not returned");
                        }

                        return RedirectToAction("Index");
                        
                    }
                    else
                    {
                        TempData["Notification"] = NotificationHelper.Inform("Booking Not found");
                        return RedirectToAction("Index");
                    }

                }

                ViewBag.LoanLocationId = new SelectList(db.rs_locations, "LocationId", "LocationName");

                return View("Return", model);

            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: Bookings/Create New Booking
        public ActionResult Return(int BookId)
        {
           return CommonReturn(BookId, false);
        }

        // POST:  Bookings/Book
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(ReturnData model)
        {
            return CommonReturn(model, false);
        }

        // GET: Bookings/Create New Booking
        public ActionResult ReturnOnBehalf(int BookId)
        {
            return CommonReturn(BookId, true);
        }

        // POST:  Bookings/Book
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnOnBehalf(ReturnData model)
        {
            return CommonReturn(model, true);
        }

        #endregion

        /*To-DO
         * Extend Booking
         * Cancel Booking
         * Return
         * Approve
         */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Helper
        
        private bool CheckDateAvailable(DateTime startDate, DateTime endDate, int AssetId)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            bool ret = false;

            if (endDate < startDate) return false;

            int bookCount = db.rs_bookings.Where(x => x.AssetId == AssetId && x.StartDate >= startDate && x.EndDate <= endDate && !x.Returned).Count();

            if (bookCount == 0) ret = true;

            return ret;
        }

        private bool CheckDateAvailable(DateTime startDate, DateTime endDate, List<int> AssetId)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            bool ret = false;

            if (endDate < startDate) return false;
            List<rs_bookings> bookings = db.rs_bookings.Where(x => x.StartDate >= startDate && x.EndDate <= endDate && !x.Returned).ToList();
            List<rs_bookings> searched = new List<rs_bookings>();

            foreach (int id in AssetId)
            {
                searched.AddRange(bookings.Where(x => x.AssetId == id));
            }

            bookings = searched.Distinct().ToList();

            int bookCount = bookings.Count();

            if (bookCount == 0) ret = true;

            return ret;
        }

        private List<string> GetExtensibleDates(int BookingId)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            List<string> ret = new List<string>();

            rs_bookings booking = db.rs_bookings.Find(BookingId);

            if (booking != null)
            {
                DateTime cutoffDate = booking.EndDate.AddDays(30);
                //get nearest booking if available
                try
                {
                    rs_bookings nextBooking = db.rs_bookings.Where(x => x.AssetId == booking.AssetId && x.StartDate > booking.EndDate && x.EndDate < cutoffDate && x.Approved && !x.Returned).First();
                    if (nextBooking != null)
                    {
                        for (DateTime date = booking.EndDate; date.Date <= nextBooking.StartDate.Date; date = date.AddDays(1))
                        {
                            string dateStr = date.ToString("d-M-yyyy");
                            ret.Add(dateStr);
                        }
                    }
                    else //Enable max extension of 30 days
                    {
                        for (DateTime date = booking.EndDate; date.Date <= booking.EndDate.AddDays(30); date = date.AddDays(1))
                        {
                            string dateStr = date.ToString("d-M-yyyy");
                            ret.Add(dateStr);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            return ret;
        }

        private List<string> GetBookedDates(int AssetId)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            List<string> ret = new List<string>();

            List<rs_bookings> bookings = db.rs_bookings.Where(x => x.AssetId == AssetId && !x.Returned).ToList();

            foreach (rs_bookings book in bookings)
            {
                for (DateTime date = book.StartDate; date.Date <= book.EndDate.Date; date = date.AddDays(1))
                {
                    string dateStr = date.ToString("d-M-yyyy");
                    ret.Add(dateStr);
                }
            }
            return ret;
        }

        private List<string> GetBookedDates(List<int> AssetId)
        {

            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            List<string> ret = new List<string>();

            List<rs_bookings> bookings = db.rs_bookings.Where(x => !x.Returned).ToList();

            List<rs_bookings> searched = new List<rs_bookings>();

            foreach (int id in AssetId)
            {
                searched.AddRange(bookings.Where(x => x.AssetId == id));
            }

            bookings = searched.Distinct().ToList();

            foreach (rs_bookings book in bookings)
            {
                for (DateTime date = book.StartDate; date.Date <= book.EndDate.Date; date = date.AddDays(1))
                {
                    string dateStr = date.ToString("d-M-yyyy");
                    if (!ret.Contains(dateStr))
                        ret.Add(dateStr);
                }

            }

            return ret;
        }

        private List<rs_bookings> GetExpiredBookings(int userId)
        {
            List<rs_bookings> ret = null;

            try
            {
                ret = db.rs_bookings.Where(x => x.ResquestorId == userId && x.EndDate <= DateTime.Now && !x.Returned && x.Approved).ToList();
            }
            catch
            { }
            return ret;

        }
        private List<rs_bookings> GetActiveBookings(int userId)
        {
            List<rs_bookings> ret = null;

            try
            {
                ret = db.rs_bookings.Where(x => x.ResquestorId == userId && !x.Returned && x.Approved).ToList();
            }
            catch
            { }
            return ret;

        }

        private List<rs_bookings> GetRejectedBookings(int userId)
        {
            List<rs_bookings> ret = null;

            try
            {
                ret = db.rs_bookings.Where(x => x.ResquestorId == userId && !x.Returned && !x.Approved).ToList();
            }
            catch
            { }
            return ret;

        }

        private List<rs_bookings> GetReturnedBookings(int userId)
        {
            List<rs_bookings> ret = null;

            try
            {
                ret = db.rs_bookings.Where(x => x.ResquestorId == userId && x.Returned && x.Approved).ToList();
            }
            catch
            { }
            return ret;

        }

        public bool ReturnAsset(int userId, int bookId, int locationId, string remarks)
        {
            bool ret = false;

            //Check if Booking existed
            rs_bookings book = db.rs_bookings.Find(bookId);
            if (book != null)
            {
                book.ReturnDate = DateTime.Now;
                book.Returned = true;
                book.ReturnLocationId = locationId;
                book.UpdatedAt = DateTime.Now;
                book.ReturnBy = userId;
                book.ReturnRemark = remarks;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                ret = true;

                //Add Notification to owner here
            }

            return ret;
        }

        #endregion
    }
}
