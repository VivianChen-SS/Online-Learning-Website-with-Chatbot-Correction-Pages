using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestGroup.Models;

namespace TestGroup.Controllers
{
    public class HomeController : Controller
    {
        testgroupEntities db = new testgroupEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string id)
        {
            StudentTestGroup student = db.StudentTestGroup
                .Where(a => a.StudentID == id)
                .SingleOrDefault();
            if (student != null)
            {
                Session["studentID"] = student.StudentID;
                return RedirectToAction("DoubleCheck", new { name = student.Name });
            }
            else
            {
                ViewBag.findNone = true;
                return View();
            }

        }

        public ActionResult DoubleCheck(string name)
        {
            Session["studentName"] = name;
            ViewBag.name = name;
            return View();
        }

        public ActionResult Logout()
        {
            Session["studentID"] = null;
            return View();
        }

        public ActionResult Index(string doAlert)
        {
            if (Session["studentID"] != null)
            {
                ViewBag.studentName = Session["studentName"];
                if (doAlert != "")
                {
                    switch (doAlert)
                    {
                        case "a": ViewBag.doAlert = "該單元已經完成嘍！辛苦了！ 請點擊「查看紀錄」連結，查看成果。"; break;
                        case "b": ViewBag.doAlert = "該單元「影片」時段未開放 (o*｡_｡)o sorry"; break;
                        case "c": ViewBag.doAlert = "該單元「答題」時段未開放 (o*｡_｡)o sorry"; break;
                        case "d": ViewBag.doAlert = "你這題答錯了喔！但是，該單元「訂正」時段未開放 (o*｡_｡)o sorry"; break;
                        default: break;
                    }
                }

                return View(db.Unit.ToList());
            }
            return RedirectToAction("Login");
        }

        [HttpPost, ActionName("Index")]
        public ActionResult KillOrder()
        {
            String studentID = Session["studentID"] + "";
            db.StudentTestGroup_Unit_Junction.RemoveRange(
                db.StudentTestGroup_Unit_Junction.Where(a => a.StudentID == studentID)
                );
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Resources()
        {
            if (Session["studentID"] != null)
            {
                string studentID = Session["studentID"] + "";
                var records = db.StudentTestGroup_Unit_Junction.Where(a => a.StudentID == studentID).OrderBy(a => a.UnitID);
                List <Unit> units = new List<Unit>();
                foreach (var record in records)
                {
                    Unit unit = db.Unit.Where(a => a.UnitID == record.UnitID).Single();
                    units.Add(unit);
                }
                Object[] idList = db.Unit.OrderBy(a => a.UnitID).Select(a => a.UnitID).ToArray();
                ViewBag.idList = idList;
                return View(units);
            }
            return RedirectToAction("Login");
        }



        public ActionResult Contact()
        {
            return View();
        }
    }
}