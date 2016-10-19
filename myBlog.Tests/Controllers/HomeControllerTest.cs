using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myBlog;
using myBlog.Controllers;
using static myBlog.Models.BlogModel;
using myBlog.Models;


namespace myBlog.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest : Controller
    {
        [TestMethod]
        public void TestIndex()                     //I have never done a unit test before I promise I will get better at it!
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Index", result.ViewName);

        }
        [TestMethod]
        public void TestPostComment()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            CommentMD md = new CommentMD();
            var result = controller.PostComment(md) as ViewResult;

            // Assert
            Assert.AreEqual("Index", result.ViewName);
        }
     
        [TestMethod]
        public void TestPostCommentValidation()
        {
           
            HomeController controller = new HomeController();
            controller.ModelState.Clear();

           var comment = new CommentMD
            {
                CommentID = 1,
                CommentPostDate = DateTime.Now,
                CommentText = "Hello!",
                CommentUserName = "Nicole"
            };

            ActionResult result = controller.PostComment(comment);
            Assert.IsTrue(controller.ModelState.IsValid, "Not valid");
        }
        [TestMethod]
        public void TestAddComment()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            var result = controller.AddComment() as ViewResult;

            // Assert
            Assert.AreEqual("AddComment", result.ViewName);
        }

    }
}
