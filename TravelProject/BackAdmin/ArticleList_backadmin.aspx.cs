using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TravelProject.Managers;
using TravelProject.Models;

namespace TravelProject.BackAdmin
{
    public partial class ArticleList_backadmin : AdminPageBace  
    {
        
        private ArticleManager _mgr = new ArticleManager();
        private static UserAccountModel _nowUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string keyword = this.Request.QueryString["keyword"];
                this.txtkeyword.Text = keyword;

                List<ArticleModel> list = this._mgr.GetAdminArticleList(keyword);
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
        public void btnDelete_Click(object sender, EventArgs e)
        {
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = this.txtkeyword.Text.Trim();

            if (string.IsNullOrWhiteSpace(this.txtkeyword.Text))
                this.Response.Redirect("ArticleList_backadmin.aspx");
            else
                Response.Redirect("ArticleList_backadmin.aspx?keyword=" + keyword);
        }
    }
}