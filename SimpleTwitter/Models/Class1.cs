using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleTwitter.Models
{
    public class FeedbackForm
    {
         
            public string Name { get; set; }
            public string text { get; set; }
            //public string Phone { get; set; }
           // public string Company { get; set; }
           // public string AdditionalInformation { get; set; }
            public HttpPostedFileBase ProjectInformation { get; set; }
        
    }
}