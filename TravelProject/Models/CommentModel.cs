using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelProject.Models
{
    public class CommentModel
    {
        public Guid ArticleID { get; set; }
        public Guid UserID { get; set; }
        public Guid CommentID { get; set; }
        public string Account { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreateTime { get; set; }
    }
}