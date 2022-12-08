using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelProject.Models;

namespace TravelProject.BackAdmin
{
    public abstract class AdminPageBace : System.Web.UI.Page 
    {
        public virtual UserLevelEnumModel[] PageUserLevel { get; set; } = { UserLevelEnumModel.Admin, UserLevelEnumModel.Super };
        public UserLevelEnumModel[] GetUserLevel()
        {
            return this.PageUserLevel;
        }
    }
}