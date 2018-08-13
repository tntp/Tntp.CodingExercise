using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Simple_Twitter.Entities;
using Simple_Twitter.Utilities;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Simple_Twitter.Models
{
    public class SimpleTwitterModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<SimpleTwitter> tweetCollection;

        public SimpleTwitterModel(IDBConfig dbConfig)
        {
            mongoClient = new MongoClient(
                dbConfig.GetDBHost());
            IMongoDatabase db = mongoClient.GetDatabase(
                dbConfig.GetDBName());
            tweetCollection = db.GetCollection<SimpleTwitter>("SimpleTwitter");
        }

        public List<SimpleTwitter> findAll()
        {
            return tweetCollection.AsQueryable<SimpleTwitter>().ToList();
        }

        public IEnumerable<SimpleTwitter> find(string handle)
        {
            List<SimpleTwitter> results = tweetCollection.
                        AsQueryable<SimpleTwitter>().Where(
                            x => x.Handle == handle).ToList<SimpleTwitter>();

            return results.OrderByDescending(x => DateTime.Parse(x.Time)).ToList();
        }

        public void create(SimpleTwitter simpleTwitter)
        {
            tweetCollection.InsertOne(simpleTwitter);
        }
    }
}