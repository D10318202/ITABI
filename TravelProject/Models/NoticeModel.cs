using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelProject.Models
{
    public class NoticeModel
    {
        public Guid UserID { get; set; }
        public string ActAccount { get; set; }
        public NotifyType NfType { get; set; }
        public string NotifyContent { get; set; }
        public string Url { get; set; }
        public bool IsViewed { get; set; }
        public string CreateTime { get; set; }
        public DateTime? ExpireTime { get; set; }
        public string ProfileImg { get; set; }
    }
    public enum NotifyType
    {
        Follow = 0,
        Like = 1,
        Comment = 2,
        Tag = 3
    }
}