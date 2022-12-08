using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelProject.Models
{
    public class ArticleModel
    {
        public Guid ArticleID { get; set; }
        public Guid UserID { get; set; }
        public string District { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string PlaceID { get; set; }
        public string ArticleContent { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public ArticleViewLimitEnum ViewLimit { get; set; }
        public CommentLimitEnum CommentLimit { get; set; }

    }
    public enum ArticleViewLimitEnum
    {
        Public = 1,
        FollowersLimit = 2,
        Private = 3
    }
    public enum CommentLimitEnum
    {
        Public = 1,
        FollowersLimit = 2,
        Private = 3
    }
}