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
    public partial class index : System.Web.UI.Page
    {
        private static ArticleManager _mgr = new ArticleManager();
        protected void Page_Load(object sender, EventArgs e)
        {
           




            string keyword = this.Request.QueryString["keyword"];
            if (keyword != null)
            {
                List<ImageModel> resultList = _mgr.GetSearchArticleList(keyword);
                if (resultList != null)
                {
                    this.rptCoverImg.DataSource = resultList;
                    this.rptCoverImg.DataBind();
                }
                else
                {
                    this.rptCoverImg.Visible = false;
                }
            }
            else
            {
                List<ImageModel> resultList = _mgr.GetIndexArticleList();
                this.rptCoverImg.DataSource = resultList;
                this.rptCoverImg.DataBind();
            }
        }

        protected void rptCoverImg_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

    }
}