using SimpleTwitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace SimpleTwitter.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            SimpleTwitterEntities4 db = new SimpleTwitterEntities4();

            List<CommentViewModel> cvmList_Local = new List<CommentViewModel>();

            // Manually create the new list from the database table
            foreach (var item in db.commentTables)
            {
                var cvm = new CommentViewModel()
                {
                    Comment_Id = item.Comment_Id,
                    Comment_Name = item.Comment_Name,
                    Comment_Comment = item.Comment_Comment,
                    Comment_Date = item.Comment_Date
                };

            cvmList_Local.Add(cvm);
            }

            cvmList_Local.Sort((x, y) => -x.Comment_Date.CompareTo(y.Comment_Date)); // sort the list descending

            // Add to the CommentViewMaster - holds the list and the new values that are typed
            var cvMaster = new CommentViewMaster()
            {
                CVMList = cvmList_Local
            };
            

            return View(cvMaster);
        }

        

        [HttpPost]
        public ActionResult Index(CommentViewMaster model)
        {
            if (ModelState.IsValid == true)
            {

                try
                {
                    if (model is null ||
                        model.Comment_Name is null ||
                        model.Comment_Comment is null)
                    {
                        return View(model); // We shouldn't be here in this case
                    }

                    // Build a container for the new record
                    var c = new commentTable()
                    {
                        Comment_Name = model.Comment_Name,
                        Comment_Comment = model.Comment_Comment,
                        Comment_Date = System.DateTime.Now
                    };

                    // Save to the database
                SimpleTwitterEntities4 db = new SimpleTwitterEntities4();
                    // Manually create the new list from the database table
                    foreach (var item in db.commentKeys)
                    {
                        c.Comment_Id = item.Next_Id;
                    }

                    // Delete all records in the keys table - not pretty but it works!
                    var rows = from o in db.commentKeys
                               select o;
                    foreach (var row in rows)
                    {
                        db.commentKeys.Remove(row);
                    }

                    var k = new commentKey()
                    {
                        Next_Id = c.Comment_Id + 1
                    };

                    db.commentKeys.Add(k);
                    db.commentTables.Add(c);
                    db.SaveChanges();

                }
                catch (Exception)
                {
                    return View(model);
                }


                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}