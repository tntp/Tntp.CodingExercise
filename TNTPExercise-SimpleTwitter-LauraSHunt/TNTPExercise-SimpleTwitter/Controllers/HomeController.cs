using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNTPExercise_SimpleTwitter.Models;

namespace TNTPExercise_SimpleTwitter.Controllers
{
    public class HomeController : Controller
    {
        SimpleTwitterEntities db = new SimpleTwitterEntities();

        public ActionResult Index()
        {
            if (TempData.ContainsKey("ModelState"))
            {
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            }

            return View(LoadModel());
        }

        [HttpPost]
        public ActionResult Index(UserFeed model)
        {
            TempData["MessageSubmit"] = "";

            if (ModelState.IsValid)
            {
                db.usp_AddUserComment(model.Name, model.Comment);
                ModelState.Clear();
                TempData["MessageSubmit"] = "Thanks, " + model.Name + ". Your comment is posted.";
            }

            TempData["ModelState"] = ModelState;

            return RedirectToAction("Index");
        }

        private UserFeed LoadModel()
        {
            UserFeed userFeed = new UserFeed()
            {
                Name = "",
                Comment = "",
                UserComments = db.UserComments.ToList().OrderByDescending(c => c.CreatedOn)
            };

            return userFeed;
        }
    }
}