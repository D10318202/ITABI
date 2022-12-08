using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TravelProject.Managers;
using TravelProject.Models;

namespace TravelProject.BackAdmin
{
    public partial class DeactivateList_backadmin : AdminPageBace
    {
        public static AccountManager _mgr = new AccountManager();
        public override UserLevelEnumModel[] PageUserLevel { get; set; } = { UserLevelEnumModel.Super };
        protected void Page_Load(object sender, EventArgs e)
        {
            List<DeactivateApplicationModel> list = _mgr.GetDeactivateList();
            if (list.Count == 0)
            {
                this.plcEmpty.Visible = true;
                this.gvList.Visible = false;
            }
            else
            {
                this.plcEmpty.Visible = false;
                this.gvList.Visible = true;

                this.gvList.DataSource = list;
                this.gvList.DataBind();
            }
        }
    }
}