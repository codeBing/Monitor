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
    public class deviceController : Controller
    {
        private testEntities db = new testEntities();

        //
        // GET: /device/

        public ActionResult Index()
        {
            return View(db.cow_device.ToList());
        }

        //
        // GET: /device/Details/5

        public ActionResult Details(int id = 0)
        {
            cow_device cow_device = db.cow_device.Find(id);
            if (cow_device == null)
            {
                return HttpNotFound();
            }
            return View(cow_device);
        }

        //
        // GET: /device/Create

        public ActionResult Create()
        {
            ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId");
            return View();
        }

        //
        // POST: /device/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(cow_device cow_device)
        {

            if (ModelState.IsValid)
            {
                //把十六进制转换成十进制
                cow_device.deviceId = ConvertToTen(cow_device.showDeviceId);
                //如果转换出现异常则返回页面
                if (cow_device.deviceId == null)
                {
                    ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId");
                    ViewBag.error = "请输入十六进制设备编号";
                    return View(cow_device);
                }
                var item = db.cow_device.AsNoTracking().SingleOrDefault(g => g.deviceId == cow_device.deviceId);
                
                if (item != null && item.id != cow_device.id)
                {
                    ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId");
                    ViewBag.error = "设备编号重复，请修改";
                    return View(cow_device);
                }
                
                if (cow_device.cowId != null)
                {
                    cow_device device = db.cow_device.AsNoTracking().SingleOrDefault(g => g.cowId == cow_device.cowId);
                    if (device != null)
                    {
                        device.cowId = null;
                        db.Entry(device).State = EntityState.Modified;
                    }
                }
                
                db.cow_device.Add(cow_device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId");
            return View(cow_device);
        }

        //
        // GET: /device/Edit/5

        public ActionResult Edit(int id = 0)
        {
            cow_device cow_device = db.cow_device.Find(id);
            if (cow_device == null)
            {
                return HttpNotFound();
            }
            ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId", cow_device.cowId);
            return View(cow_device);
        }

        //
        // POST: /device/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(cow_device cow_device)
        {
            if (ModelState.IsValid)
            {
                //把十六进制转换成十进制
                cow_device.deviceId = ConvertToTen(cow_device.showDeviceId);
                //如果转换出现异常则返回页面
                if (cow_device.deviceId == null)
                {
                    ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId");
                    ViewBag.error = "请输入十六进制设备编号";
                    return View(cow_device);
                }

                var item = db.cow_device.AsNoTracking().SingleOrDefault(g => g.deviceId == cow_device.deviceId);
                if (item != null && item.id != cow_device.id)
                {
                    ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId");
                    ViewBag.error = "设备编号重复，请修改";
                    return View(cow_device);
                }
                if (cow_device.cowId != null)
                {
                    cow_device device = db.cow_device.AsNoTracking().SingleOrDefault(g => g.cowId == cow_device.cowId);
                    if (device != null && device.deviceId != cow_device.deviceId)
                    {
                        device.cowId = null;
                        db.Entry(device).State = EntityState.Modified;
                    }
                }
                db.Entry(cow_device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId");
            return View(cow_device);
        }

        //
        // GET: /device/Delete/5

        public ActionResult Delete(int id = 0)
        {
            cow_device cow_device = db.cow_device.Find(id);
            if (cow_device == null)
            {
                return HttpNotFound();
            }
            return View(cow_device);
        }

        //
        // POST: /device/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cow_device cow_device = db.cow_device.Find(id);
            db.cow_device.Remove(cow_device);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //十六进制转换成十进制
        private string ConvertToTen(string number)
        {
            char[] nums = number.ToCharArray();
            int total = 0;
            try
            {
                for (int i = 0; i < nums.Length; ++i)
                {
                    String strNum = nums[i].ToString().ToUpper();
                    switch (strNum)
                    {
                        case "A":
                            strNum = "10";
                            break;
                        case "B":
                            strNum = "11";
                            break;
                        case "C":
                            strNum = "12";
                            break;
                        case "D":
                            strNum = "13";
                            break;
                        case "E":
                            strNum = "14";
                            break;
                        case "F":
                            strNum = "15";
                            break;
                        default:
                            break;
                    }
                    double power = Math.Pow(16, Convert.ToDouble(nums.Length - i - 1));
                    total += Convert.ToInt32(strNum) * Convert.ToInt32(power);
                }
            }
            catch (System.Exception ex)
            {
                String strErorr = ex.ToString();
                return null;
            }
            return total.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}