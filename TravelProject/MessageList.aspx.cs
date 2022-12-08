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
    public partial class MessageList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserAccountModel user = HttpContext.Current.Session["UserAccount"] as UserAccountModel;
            List<NoticeModel> noticeList = new NotifyManager().GetNotifyList(user.UserID);
            if (noticeList.Count == 0)
                Response.Redirect("index.aspx");

            this.rptNotify.DataSource = noticeList;
            this.rptNotify.DataBind();
        }
    }
}