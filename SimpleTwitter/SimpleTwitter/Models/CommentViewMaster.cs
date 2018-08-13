using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleTwitter.Models
{
    public class CommentViewMaster
    {
        // Not required for the user as the system sets it
        public int Comment_Id { get; set; }

        [Required(ErrorMessage = "Name must be between 1 and 128 characters")]
        public string Comment_Name { get; set; }

        [Required(ErrorMessage = "Comment must be between 1 and 140 characters")]
        public string Comment_Comment { get; set; }

        // Not required for the user as the system sets it
        public System.DateTime Comment_Date { get; set; }


        public List<CommentViewModel> CVMList { get; set; }

    }
}