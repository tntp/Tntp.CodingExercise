using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple_Twitter.Utilities;

namespace Simple_Twitter.Tests.Utilities
{
    [TestClass]
    public class ElapsedTimeStringTest
    {
        [TestMethod]
        public void GetElaspedTimeString()
        {
            DateTime fortyFiveMinsAgo = DateTime.Now.AddMinutes(-45);
            string str = ElaspedTimeString.GetElaspedTimeString(fortyFiveMinsAgo);
            Assert.AreEqual(str, "45 minutes ago");

            DateTime oneMinuteAgo = DateTime.Now.AddMinutes(-1);
            str = ElaspedTimeString.GetElaspedTimeString(oneMinuteAgo);
            Assert.AreEqual(str, "1 minute ago");

            DateTime tenSecondsAgo = DateTime.Now.AddSeconds(-10);
            str = ElaspedTimeString.GetElaspedTimeString(tenSecondsAgo);
            Assert.AreEqual(str, "10 seconds ago");

            DateTime oneSecondAgo = DateTime.Now.AddSeconds(-1);
            str = ElaspedTimeString.GetElaspedTimeString(oneSecondAgo);
            Assert.AreEqual(str, "1 second ago");

            DateTime fiveHoursAgo = DateTime.Now.AddHours(-5);
            str = ElaspedTimeString.GetElaspedTimeString(fiveHoursAgo);
            Assert.AreEqual(str, "5 hours ago");

            DateTime oneHourAgo = DateTime.Now.AddHours(-1);
            str = ElaspedTimeString.GetElaspedTimeString(oneHourAgo);
            Assert.AreEqual(str, "1 hour ago");

            DateTime fiveDaysAgo = DateTime.Now.AddDays(-5);
            str = ElaspedTimeString.GetElaspedTimeString(fiveDaysAgo);
            Assert.AreEqual(str, "5 days ago");

            DateTime oneDayAgo = DateTime.Now.AddDays(-1);
            str = ElaspedTimeString.GetElaspedTimeString(oneDayAgo);
            Assert.AreEqual(str, "1 day ago");

            DateTime twoYearsAgo = DateTime.Now.AddDays(-730);
            str = ElaspedTimeString.GetElaspedTimeString(twoYearsAgo);
            Assert.AreEqual(str, "2 years ago");

            DateTime oneYearAgo = DateTime.Now.AddDays(-365);
            str = ElaspedTimeString.GetElaspedTimeString(oneYearAgo);
            Assert.AreEqual(str, "1 year ago");

        }
    }
}
