using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simple_Twitter.Entities;
using Simple_Twitter.Models;
using Simple_Twitter.Utilities;

namespace Simple_Twitter.Controllers
{
    public class SimpleTwitterController : Controller
    {
        //IDBConfig dbConfig = new MongoDBConfiguration();
        SimpleTwitterModel simpleTwitterModel;
        public SimpleTwitterController(IDBConfig dbConfig)
        {
            simpleTwitterModel = new SimpleTwitterModel(dbConfig);
        }
            

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string handle, string tweet)
        {
            const int maxTweetLength = 140;
            ModelState.Clear();
            if (string.IsNullOrEmpty(handle))
            {
                ModelState.AddModelError("Handle", "Handle is required");
            }
            if (string.IsNullOrEmpty(tweet))
            {
                ModelState.AddModelError("Tweet", "Tweet is required");
            }
            else if (tweet.Length > maxTweetLength)
            {
                string errorMessage = string.Format(
                        "The tweet cannot exceed {0} characters", maxTweetLength);
                ModelState.AddModelError("Tweet", errorMessage);
            }
            if (ModelState.IsValid)
            {
                ViewBag.Handle = handle;
                ViewBag.Tweet = tweet;
                SimpleTwitter simpleTwitter = new SimpleTwitter(handle, tweet, DateTime.Now.ToString());
                simpleTwitterModel.create(simpleTwitter);
            }
            return View();
        }

        [HttpGet]
        public ActionResult ViewTweets()
        {
            ViewBag.Tweets = new List<SimpleTwitter>();
            return View();
        }

        [HttpPost]
        public ActionResult ViewTweets(string handle)
        {
            ModelState.Clear();
            if (string.IsNullOrEmpty(handle))
            {
                ModelState.AddModelError("Handle", "Handle is required");
            }
            if (ModelState.IsValid)
            {                
                var tweets = simpleTwitterModel.find(handle);   
                foreach (SimpleTwitter tweet in tweets)
                {
                    System.Console.WriteLine(tweet.Time.ToString());
                }
                ViewBag.Tweets = tweets;               
            }
            return View();
        }
    }
}