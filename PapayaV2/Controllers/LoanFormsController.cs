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
    public class LoanFormsController : Controller
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
        public ActionResult Index(LoanFormSearchModel model, string search)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                List<rs_loan_form> loanForm = new List<rs_loan_form>();
                loanForm = db.rs_loan_form.ToList();

                ViewBag.search = search;

                if (model != null)
                {
                    if (model.From != null && model.To != null)
                    {
                        model.To = model.To.Date.Add(new TimeSpan(23, 59, 59));

                        loanForm = loanForm.Where(t => (t.StartDate >= model.From && t.EndDate < model.To)).ToList();
                    }
                    if (model.LoanId != 0)
                    {
                        loanForm = loanForm.Where(t => t.LoanId == model.LoanId).ToList();
                    }
                    if (model.BookedBy != 0)
                    {
                        loanForm = loanForm.Where(t => t.RequestorId == model.BookedBy).ToList();
                    }

                    if (search != null && search.Length > 0)
                    {

                        List<rs_loan_form> searched = new List<rs_loan_form>();

                        foreach (var item in search.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            searched.AddRange(loanForm.Where(s => s.Address.Contains(item) || s.Company.Contains(item)
                            || s.ContactPersion.Contains(item) || s.Email.Contains(item)
                            || s.Purpose.Contains(item) || s.Telephone.Contains(item)
                            || s.UploadedFile.Contains(item)));
                        }

                        model.Loans = searched.Distinct().ToList();
                        return View("Index", model);
                    }

                    model.Loans = loanForm;
                }
                else
                {
                    model = new LoanFormSearchModel();
                    model.Loans = loanForm;
                }

                return View("Index", model);

            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        public ActionResult Printable(int? id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                rs_loan_form rs_loan_form = db.rs_loan_form.Find(id);
                if (rs_loan_form == null)
                {
                    return HttpNotFound();
                }
                return View(rs_loan_form);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: LoanForms/Details/5
        public ActionResult Details(int? id)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                rs_loan_form rs_loan_form = db.rs_loan_form.Find(id);
                if (rs_loan_form == null)
                {
                    return HttpNotFound();
                }
                return View(rs_loan_form);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: LoanForms/Create
        public ActionResult Create()
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                LoanFormViewModel viewModel = new LoanFormViewModel();

                //Get own booking
                int userId = AclHelper.GetUserId(User.Identity.Name);
                List<rs_bookings> bookings = db.rs_bookings.Where(x => x.Approved && !x.Returned && x.ResquestorId == userId).ToList();

                viewModel.AvailableBookings = bookings;
                viewModel.SelectedBookings = new List<rs_bookings>();

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // POST: LoanForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoanFormViewModel model)
        {
            if (AclHelper.hasAccess(User, currentAction, currentController))
            {
                if (ModelState.IsValid)
                {
                    model.LoanForm.RequestorId = AclHelper.GetUserId(User.Identity.Name);
                    model.LoanForm.RequestNo = 0;
                    model.LoanForm.UpdatedDate = DateTime.Now;
                    model.LoanForm.CreatedDate = DateTime.Now;
                   
                    db.rs_loan_form.Add(model.LoanForm);
                    db.SaveChanges();
                    
                    foreach (string bookId in model.Bookings.BookingIds)
                    {
                        int id;
                        int.TryParse(bookId, out id);
                        rs_bookings booking = db.rs_bookings.Find(id);

                        if (booking != null)
                        {
                            booking.LoanFormId = model.LoanForm.LoanId;
                            db.Entry(booking).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("NotAuthenticated", "Home");
            }
        }

        // GET: LoanForms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rs_loan_form rs_loan_form = db.rs_loan_form.Find(id);
            if (rs_loan_form == null)
            {
                return HttpNotFound();
            }
            return View(rs_loan_form);
        }

        // POST: LoanForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanId,RequestNo,RequestorId,StartDate,EndDate,Company,ContactPersion,Telephone,Email,Address,Purpose,CreatedDate,UpdatedDate,Invalid,Returned,Signed,UploadedFile")] rs_loan_form rs_loan_form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rs_loan_form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rs_loan_form);
        }

        // GET: LoanForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rs_loan_form rs_loan_form = db.rs_loan_form.Find(id);
            if (rs_loan_form == null)
            {
                return HttpNotFound();
            }
            return View(rs_loan_form);
        }

        // POST: LoanForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rs_loan_form rs_loan_form = db.rs_loan_form.Find(id);
            db.rs_loan_form.Remove(rs_loan_form);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
