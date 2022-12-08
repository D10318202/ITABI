using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TravelProject.Managers;
using TravelProject.Models;

namespace TravelProject
{
    public partial class FrountMain : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!new AccountManager().IsLogin())
                Response.Redirect("Login.aspx", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            UserAccount user = HttpContext.Current.Session["UserAccount"] as UserAccount;;
            this.userPage.HRef = "UserPage.aspx?User=" + user.Account;
            this.collectionPage.HRef = "Collection.aspx?Collector=" + user.UserID;

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            new AccountManager().Logout();
            Response.Redirect("login.aspx");
        }
    }
}