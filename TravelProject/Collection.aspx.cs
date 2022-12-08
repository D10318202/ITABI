using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TravelProject.Models;
using TravelProject.Managers;

namespace TravelProject
{
    public partial class Collection : System.Web.UI.Page
    {
        private UserActiveManager _uamgr = new UserActiveManager();
        private static UserAccountModel _account;
        protected void Page_Load(object sender, EventArgs e)
        {
            _account = HttpContext.Current.Session["UserAccount"] as UserAccountModel;
            if (_account != null)
            {
                //List<ArticleModel> articleList = _uamgr.GetCollectList(_account.UserID);
                List<ImageModel> imgList = _uamgr.GetCollectCoverList(_account.UserID);
                //if (articleList.Count > 0)
                if (imgList.Count > 0)
                {
                    //this.rptCollection.DataSource = articleList;
                    //this.rptCollection.DataBind();
                    this.rptCollectionCover.DataSource = imgList;
                    this.rptCollectionCover.DataBind();
                }
            }
            else
                Response.Redirect("login.aspx");
        }
    }
}