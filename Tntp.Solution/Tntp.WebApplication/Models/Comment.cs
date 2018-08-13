using System;

namespace Tntp.WebApplication.Models
{
    /// <summary>
    /// The Comment class is an entity class representing rows in the Comments database table.
    /// </summary>
    public class Comment
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime CreationTimestamp { get; set; }
    }
}