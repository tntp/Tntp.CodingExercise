using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tntp.WebApplication.Controllers;
using System;
using System.Linq;
using Tntp.WebApplication.Models;
using System.Web.Http.Results;
using System.Net;

namespace Tntp.WebApplication.Tests
{
    [TestClass]
    public class CommentsControllerTests
    {
        private readonly IRepository _repository = new MockRepository();
        private readonly CommentsController _controller;

        public CommentsControllerTests()
        {
            _repository.Comments.Add(new Comment { Username = "Eddard", Content = "Stark", CreationTimestamp = DateTime.Now.AddDays(-1) });
            _repository.Comments.Add(new Comment { Username = "Jon", Content = "Snow", CreationTimestamp = DateTime.Now });
            _controller = new CommentsController(_repository);
        }

        [TestMethod]
        public void GetCommentsTest()
        {
            var results = _controller.GetComments();
            Assert.AreEqual("Jon", results.ElementAt(0).Username);
            Assert.AreEqual("Snow", results.ElementAt(0).Content);
            Assert.AreEqual("Eddard", results.ElementAt(1).Username);
            Assert.AreEqual("Stark", results.ElementAt(1).Content);
        }

        [TestMethod]
        public void AddCommentTest_NullUsername()
        {
            var result = _controller.AddComment(new Comment { Content = "Seaworth" }) as BadRequestErrorMessageResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("A username is required.", result.Message);
        }

        [TestMethod]
        public void AddCommentTest_EmptyUsername()
        {
            var result = _controller.AddComment(new Comment { Username = "", Content = "Seaworth" }) as BadRequestErrorMessageResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("A username is required.", result.Message);
        }

        [TestMethod]
        public void AddCommentTest_TooLongUsername()
        {
            var result = _controller.AddComment(new Comment { Username = "Stannnnnnnnnnnis", Content = "Baratheon" }) as BadRequestErrorMessageResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("A username must not exceed 15 characters.", result.Message);
        }

        [TestMethod]
        public void AddCommentTest_NullContent()
        {
            var result = _controller.AddComment(new Comment { Username = "Davos" }) as BadRequestErrorMessageResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("A comment is required.", result.Message);
        }

        [TestMethod]
        public void AddCommentTest_EmptyContent()
        {
            var result = _controller.AddComment(new Comment { Username = "Davos", Content = "" }) as BadRequestErrorMessageResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("A comment is required.", result.Message);
        }

        [TestMethod]
        public void AddCommentTest_TooLongContent()
        {
            var result = _controller.AddComment(new Comment
            {
                Username = "Hodor",
                Content = "Hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor hodor"
            }) as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("A comment must not exceed 140 characters.", result.Message);
        }

        [TestMethod]
        public void AddCommentTest_Successful()
        {
            var result = _controller.AddComment(new Comment { Username = "Davos", Content = "Seaworth" }) as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}