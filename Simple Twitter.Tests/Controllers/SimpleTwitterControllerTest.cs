using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple_Twitter.Utilities;
using Simple_Twitter.Controllers;
using Moq;

namespace Simple_Twitter.Tests.Controllers
{
    [TestClass]
    public class SimpleTwitterControllerTest
    {
        [TestMethod]
        public void Index()
        {
            Mock<IDBConfig> dbConfig = new Mock<IDBConfig>();
            dbConfig.Setup(x => x.GetDBHost()).Returns("mongodb://127.0.0.1:27017");
            dbConfig.Setup(x => x.GetDBName()).Returns("SimpleTwitterDB");
            SimpleTwitterController controller = new SimpleTwitterController(dbConfig.Object);

            // Test null strings 
            string handle = null;
            string tweet = null;
            ViewResult result = controller.Index(handle, tweet) as ViewResult;
            Assert.IsNull(result.ViewBag.Handle);
            Assert.IsNull(result.ViewBag.Tweet);

            //Test empty strings
            handle = "";
            tweet = "";
            result = controller.Index(handle, tweet) as ViewResult;
            Assert.IsNull(result.ViewBag.Handle);
            Assert.IsNull(result.ViewBag.Tweet);

            //Test large tweet
            handle = "@user";
            tweet = "asdkfja;skldfjalskdfjas;lkdjfsalkdfjslk;dfjaslk;dfjsakd;lfjsal;kdfjslkdfjskldfjsdlkjasldkjdsalkfj" +
                "aslkdfjsal;kdfjasl;kdjfl;aksdjfl;aksdjfaksdfklasjdfklasjdflkasjdfl;kasjdflkasjdflkasjdflkdfjasld;kfjasdlk" +
                "asdlfjsadlkfjasdl;kfjasdlkfjsadlkfjasdlkfjasldkfjaslkd;fjasl;kdfjaskdlfjasldkfjsldkfjslkdfjaslkdfjasldk";
            result = controller.Index(handle, tweet) as ViewResult;
            Assert.IsNull(result.ViewBag.Handle);
            Assert.IsNull(result.ViewBag.Tweet);

            //Test Valid Tweet
            handle = "@user";
            tweet = "hello... I'm tweeting";
            result = controller.Index(handle, tweet) as ViewResult;
            Assert.IsNotNull(result.ViewBag.Handle);
            Assert.IsNotNull(result.ViewBag.Tweet);
        }

        [TestMethod]
        public void ViewTweets()
        {
            Mock<IDBConfig> dbConfig = new Mock<IDBConfig>();
            dbConfig.Setup(x => x.GetDBHost()).Returns("mongodb://127.0.0.1:27017");
            dbConfig.Setup(x => x.GetDBName()).Returns("SimpleTwitterDB");
            SimpleTwitterController controller = new SimpleTwitterController(dbConfig.Object);

            // Test null string
            string handle = null;
            ViewResult result = controller.ViewTweets(handle) as ViewResult;
            Assert.IsNull(result.ViewBag.Handle);

            //Test empty strings
            handle = "";
            result = controller.ViewTweets(handle) as ViewResult;
            Assert.IsNull(result.ViewBag.Handle);

            //Test Valid Tweet
            handle = "@user";
            string tweet = "hello... I'm tweeting";
            controller.Index(handle, tweet);
            result = controller.ViewTweets(handle) as ViewResult;

            bool bFound = false;
            foreach (var str in result.ViewBag.Tweets)
            {
                if (str.Tweet == tweet)
                {
                    bFound = true;
                    break;
                }
            }
            Assert.IsTrue(bFound);
        }
    }
}
