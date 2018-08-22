using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace myBlog.Models
{
    [MetadataType(typeof(CommentMD))]
    public class BlogModel
    {
        public List<CommentMD> Comments { get; set; }

        public class CommentMD
        {
            public int CommentID { get; set; }
            [Required(ErrorMessage = "*Name is required")]
            public string CommentUserName { get; set; }
            [Required(ErrorMessage = "*Comment is required")]
            [StringLength(140, ErrorMessage = "*Post must be less than 140 characters")]
            public string CommentText { get; set; }
            public System.DateTime CommentPostDate { get; set; }
        }
    }

 
}