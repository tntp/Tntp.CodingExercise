using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TNTPExercise_SimpleTwitter;
using TNTPExercise_SimpleTwitter.Controllers;
using TNTPExercise_SimpleTwitter.Models;

namespace TNTPExercise_SimpleTwitter.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {

        //
        // Test Case 1:
        //
        // Given a user who has found the site
        // When that user wants to post a comment
        // Then that user should be able to supply their name, and a comment
        //
        [TestMethod]
        public void TestCase1()
        {
            // Arrange -
            // User supplies a valid name and a valid comment
            UserFeed userFeed = new UserFeed();
            userFeed.Name = "Valid Name";  
            userFeed.Comment = "Valid Comment";


            // Act -
            // Validate the user supplied values
            var context = new ValidationContext(userFeed, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(userFeed, context, result, true);

            // Assert -
            // The user supplied values are valid (and the comment will be posted)
            Assert.IsTrue(valid);
        }

        //
        // Test Case 2:
        //
        // Given a user who is posting a comment
        // When that user does not supply a name
        // Then they should not be able to post their comment
        //
        [TestMethod]
        public void TestCase2()
        {
            // Arrange -
            // User does not supply a valid name with valid comment
            UserFeed userFeed = new UserFeed();
            userFeed.Name = "";
            userFeed.Comment = "Valid Comment";

            // Act -
            // Validate the user supplied values
            var context = new ValidationContext(userFeed, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(userFeed, context, result, true);

            // Assert -
            // The user supplied values are not valid (and the comment will not be posted)
            Assert.IsFalse(valid);
        }

        //
        // Test Case 3:
        //
        // Given a user who is posting a comment
        // When that user attempts to exceed a limit of 140 characters in their comment
        // Then that user should not be able to post their comment
        //
        [TestMethod]
        public void TestCase3()
        {
            // Arrange -
            // User comment > 140 characters
            UserFeed userFeed = new UserFeed();
            userFeed.Name = "Valid Name";
            userFeed.Comment = 
                "cccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc" +
                "cccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc" +
                "cccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc";

            // Act -
            // Validate the user supplied values
            var context = new ValidationContext(userFeed, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(userFeed, context, result, true);

            // Assert -
            // The user supplied values are not valid (and the comment will not be posted)
            Assert.IsFalse(valid);
        }

        //
        // Test Case 4:
        //
        // Given a user
        // When that user is viewing comments
        // Then they should see all of the comments in the system, in reverse chronological order (newer comments first)
        //
        [TestMethod]
        public void TestCase4()
        {
            // Arrange -
            // Create Home controller
            HomeController controller = new HomeController();
            SimpleTwitterEntities db = new SimpleTwitterEntities();

            // Act -
            // Home controller returns UserFeed (Model) object
            // w/ all comments (if exist) in reverse chrono order
            ViewResult result = controller.Index() as ViewResult;

            // Assert -
            // The comments list returned equals the expected comments list as sorted
            var testComments = ((UserFeed)result.Model) != null ? ((UserFeed)result.Model).UserComments : null;
            var expectedComments = testComments.ToList().OrderByDescending(c => c.CreatedOn);
            CollectionAssert.AreEqual(expectedComments.ToList(), testComments.ToList());
        }

        //
        // Test Case 5:
        //
        // Given a user
        // When that user attempts to exceed a limit of 25 characters in their name
        // Then that user should not be able to post their comment
        //
        [TestMethod]
        public void TestCase5()
        {
            // Arrange
            UserFeed userFeed = new UserFeed();
            userFeed.Name = "nnnnnnnnnnnnnnnnnnnnnnnnnn";
            userFeed.Comment = "Valid Comment";

            // Act
            var context = new ValidationContext(userFeed, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(userFeed, context, result, true);

            // Assert 
            Assert.IsFalse(valid);
        }

        //
        // Test Case 6:
        //
        // Given a user
        // When that user attempts to send an empty comment
        // Then that user should not be able to post their comment
        //
        [TestMethod]
        public void TestCase6()
        {
            // Arrange
            UserFeed userFeed = new UserFeed();
            userFeed.Name = "Valid Name";
            userFeed.Comment = "";

            // Act
            var context = new ValidationContext(userFeed, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(userFeed, context, result, true);

            // Assert
            Assert.IsFalse(valid);
        }

        //
        // Test Case 7:
        //
        // Given a user
        // When that user attempts to send an empty name and an empty comment
        // Then that user should not be able to post their comment
        //
        [TestMethod]
        public void TestCase7()
        {
            // Arrange
            UserFeed userFeed = new UserFeed();
            userFeed.Name = "";
            userFeed.Comment = "";

            // Act
            var context = new ValidationContext(userFeed, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(userFeed, context, result, true);

            // Assert
            Assert.IsFalse(valid);
        }
    }
}
