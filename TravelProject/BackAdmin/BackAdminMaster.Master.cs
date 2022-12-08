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
    public partial class BackAdminMaster : System.Web.UI.MasterPage
    {
        private string _loginPage = "~/Login.aspx";
        private string _indexPage = "~/Index.aspx";
        private AccountManager _AccMgr = new AccountManager();
        private static UserAccountModel _account;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!new AccountManager().IsLogin())    
                Response.Redirect(_loginPage, true);

            if (this.Page is AdminPageBace)
            {
                AdminPageBace adminPage = (AdminPageBace)this.Page;
                UserLevelEnumModel[] pageUserLevel = adminPage.GetUserLevel();

                UserAccountModel model = this._AccMgr.GetCurrentUser();
                if (!pageUserLevel.Contains(model.UserLevel))
                    Response.Redirect(_indexPage, true);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserAccountModel user = HttpContext.Current.Session["UserAccount"] as UserAccountModel;
                this.ltlUserAcc.Text = " 您好，" + user.Account;
            }
            else
                this.ltlUserAcc.Text = "尚未登入";
            //if (HttpContext.Current.Request.IsAuthenticated)
            //{
            //    string owneraccount = HttpContext.Current.User.Identity.Name;

            //    var identity = HttpContext.Current.User.Identity as FormsIdentity;
            //    var Ticket = identity.Ticket;
            //    this.ltlUserAcc.Text = " 您好，" + owneraccount;
            //}
        }
        protected void btnlogout_Click(object sender, EventArgs e)
        {
            //new AccountManager().Logout();
            Response.Redirect("../login.aspx");
        }
    }
}