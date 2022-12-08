using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelProject.Models
{
    public class ImageModel
    {
        public Guid PictureID { get; set; }
        public Guid ArticleID { get; set; }
        public string PicturePath { get; set; }
        public int PictureNumber { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public DateTime CreateTime { get; set; }

    }
}