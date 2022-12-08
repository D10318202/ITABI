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
    public partial class Rank : System.Web.UI.Page
    {
        private ArticleManager _mgr = new ArticleManager();
        private static ArticleModel _article;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<ImageModel> resultList = _mgr.GetRankArticleList();
            this.rptRank.DataSource = resultList;
            this.rptRank.DataBind();

        }
    }
}