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
    public class dataController : Controller
    {
        private testEntities db = new testEntities();
        //
        // GET: /data/

        public ActionResult Index()
        {
            string dataStr = null;
            var list = db.data.SqlQuery("SELECT * FROM `data` where DATE_SUB(CURDATE(), INTERVAL 7 DAY) <= date(date);").ToList();
            foreach (var listItem in list)
            {
                dataStr += listItem.value1 + "," + listItem.value2 + "," + listItem.value3 + "," + listItem.value4 + "," + listItem.value5 + "," + listItem.value6 + ","
                    + listItem.value7 + "," + listItem.value8 + "," + listItem.value9 + "," + listItem.value10 + "," + listItem.value11 + "," + listItem.value12 + ","
                    + listItem.value13 + "," + listItem.value14 + "," + listItem.value15 + "," + listItem.value16 + "," + listItem.value17 + "," + listItem.value18 + ","
                    + listItem.value19 + "," + listItem.value20 + "," + listItem.value21 + "," + listItem.value22 + "," + listItem.value23 + "," + listItem.value24 + ",";
            }
            dataStr=dataStr.Substring(0, dataStr.Length - 1);
            ViewBag.data = dataStr;
            ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId");
            ViewBag.during = new SelectList(new string[] { "7", "14", "30", "37" });
            ViewBag.threshold = new SelectList(new string[] { "0", "1", "2" });
            ViewBag.grading = new SelectList(new string[] { "1", "2", "4","8" });
            return View();
        }

    }
}
