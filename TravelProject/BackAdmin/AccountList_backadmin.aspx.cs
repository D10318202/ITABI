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
    public partial class AccountList_backadmin : AdminPageBace //增加3.31
    {
        private AccountManager _mgr = new AccountManager();
        private AccountManager _accMgr = new AccountManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string keyword = this.Request.QueryString["keyword"];
                this.txtkeyword.Text = keyword;

                var list = this._mgr.GetAccountList(keyword);
                if (list.Count > 0)
                {
                    this.gvList.DataSource = list;
                    this.gvList.DataBind();
                    this.gvList.Visible = true;
                }
            }
        }
        //    protected void btnDelete_Click(object sender, EventArgs e)
        //    {
        //        List<Guid> idList = new List<Guid>();
        //        foreach (GridViewRow gRow in this.gvList.Rows)
        //        {
        //            CheckBox chkdel = gRow.FindControl("chkdel") as CheckBox;
        //            HiddenField hfID = gRow.FindControl("hfID") as HiddenField;

        //            if (chkdel != null && hfID != null)
        //            {
        //                if (chkdel.Checked)
        //                {
        //                    Guid UserID;
        //                    if (Guid.TryParse(hfID.Value, out UserID))
        //                        idList.Add(UserID);
        //                }
        //            }
        //        }
        //        if (idList.Count > 0)
        //        {
        //            this._mgr.DeleteAccounts(idList);
        //            this.Response.Redirect(this.Request.RawUrl);
        //        }
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = this.txtkeyword.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
                Response.Redirect("AccountList_backadmin.aspx");
            else
                Response.Redirect("AccountList_backadmin.aspx?keyword=" + keyword);
        }
    }
}