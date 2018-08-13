using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTwitter;
using SimpleTwitter.Models;

namespace SimpleTwitterTest
{

    // Sorry for the limited number of tests but I ran out of time!
    // Note that I have to look for exceptions as the whole process fails on this first one
    // In this case I force an assert that is false.
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_NoParams_IsNotNull()
        {
            try
            {
                //Arrange
                SimpleTwitter.Controllers.HomeController hc = new SimpleTwitter.Controllers.HomeController();

                //Act
                var result = hc.Index();

                //Assert
                Assert.IsNotNull(result);

            }
            catch (Exception)
            {
                Assert.Equals("1", "2");
            }

        }

        [TestMethod]
        public void Index_PassingParams_IsNotNull()
        {
            try
            {
                //Arrange
                SimpleTwitter.Controllers.HomeController hc = new SimpleTwitter.Controllers.HomeController();
                var model = new CommentViewMaster();
                model.Comment_Name = "James";
                model.Comment_Comment = "This is a really fun day!";

                //Act
                var result = hc.Index(model);

                //Assert
                Assert.IsNotNull(result);
            }
            catch (Exception)
            {
                Assert.Equals("1", "2");
            }

        }

    }
}

