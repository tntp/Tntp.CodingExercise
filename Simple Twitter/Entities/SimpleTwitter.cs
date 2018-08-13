using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Simple_Twitter.Entities
{
    public class SimpleTwitter
    {
        public SimpleTwitter(string handle, string tweet, string date)
        {
            Handle = handle;
            Tweet = tweet;
            Time = date;
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [Required]
        [BsonElement("handle")]
        public string Handle { get; set; }

        [Required]
        [StringLength(140)]
        [BsonElement("tweet")]
        public string Tweet { get; set; }

        [BsonElement("time")]
        public string Time { get; set; }

    }
}