﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PapayaX2.Database;

namespace PapayaX2.Controllers
{
    public class Test : Controller
    {
        private PapayaEntities db = new PapayaEntities();

        // GET: Test
        public ActionResult Index()
        {
            var rs_user = db.rs_user.Include(s => s.rs_user_group);
            return View(rs_user.ToList());
        }

        // GET: Test/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rs_user rs_user = db.rs_user.Find(id);
            if (rs_user == null)
            {
                return HttpNotFound();
            }
            return View(rs_user);
        }

        // GET: Test/Create
        public ActionResult Create()
        {
            //ViewBag.CompanyId = new SelectList(db.rs_company, "CompanyId", "CompanyCode");
            ViewBag.GroupId = new SelectList(db.rs_user_group, "GroupId", "Name");
            return View();
        }

        // POST: Test/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Username,Password,FullName,MobileNumber,Email,CompanyId,GroupId,UserType,IsBackEnd,FlagActive,UserEntry,DateEntry,UserUpdate,DateUpdate,Department")] rs_user rs_user)
        {
            if (ModelState.IsValid)
            {
                db.rs_user.Add(rs_user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ////ViewBag.CompanyId = new SelectList(db.rs_company, "CompanyId", "CompanyCode", rs_user.CompanyId);
            ViewBag.GroupId = new SelectList(db.rs_user_group, "GroupId", "Name", rs_user.GroupId);
            return View(rs_user);
        }

        // GET: Test/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rs_user rs_user = db.rs_user.Find(id);
            if (rs_user == null)
            {
                return HttpNotFound();
            }
            ////ViewBag.CompanyId = new SelectList(db.rs_company, "CompanyId", "CompanyCode", rs_user.CompanyId);
            ViewBag.GroupId = new SelectList(db.rs_user_group, "GroupId", "Name", rs_user.GroupId);
            return View(rs_user);
        }

        // POST: Test/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Username,Password,FullName,MobileNumber,Email,CompanyId,GroupId,UserType,IsBackEnd,FlagActive,UserEntry,DateEntry,UserUpdate,DateUpdate,Department")] rs_user rs_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rs_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CompanyId = new SelectList(db.rs_company, "CompanyId", "CompanyCode", rs_user.CompanyId);
            ViewBag.GroupId = new SelectList(db.rs_user_group, "GroupId", "Name", rs_user.GroupId);
            return View(rs_user);
        }

        // GET: Test/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rs_user rs_user = db.rs_user.Find(id);
            if (rs_user == null)
            {
                return HttpNotFound();
            }
            return View(rs_user);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rs_user rs_user = db.rs_user.Find(id);
            db.rs_user.Remove(rs_user);
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
