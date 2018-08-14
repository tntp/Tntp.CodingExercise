using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using jrmmba_SimpleTwitter.Models;

namespace jrmmba_SimpleTwitter.Controllers
{
    public class HomeController : Controller
    {
        dbSimpleTwitterEntities _db;
        public HomeController()
        {
            _db = new dbSimpleTwitterEntities();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListTweets()
        {
            ViewData.Model = (from t in _db.tblTweets orderby t.dateCreate descending, t.tweetID descending select t).ToList();
            return View();
        }

        public ActionResult AddTweet()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTweet([Bind(Include = "userName,theTweet,createDate")] tblTweet tblTweeter)
        {
            try
            {
                tblTweeter.dateCreate = DateTime.Now;

                _db.tblTweets.Add(tblTweeter);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}