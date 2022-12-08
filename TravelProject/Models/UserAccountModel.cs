using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelProject.Models
{
    public class UserAccountModel
    {
        public Guid UserID { get; set; }
        public string Account { get; set; }
        public string PWD { get; set; }
        public string PWDkey { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool AccountStates { get; set; }
        public DateTime? DeactivateDate { get; set; }
        public string ProfileContent { get; set; }
        public string ProfileImg { get; set; }

        public UserLevelEnumModel UserLevel { get; set; }

    }
}