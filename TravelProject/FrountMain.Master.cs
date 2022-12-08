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
        private NotifyManager _nfMgr = new NotifyManager();
        private static UserAccountModel _user;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!new AccountManager().IsLogin())
            {
                Response.Redirect("Login.aspx", true);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            _user = HttpContext.Current.Session["UserAccount"] as UserAccountModel;
            this.userPage.HRef = "UserPage.aspx?User=" + _user.Account;
            this.collectionPage.HRef = "Collection.aspx?User=" + _user.Account;
            this.MainAccountltl.Text = "您好，" + _user.Account;
            this.liUserInfoEdit.HRef = "UserInfoEdit.aspx?User=" + _user.Account;
            InitNotifyBox();

            UserAccountModel user = new AccountManager().GetAccountModel(_user.Account);
            if (user.UserLevel == UserLevelEnumModel.Super || user.UserLevel == UserLevelEnumModel.Admin)
                this.ltlBackAdmin.Visible = true;
            else
                this.ltlBackAdmin.Visible = false;

            if (!this.IsPostBack)
            {
                string keyword = this.Request.QueryString["keyword"];
                if (!string.IsNullOrWhiteSpace(keyword))
                    this.txtkeyword.Text = keyword;
            }


        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            new AccountManager().Logout();
            Response.Redirect("index.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = this.txtkeyword.Text.Trim();

            if (string.IsNullOrWhiteSpace(this.txtkeyword.Text))
                this.Response.Redirect("index.aspx");
            else
                Response.Redirect("index.aspx?keyword=" + keyword);
        }

        private void InitNotifyBox()
        {
            Literal ltlNoNotice = FindControl("ltlNoNotify") as Literal;
            List<NoticeModel> notifyList = _nfMgr.GetNotifyList(_user.UserID);
            if (notifyList.Count > 0)
            {
                this.rptNotify.DataSource = notifyList;
                int notifyCount = 0;
                foreach (NoticeModel notify in notifyList)
                {
                    if (!notify.IsViewed)
                        notifyCount++;
                }
                if (notifyList.Count > 7)
                {
                    this.rptNotify.DataSource = notifyList.GetRange(0, 7);
                    ltlNoNotice.Text = "<li><hr class='dropdown-divider'></li><li><a class='dropdown-item' href=\"MessageList.aspx\">查看更多...</a></li>";
                }
                this.rptNotify.DataBind();
                this.hfNoticeCount.Value = notifyCount.ToString();
            }
            else
            {
                ltlNoNotice.Text = "<li><p class='dropdown-item'>尚無通知</p></li>";
            }


        }
    }
}