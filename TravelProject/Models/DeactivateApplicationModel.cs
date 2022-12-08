using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelProject.Models
{
    public class DeactivateApplicationModel
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Reason { get; set; }
        public string DeactContent { get; set; }
    }
}