using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monitor.Models;
using HanXing;

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
            string hanxingData = null;
            string dateStr = null;
            string cowId = db.cow.First().cowId;
            transfer(cowId, 7, 0,out dataStr,out dateStr);
            hanxingData = computeHan(dataStr);
            ViewBag.data = dataStr;
            ViewBag.date = dateStr;
            ViewBag.hanxingData = hanxingData;
            ViewBag.cowId = new SelectList(db.cow, "cowId", "cowId");
            ViewBag.during = new SelectList(new string[] { "7", "14", "30", "37" });
            ViewBag.threshold = new SelectList(new string[] { "0", "1", "2" });
            ViewBag.grading = new SelectList(new string[] { "1", "2", "4", "8" });
            return View();
        }

        [HttpPost]
        public ActionResult show(string cowId, string during, string threshold, string grading)
        {
            string dataStr = null;
            string dateStr = null;
            string hanxingData = null;
            transfer(cowId, int.Parse(during), int.Parse(threshold), out dataStr, out dateStr);
            hanxingData = computeHan(dataStr);

            return Json(new
            {
                data = dataStr,
                date = dateStr,
                hanxing = hanxingData
            });
        }

        public string computeHan(string dataStr)
        {
            dataStr = ComputeHanXing.result(dataStr);
            return dataStr;
        }

        public void transfer(string cowId,int dayCount,int threshold,out string dataStr,out string dateStr)
        {
            DateTime today = DateTime.Now.AddDays(-dayCount+1);
            dateStr = "";
            dataStr = "";
            for (int i = 0; i < dayCount; ++i)
            {
                string cmd = "SELECT * FROM `data` where date='" + today.ToString("yyyy/MM/dd") + "' AND cowId = " + cowId + " AND threshold = " + threshold + ";";
                var list = db.data.SqlQuery(cmd).ToList();
                if (list.Count!=0)
                {
                    var listItem = list[0];
                    dataStr += listItem.value1 + "," + listItem.value2 + "," + listItem.value3 + "," + listItem.value4 + "," + listItem.value5 + "," + listItem.value6 + ","
                            + listItem.value7 + "," + listItem.value8 + "," + listItem.value9 + "," + listItem.value10 + "," + listItem.value11 + "," + listItem.value12 + ","
                            + listItem.value13 + "," + listItem.value14 + "," + listItem.value15 + "," + listItem.value16 + "," + listItem.value17 + "," + listItem.value18 + ","
                            + listItem.value19 + "," + listItem.value20 + "," + listItem.value21 + "," + listItem.value22 + "," + listItem.value23 + "," + listItem.value24 + ",";
                    dateStr += listItem.date + ",";
                }
                else
                {
                    dataStr += "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,";
                    dateStr += today.ToShortDateString() + ",";
                }
                today = DateTime.Now.AddDays(-dayCount+2+i);
            }
            if (dataStr != null)
            {
                dataStr = dataStr.Substring(0, dataStr.Length - 1);
            }
            if (dateStr != null)
            {
                dateStr = dateStr.Substring(0, dateStr.Length - 1);
            }
        }

    }
}
