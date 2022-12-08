using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelProject.Models
{
    public class ReportedModel
    {
        public Guid ID { get; set; }
        public Guid ReportedID { get; set; }  // Article or Comment
        public Guid UserID { get; set; }    // 進行檢舉的user
        public string ReportType { get; set; }
        public DateTime ReportDate { get; set; }
        public string Reason { get; set; }
        public string ReasonContent { get; set; }
    }
}