using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tntp.WebApplication.Models
{
    /// <summary>
    /// The Comment class is an entity class representing rows in the Comments database table.
    /// </summary>
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }
        public string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationTimestamp { get; set; }
    }
}