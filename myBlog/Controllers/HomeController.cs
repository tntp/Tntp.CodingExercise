using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myBlog.Models;
using System.Data.Entity;
using static myBlog.Models.BlogModel;

namespace myBlog.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            BlogModel bm = new BlogModel();

            using (BlogEntities db = new BlogEntities())
            {
                bm.Comments = db.MicroBlogs.Select(i => new CommentMD
                {
                    CommentID = i.CommentID,
                    CommentText = i.CommentText,
                    CommentPostDate = i.CommentPostDate,
                    CommentUserName = i.CommentUserName

                }).Distinct().OrderByDescending(i => i.CommentPostDate).ToList();
            }

            return View(bm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostComment(CommentMD c)
        {
            MicroBlog mb = new MicroBlog();
            mb.CommentPostDate = DateTime.Now;
            mb.CommentText = c.CommentText;
            mb.CommentUserName = c.CommentUserName;

            if (ModelState.IsValid)
            {
                using (BlogEntities db = new BlogEntities())
                {
                    try
                    {
                        db.MicroBlogs.Add(mb);
                        db.SaveChanges();
                        return RedirectToAction("Index");

                    }catch(Exception e)
                    {

                    }
                    
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult AddComment()
        {
            return PartialView();
        }
       
    }
}