using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Simple_Twitter.Utilities
{
    public class MongoDBConfiguration : IDBConfig
    {
        public string GetDBHost()
        {
            return ConfigurationManager.AppSettings["mongoDBHost"];
        }

        public string GetDBName()
        {
            return ConfigurationManager.AppSettings["mongoDBName"];
        }
    }
}