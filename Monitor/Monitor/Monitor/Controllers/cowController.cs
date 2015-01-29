using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monitor.Models;

namespace Monitor.Controllers
{
    public class cowController : Controller
    {
        private testEntities db = new testEntities();

        //
        // GET: /cow/

        public ActionResult Index()
        {
            return View(db.cow.ToList());
        }

        //
        // GET: /cow/Details/5

        public ActionResult Details(int id = 0)
        {
            cow cow = db.cow.Find(id);
            if (cow == null)
            {
                return HttpNotFound();
            }
            return View(cow);
        }

        //
        // GET: /cow/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /cow/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(cow cow)
        {
            if (ModelState.IsValid)
            {
                var item = db.cow.AsNoTracking().SingleOrDefault(g => g.cowId == cow.cowId);
                if (item != null && item.id != cow.id)
                {
                    ViewBag.error = "奶牛编号重复，请修改";
                    return View(cow);
                }
                db.cow.Add(cow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cow);
        }

        //
        // GET: /cow/Edit/5

        public ActionResult Edit(int id = 0)
        {
            cow cow = db.cow.Find(id);
            if (cow == null)
            {
                return HttpNotFound();
            }
            return View(cow);
        }

        //
        // POST: /cow/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(cow cow)
        {
            if (ModelState.IsValid)
            {
                var item = db.cow.AsNoTracking().SingleOrDefault(g => g.cowId == cow.cowId);
                if (item != null && item.id != cow.id)
                {
                    ViewBag.error = "奶牛编号重复，请修改";
                    return View(cow);
                }
                db.Entry(cow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cow);
        }

        //
        // GET: /cow/Delete/5

        public ActionResult Delete(int id = 0)
        {
            cow cow = db.cow.Find(id);
            if (cow == null)
            {
                return HttpNotFound();
            }
            return View(cow);
        }

        //
        // POST: /cow/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cow cow = db.cow.Find(id);
            db.cow.Remove(cow);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}