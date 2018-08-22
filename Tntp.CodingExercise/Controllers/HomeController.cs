using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using Tntp.CodingExercise.Models;


namespace Tntp.CodingExercise.Controllers
{
    public class HomeController : Controller
    {
        static DeveloperDBContext dbContext;
        //static Developer currentDeveloper;

        public ActionResult Index()
        {
            ReadMongoDB();

            //return String.Format(
            //    "Documents Read: {0}<br/>Developers Inserted: {1}<br/>Contribs Inserted: {2}<br/>Awards Inserted: {3}",
            //    dbContext.DocumentsRead,
            //    dbContext.DevelopersInserted,
            //    dbContext.ContribsInserted,
            //    dbContext.AwardsInserted
            //);

            ViewBag.DocumentsRead = dbContext.DocumentsRead;

            ViewBag.Developers = dbContext.Developers.ToList();
            ViewBag.DevelopersInserted = dbContext.DevelopersInserted;

            ViewBag.Contribs = dbContext.Contribs.ToList();
            ViewBag.ContribsInserted = dbContext.ContribsInserted;

            ViewBag.Awards = dbContext.Awards.ToList();
            ViewBag.AwardsInserted = dbContext.AwardsInserted;

            return View();
        }

        public void ReadMongoDB()
        {
            // TODO: Store connection string, database & collection names in Web.config
            var connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("test");
            var collection = database.GetCollection("bios").FindAll();
            dbContext = new DeveloperDBContext();

            foreach (BsonDocument doc in collection)
            {
                InsertDeveloper(doc);
                InsertContrib(doc);
                InsertAward(doc);
                dbContext.DocumentsRead++;
            }

            dbContext.SaveChanges();
        }

        private static void InsertAward(BsonDocument doc)
        {
            if (doc.Contains("awards"))
            {
                foreach (BsonDocument award in doc["awards"].AsBsonArray)
                {
                    var newAward = new Award()
                    {
                        ID = Guid.NewGuid(),
                        Developer_ID = doc["_id"].ToString(),
                        //Developer = currentDeveloper,
                        Name = award["award"].AsString,
                        Year = Convert.ToInt16(award["year"]),
                        By = award["by"].AsString
                    };

                    if (!dbContext.Awards.Any(
                        a => a.Developer_ID == newAward.Developer_ID && a.Name == newAward.Name
                        &&   a.Year == newAward.Year && a.By == newAward.By
                    )) 
                    {
                        dbContext.Awards.Add(newAward);
                        dbContext.AwardsInserted++;
                    }
                        
                }
            }
        }

        private static void InsertContrib(BsonDocument doc)
        {
            if (doc.Contains("contribs"))
                foreach (BsonValue contrib in doc["contribs"].AsBsonArray)
                {
                    var newContrib = new Contrib()
                    {
                        ID = Guid.NewGuid(),
                        Developer_ID = doc["_id"].ToString(),
                        //Developer = currentDeveloper,
                        Name = contrib.AsString
                    };

                    if (!dbContext.Contribs.Any(c => c.Developer_ID == newContrib.Developer_ID && c.Name == newContrib.Name))
                    {
                        dbContext.Contribs.Add(newContrib);
                        dbContext.ContribsInserted++;
                    }
                }
        }

        private static void InsertDeveloper(BsonDocument doc)
        {
            var newDeveloper = new Developer()
            {
                ID = doc["_id"].ToString(),
                FirstName = doc["name"]["first"].AsString,
                LastName = doc["name"]["last"].AsString,
                Title = doc.GetValue("title", "").AsString
            };

            if (doc.Contains("birth"))
                newDeveloper.BirthDate = Convert.ToDateTime(doc["birth"]);
            if (doc.Contains("death"))
                newDeveloper.DeathDate = Convert.ToDateTime(doc["death"]);

            if (!dbContext.Developers.Any(d => d.ID == newDeveloper.ID))
            { 
                dbContext.Developers.Add(newDeveloper);
                dbContext.DevelopersInserted++;
            }

            //currentDeveloper = newDeveloper;
        }
    }
}