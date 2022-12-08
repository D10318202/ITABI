using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TravelProject.Models;
using TravelProject.Managers;
using System.Web.UI.HtmlControls;

namespace TravelProject
{
    public partial class MyPage : System.Web.UI.Page
    {
        private ArticleManager _mgr = new ArticleManager();
        private AccountManager _AccMgr = new AccountManager();
        private UserActiveManager _uaMgr = new UserActiveManager();
        private MemberPageBase _mpB = new MemberPageBase();
        private static UserAccountModel _nowUser;
        private static UserAccountModel _pageOwner;
        protected void Page_Load(object sender, EventArgs e)
        {
            string ownerAccount = this.Request.QueryString["User"];
            if (!string.IsNullOrWhiteSpace(ownerAccount))
            {
                _pageOwner = _AccMgr.GetAccountModel(ownerAccount);
                if (_pageOwner != null)
                {
                    _nowUser = HttpContext.Current.Session["UserAccount"] as UserAccountModel;
                    this.ltlUserAcc.Text = _pageOwner.Account;
                    this.PageOwnerimg.Src = _pageOwner.ProfileImg;
                    this.ltlNickName.Text = _pageOwner.NickName;
                    this.ltlProfileContent.Text = _pageOwner.ProfileContent;
                    this.hfUserAccount.Value = _pageOwner.Account;
                    if (!IsPostBack)
                    {
                        InitArticle(_pageOwner.UserID);
                        InitFollower(_pageOwner.UserID);
                        if (_pageOwner.UserID == _nowUser.UserID)
                            InitMyPage();
                        else
                            InitGuestMode(_nowUser.UserID, _pageOwner.UserID);
                    }
                }
                else
                    Response.Redirect("index.aspx");
            }
            else
                Response.Redirect("index.aspx");

        }

        protected void btnFollow_Click(object sender, EventArgs e)
        {
            if (!_uaMgr.isFollowed(_nowUser.UserID, _pageOwner.UserID))
                _uaMgr.PressFollow(_nowUser.UserID, _pageOwner.UserID);
            else
                _uaMgr.UnFollow(_nowUser.UserID, _pageOwner.UserID);

            InitGuestMode(_nowUser.UserID, _pageOwner.UserID);
            InitFollower(_pageOwner.UserID);
        }
        private void InitFollower(Guid UserID)
        {
            List<UserAccountModel> followingList = _uaMgr.GetFollowingList(UserID);
            this.lkbFollowingCount.Text = followingList.Count.ToString();
            this.rptFollowing.DataSource = followingList;
            this.rptFollowing.DataBind();
            foreach (RepeaterItem item in this.rptFollowing.Items)
            {
                GetBtnList(item);
            }

            List<UserAccountModel> fansList = _uaMgr.GetFansList(UserID);
            this.lkbFansCount.Text = fansList.Count.ToString();
            this.rptFans.DataSource = fansList;
            this.rptFans.DataBind();
            foreach (RepeaterItem item in this.rptFans.Items)
            {
                GetBtnList(item);
            }
        }

        private void GetBtnList(RepeaterItem item)
        {
            HtmlInputHidden hfAcc = item.FindControl("hfAcc") as HtmlInputHidden;
            HtmlInputButton btnFollow = item.FindControl("btnListFollow") as HtmlInputButton;
            HtmlInputButton btnUnFollow = item.FindControl("btnListUnFollow") as HtmlInputButton;
            if (hfAcc != null)
            {
                UserAccountModel listUser = _AccMgr.GetAccountModel(hfAcc.Value);
                if (listUser != null)
                {
                    if (_nowUser.UserID == listUser.UserID)
                    {
                        btnFollow.Visible = false;
                        btnUnFollow.Visible = false;
                    }
                    else if (_uaMgr.isFollowed(_nowUser.UserID, listUser.UserID))
                    {
                        btnFollow.Visible = false;
                        btnUnFollow.Visible = true;
                        //btnFollow.Style["disabled"] = "disable";
                        //btnUnFollow.Style["disabled"] = "";
                    }
                    else
                    {
                        btnFollow.Visible = true;
                        btnUnFollow.Visible = false;
                        //btnFollow.Style["disabled"] = "";
                        //btnUnFollow.Style["disabled"] ="disable";
                    }
                }
            }
        }

        private void InitArticle(Guid UserID)
        {
            List<ArticleModel> articleList = _mgr.GetArticleList(UserID);
            List<ImageModel> imageList = new List<ImageModel>();
            //List<ArticleModel> canViewArticleList = new List<ArticleModel>();
            foreach (ArticleModel article in articleList)
            {
                ArticleViewLimitEnum limitEnum = _mpB.GetFollowerViewLimit(article.ArticleID);

                if (article.ViewLimit == ArticleViewLimitEnum.Public || limitEnum == article.ViewLimit || _nowUser.UserID == article.UserID)
                {
                    //canViewArticleList.Add(article);
                    ImageModel img = new ImageModel();
                    img.ArticleID = article.ArticleID;
                    img.LikeCount = _uaMgr.GetLikeList(img.ArticleID).Count();
                    img.CommentCount = _uaMgr.GetCommentList(img.ArticleID).Count();
                    img.CreateTime = article.CreateTime;
                    img.PicturePath = _mgr.GetCoverImgPath(img.ArticleID);
                    imageList.Add(img);

                }

            }

            this.rptUserPage.DataSource = imageList;
            this.rptUserPage.DataBind();
            this.ltlArticleCount.Text = imageList.Count.ToString();
        }




        private void InitMyPage()
        {
            this.plcPageOwner.Visible = true;
            this.plcGuest.Visible = false;
        }
        private void InitGuestMode(Guid nowUserID, Guid ownerUserID)
        {
            this.plcPageOwner.Visible = false;
            this.plcGuest.Visible = true;
            if (!_uaMgr.isFollowed(nowUserID, ownerUserID))
                this.btnFollow.Text = "追蹤";
            else
                this.btnFollow.Text = "取消追蹤";
        }
        protected void btnChange_Click(object sender, EventArgs e)
        {
            Response.Redirect($"UserInfoEdit.aspx?User={_nowUser.Account}");
        }

        protected void lkbShowArticle_Click(object sender, EventArgs e)
        {
            this.plcShowArticle.Visible = true;
            this.plcShowMap.Visible = false;
            this.lkbShowArticle.Enabled = false;
            this.lkbShowMap.Enabled = true;
        }

        protected void lkbShowMap_Click(object sender, EventArgs e)
        {
            this.plcShowArticle.Visible = false;
            this.plcShowMap.Visible = true;
            this.lkbShowArticle.Enabled = true;
            this.lkbShowMap.Enabled = false;
        }
    }
}