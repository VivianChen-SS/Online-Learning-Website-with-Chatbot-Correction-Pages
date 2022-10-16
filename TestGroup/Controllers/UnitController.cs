using TestGroup.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Text;

namespace TestGroup.Controllers
{
    public class UnitController : Controller
    {
        testgroupEntities db = new testgroupEntities();


        // GET: Unit
        public ActionResult WatchVideo(string id)
        {
            if (Session["studentID"] != null && id != null) //防呆未登入
            {
                //sortingHat(id);

                #region sortingHat

                Session["unitID"] = id; //這個到底用不用得到啊？？？
                string studentID = (string)Session["studentID"];


                //找這個人的這一題有沒有紀錄
                StudentTestGroup_Unit_Junction record = db.StudentTestGroup_Unit_Junction
                    .Where(a => a.StudentID == studentID && a.UnitID == id)
                    .SingleOrDefault();

                //如果這個人沒紀錄的話就創一個空的
                if (record == null)
                {
                    StudentTestGroup_Unit_Junction stuffToAdd = new StudentTestGroup_Unit_Junction()
                    {
                        StudentID = studentID,
                        UnitID = id,
                        VideoWatched = false,
                        ChatbotFinished = false,
                        ConceptMapAnswer = ""
                    };
                    db.StudentTestGroup_Unit_Junction.Add(stuffToAdd);
                    db.SaveChanges();

                    Session["record"] = stuffToAdd;
                }

                //如果有紀錄的話，就看這人做到哪一階段，就redirect到哪邊
                else
                {
                    Session["record"] = record;
                    if (record.ChatbotFinished && record.Result == false && record.ExtendQuestionAnswer == null) //答錯，已經看過concept map，還沒答相似題
                    {
                        return RedirectToAction("ExtendQ", new { secret = Crypto.HashPassword(record.StudentID) });
                    }
                    if (record.ChatbotFinished && record.Result == false) //答錯，已經看過concept map
                    {
                        return RedirectToAction("Index", "Home", new { doAlert = "a" });
                    }
                    if (!record.ChatbotFinished && record.Result == false) //答錯，未看concept map
                    {
                        return RedirectToAction("Chatbot");
                    }
                    if (record.Answer != null && record.Result == true) //答對
                    {
                        return RedirectToAction("Index", "Home", new { doAlert = "a" });
                    }
                    if (record.VideoWatched && record.Answer == null) //看完影片，未答題
                    {
                        return RedirectToAction("Question");
                    }
                }
                #endregion


                //Set ViewBags
                Unit unit = db.Unit.Where(u => u.UnitID == id).Single();
                if (unit.VideoStartDate.Value < DateTime.Now && unit.VideoEndDate > DateTime.Now) //在開放時間內 (其實index的button已經擋掉了，這裡是防直接用網址連進來的)
                {
                    return View(unit);
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { doAlert = "b" });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult WatchVideo(DateTime startTime)
        {
            //StudentTestGroup_Unit_Junction record = (StudentTestGroup_Unit_Junction)Session["record"];
            if (ModelState.IsValid)
            {
                string studentID = Session["studentID"] + "";
                string unitID = Session["unitID"] + "";
                StudentTestGroup_Unit_Junction recFromDB = db.StudentTestGroup_Unit_Junction
                    .Where(a => a.StudentID == studentID && a.UnitID == unitID)
                    .Single();
                recFromDB.VideoWatched = true;
                recFromDB.VideoElapsedTime = (int)DateTime.Now.Subtract(startTime).TotalSeconds;
                db.SaveChanges();
                Session["record"] = recFromDB;
                return RedirectToAction("Question");
            }
            return View();
        }

        public ActionResult Question()
        {
            if (Session["studentID"] != null)
            {
                //先找有沒有紀錄
                StudentTestGroup_Unit_Junction record = (StudentTestGroup_Unit_Junction)Session["record"];

                //我不知道為什麼session == null會失靈，結果產生以下奇葩的code @@"，but it still works......
                //有紀錄的人......
                if (record != null)
                {
                    if (record.Answer != null || !record.VideoWatched) //已經答題or影片沒看的人，都給老子滾回首頁，來亂的
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    Unit unit = record.Unit; //有紀錄，尚未達題者請進，應該是從WatchVideo來的 or 看完影片中途跳開的
                    if (unit.QuestionStartDateTime.Value < DateTime.Now && unit.QuestionEndDateTime > DateTime.Now) //在開放時間內 (其實index的button已經擋掉了，這裡是防直接用網址連進來的)
                    {
                        return View(unit);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { doAlert = "c" });
                    }
                }
                //else
                //{
                //沒紀錄的話給老子滾回首頁
                return RedirectToAction("Index", "Home");
                //}
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult Question(String answer, DateTime startTime)
        {
            string studentID = Session["studentID"] + "";
            string unitID = Session["unitID"] + "";
            StudentTestGroup_Unit_Junction recFromDB = db.StudentTestGroup_Unit_Junction
                .Where(a => a.StudentID == studentID && a.UnitID == unitID)
                .Single();
            recFromDB.QuestionElapsedTime = (int)DateTime.Now.Subtract(startTime).TotalSeconds;
            recFromDB.Answer = answer;

            //對答案
            String[] answers = db.Unit
                .Where(a => a.UnitID == unitID)
                .Single()
                .Answer
                .Split(',');
            recFromDB.Result = Array.Exists(answers, element => element == answer.ToUpper());
            db.SaveChanges();
            Session["record"] = recFromDB;
            if ((bool)recFromDB.Result)
            {
                return RedirectToAction("Congrats");
            }
            return RedirectToAction("Chatbot");
        }

        public ActionResult Congrats()
        {
            if (Session["studentID"] != null)
            {
                return View();
            }
            return View();
        }

        public ActionResult Chatbot()
        {
            if (Session["studentID"] != null)
            {
                //先找有沒有紀錄
                StudentTestGroup_Unit_Junction record = (StudentTestGroup_Unit_Junction)Session["record"];

                //我不知道為什麼session == null會失靈，結果產生以下奇葩的code @@"，but it still works......
                //有紀錄的人......
                if (record != null)
                {
                    if (record.ChatbotFinished || record.Result == true || record.Result == null) //已經讀過的人給老子滾回首頁，來亂的 (我要申明！！record.Result是nullable的！！！ ((( 從來沒想過有天會寫出 == true的智障code (╬▔皿▔)╯  )
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    Unit unit = record.Unit; //有紀錄，尚未達題者請進，應該是從WatchVideo來的 or 看完影片中途跳開的
                    if (unit.ConceptMapStartDate.Value < DateTime.Now && unit.ConceptMapEndDate > DateTime.Now) //在開放時間內 (其實index的button已經擋掉了，這裡是防直接用網址連進來的)
                    {
                        //如果有鍋的chatbot訂正劇本
                        if (unit.MultipleChatbot == true)
                        {
                            string[] agentIDs = unit.MultipleChatbotAgentID.Split('/');
                            ViewBag.AgentID = agentIDs[int.Parse(record.Answer) - 1]; //上面split完了，index從0開始，所以選項編號要減一
                        }
                        //製作完成連結密碼
                        ViewBag.secret = Crypto.HashPassword(record.StudentID);
                        return View(unit);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { doAlert = "d" });
                    }
                }
                else
                {   //沒紀錄的話給老子滾回首頁
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //ExtendQuestion 為了避免跟資料表同名而縮減
        public ActionResult ExtendQ(string secret)
        {
            if (Session["studentID"] != null)
            {
                //先找有沒有紀錄
                StudentTestGroup_Unit_Junction record = (StudentTestGroup_Unit_Junction)Session["record"];

                //我不知道為什麼session == null會失靈，結果產生以下奇葩的code @@"，but it still works......
                //有紀錄的人......
                if (record != null)
                {
                    if (!(record.Result == false && record.ExtendQuestionAnswer == null)) //如果不是需要回答相似題的人，都滾
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    Unit unit = record.Unit; //有紀錄，尚未達題者請進，應該是從WatchVideo來的 or 看完影片中途跳開的
                    if (unit.ConceptMapStartDate.Value < DateTime.Now && unit.ConceptMapEndDate > DateTime.Now) //在開放時間內 (其實index的button已經擋掉了，這裡是防直接用網址連進來的) 不要驗證ConceptMapRead，因為底下才會改，不會有作弊嫌疑，因為secret猜不到
                    {
                        if (Crypto.VerifyHashedPassword(secret, record.StudentID))
                        {
                            //紀錄已經聊完聊天室
                            StudentTestGroup_Unit_Junction recFromDB = db.StudentTestGroup_Unit_Junction
                            .Where(a => a.StudentID == record.StudentID && a.UnitID == record.UnitID)
                            .Single();
                            recFromDB.ChatbotFinished = true;
                            db.SaveChanges();

                            //存Session
                            record = recFromDB;

                            //找到ExtendQuestion 
                            ExtendQuestion extendQuestion = db.ExtendQuestion
                                .Where(u => u.UnitID == unit.UnitID)
                                .Single();

                            //整理ViewBag
                            ViewBag.conceptMapLink = unit.ConceptMapLink;
                            ViewBag.conceptMapDescription = unit.ConceptMapDescription;
                            setTitle(unit);
                            return View(extendQuestion);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { doAlert = "!@#$%^&*(" });
                    }
                }
                //沒紀錄的話給老子滾回首頁
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult CheckExtendQ(String answer)
        {
            StudentTestGroup_Unit_Junction record = (StudentTestGroup_Unit_Junction)Session["record"];
            StudentTestGroup_Unit_Junction recFromDB = db.StudentTestGroup_Unit_Junction
                .Where(a => a.StudentID == record.StudentID && a.UnitID == record.UnitID)
                .Single();

            if (recFromDB.ExtendQuestionResult == null)
            {
                recFromDB.ExtendQuestionAnswer = answer;

                //對答案
                String[] answers = db.ExtendQuestion
                    .Where(a => a.UnitID == record.UnitID)
                    .Single()
                    .Answer
                    .Split(',');
                recFromDB.ExtendQuestionResult = Array.Exists(answers, element => element == answer.ToUpper());
                db.SaveChanges();

                record = recFromDB;

                ViewBag.Result = recFromDB.ExtendQuestionResult;
                string[] mapAns = { "?", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                string ans = mapAns[int.Parse(answers[0])];
                ViewBag.Answer = ans;
                return PartialView();
            }
            return View("Error");
        }

        //public ActionResult temp()
        //{
        //    return RedirectToAction("Index", "Home", new { doAlert = "e" });
        //}

        //2021.11.19 老師說要證明chatbot有效，所以在跟chatbot聊完了以後，要再考一個相似題給他，所以原本的Finish就不能用了QAQ
        //public ActionResult Finished(string secret)
        //{
        //    if (Session["studentID"] != null)
        //    {
        //        //先找有沒有紀錄
        //        StudentTestGroup_Unit_Junction record = (StudentTestGroup_Unit_Junction)Session["record"];

        //        //我不知道為什麼session == null會失靈，結果產生以下奇葩的code @@"，but it still works......
        //        //有紀錄的人......
        //        if (record != null)
        //        {
        //            if (record.ChatbotFinished) //已經完成聊天的，都給老子滾回首頁，來亂的
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //            Unit unit = record.Unit; //有紀錄，尚未達題者請進，應該是從WatchVideo來的 or 看完影片中途跳開的
        //            if (unit.ConceptMapStartDate.Value < DateTime.Now && unit.ConceptMapEndDate > DateTime.Now) //在開放時間內 (其實index的button已經擋掉了，這裡是防直接用網址連進來的)
        //            {
        //                if (Crypto.VerifyHashedPassword(secret, record.StudentID))
        //                {
        //                    StudentTestGroup_Unit_Junction recFromDB = db.StudentTestGroup_Unit_Junction
        //                    .Where(a => a.StudentID == record.StudentID && a.UnitID == record.UnitID)
        //                    .Single();
        //                    recFromDB.ChatbotFinished = true;
        //                    db.SaveChanges();
        //                    record = recFromDB;
        //                    return View();
        //                }
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Home", new { doAlert = "!@#$%^&*(" });
        //            }
        //        }
        //        //else
        //        //{
        //        //沒紀錄的話給老子滾回首頁
        //        return RedirectToAction("Index", "Home");
        //        //}
        //    }
        //    return RedirectToAction("Login", "Home");
        //}

        public ActionResult Progress(string id)
        {
            if (Session["studentID"] != null)
            {
                ViewBag.selectedUnitID = id;

                //弄那一串標題選項
                var unitList = db.Unit.ToList();
                List<UnitTitleViewModels> unitTitleList = new List<UnitTitleViewModels>();
                foreach (var item in unitList)
                {
                    unitTitleList.Add(new UnitTitleViewModels
                    {
                        unitID = item.UnitID,
                        week = item.Week,
                        number = item.Number
                    });
                }
                //系統LINQ版本太舊，以下不能用，不是code寫錯@@"...............於是我只好像上面那樣，開一個ViewModels了
                //List<UnitTitleViewModels> unitTitleList = db.Unit.Select(u => new UnitTitleViewModels { 
                //    unitID = u.UnitID,
                //    week = week[u.Week],
                //    number = questionOrder[u.Number] 
                //}).ToList();

                //計算該使用者的總進度
                string studentID = Session["studentID"] + "";
                var recsFromDB = db.StudentTestGroup_Unit_Junction
                    .Where(a => a.StudentID == studentID)
                    .ToList();
                int correct = 0;
                int incorrect = 0;

                foreach (var rec in recsFromDB)
                {
                    if (rec.Result != null)
                    {
                        if (rec.Result == true)
                        {
                            correct++;
                        }
                        else
                        {
                            incorrect++;
                        }

                    }
                }
                ViewBag.correct = correct;
                ViewBag.incorrect = incorrect;
                ViewBag.unfinished = db.Unit.ToList().Count() - correct - incorrect;

                return View(unitTitleList);
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult UnitProcess(string id)
        {
            id = id == null ? db.Unit.First().UnitID : id;
            setTitle(db.Unit.Where(a => a.UnitID == id).Single());
            string studentID = Session["studentID"] + "";
            StudentTestGroup_Unit_Junction recFromDB = db.StudentTestGroup_Unit_Junction
                .Where(a => a.StudentID == studentID && a.UnitID == id).SingleOrDefault();
            Session["record"] = recFromDB;
            if (recFromDB == null)
            {
                recFromDB = new StudentTestGroup_Unit_Junction
                {
                    StudentID = studentID,
                    UnitID = id,
                    VideoWatched = false,
                    ChatbotFinished = false
                };
            }

            //if (recFromDB == null){ //【未完成】沒看影片，沒答題
            //    ViewBag.Video = "../picture/pen_blue.png";
            //    ViewBag.Question = "../picture/lock_gray.png";
            //ViewBag.Finished = "../picture/lock_gray.png";
            //}
            if (recFromDB.ExtendQuestionAnswer != null)
            { //【完成】答錯並讀完概念構圖也完成相似題的乖寶寶
                ViewBag.Video = "../picture/check_green.png";
                ViewBag.Question = "../picture/cross_red.png";
                ViewBag.Conceptmap = "../picture/check_green.png";
                ViewBag.ExtendQ = recFromDB.ExtendQuestionResult == true?  "../picture/check_green.png" : "../picture/cross_red.png";
                ViewBag.Finished = "../picture/check_green.png";
                recFromDB = db.StudentTestGroup_Unit_Junction
                .Where(a => a.StudentID == studentID && a.UnitID == id)
                .Include(a => a.Unit)
                .SingleOrDefault();
            }
            else if (recFromDB.ChatbotFinished)
            { //【完成】答錯並讀完概念構圖
                ViewBag.Video = "../picture/check_green.png";
                ViewBag.Question = "../picture/cross_red.png";
                ViewBag.Conceptmap = "../picture/check_green.png";
                ViewBag.ExtendQ = "../picture/pen_blue.png";
                ViewBag.Finished = "../picture/lock_gray.png";
                recFromDB = db.StudentTestGroup_Unit_Junction
                .Where(a => a.StudentID == studentID && a.UnitID == id)
                .Include(a => a.Unit)
                .SingleOrDefault();
            }
            else if (recFromDB.Result == true)
            {//【完成】答對
                ViewBag.Video = "../picture/check_green.png";
                ViewBag.Question = "../picture/check_green.png";
                ViewBag.Finished = "../picture/check_green.png";
            }
            else if (recFromDB.Result == false && !recFromDB.ChatbotFinished)
            { //【未完成】答錯，但是還沒看概念構圖
                ViewBag.Video = "../picture/check_green.png";
                ViewBag.Question = "../picture/cross_red.png";
                ViewBag.Conceptmap = "../picture/pen_blue.png";
                ViewBag.ExtendQ = "../picture/lock_gray.png";
                ViewBag.Finished = "../picture/lock_gray.png";
                recFromDB = db.StudentTestGroup_Unit_Junction
                .Where(a => a.StudentID == studentID && a.UnitID == id)
                .Include(a => a.Unit)
                .SingleOrDefault();
            }
            else if (recFromDB.Result == null && recFromDB.VideoWatched)
            { //【未完成】看完影片，未答題
                ViewBag.Video = "../picture/check_green.png";
                ViewBag.Question = "../picture/pen_blue.png";
                ViewBag.Finished = "../picture/lock_gray.png";
            }
            else
            {
                //【未完成】沒看影片，沒答題
                ViewBag.Video = "../picture/pen_blue.png";
                ViewBag.Question = "../picture/lock_gray.png";
                ViewBag.Finished = "../picture/lock_gray.png";
            }
            return PartialView(recFromDB);
        }

        private void setTitle(Unit unit)
        {
            ViewBag.week = unit.Week;
            ViewBag.questionNumber = unit.Number;
        }
    }
}