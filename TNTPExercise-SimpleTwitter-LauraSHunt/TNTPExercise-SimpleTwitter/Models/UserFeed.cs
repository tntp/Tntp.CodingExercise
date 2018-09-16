using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TNTPExercise_SimpleTwitter.Models
{
    public class UserFeed
    {
        [Required]
        [Display (Name = "Name")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "Name must be less than 25 characters")]
        public string Name { get; set; }

        [Required]
        [Display (Name = "Comment")]
        [StringLength(140, MinimumLength = 1, ErrorMessage = "Comment cannot be more than 140 characters")]
        public string Comment { get; set; }

        public IEnumerable<UserComment> UserComments { get; set; }

    }
}