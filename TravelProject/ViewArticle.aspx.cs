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
    public partial class ViewArticle : System.Web.UI.Page
    {
        private ArticleManager _mgr = new ArticleManager();
        private AccountManager _accMgr = new AccountManager();
        private UserActiveManager _uaMgr = new UserActiveManager();
        private NotifyManager _nfMgr = new NotifyManager();
        private MemberPageBase _mpB = new MemberPageBase();
        private static UserAccountModel _account;
        private static Guid _articleID;

        protected void Page_Load(object sender, EventArgs e)
        {
            string articleIDText = this.Request.QueryString["Post"];
            if (!Guid.TryParse(articleIDText, out _articleID))
                Response.Redirect("index.aspx", true);

            ArticleModel article = _mgr.GetArticle(_articleID);
            string coverPath = _mgr.GetCoverImgPath(_articleID);
            List<ImageModel> articleImagesList = _mgr.GetArticleImageList(_articleID);
            this.hfID2.Value = article.ArticleID.ToString();
            _account = HttpContext.Current.Session["UserAccount"] as UserAccountModel;

            ArticleViewLimitEnum limitEnum = _mpB.GetFollowerViewLimit(_articleID);

            if (!IsPostBack)
            {
                InitComment(_articleID);
            }

            InitLike(_articleID);
            InitCollect();

            //取得本文的發布者           

            string UserAccount = _accMgr.GetAccount(article.UserID);
            string profileImgPath = _accMgr.GetProfileImg(article.UserID);

            if (!IsAtcAuthorized(article.ViewLimit, limitEnum, _account.UserID, article.UserID))
                Response.Redirect("index.aspx", true);

            if (IsAtcAuthorized(article.ViewLimit, limitEnum, _account.UserID, article.UserID))
            {
                this.ImgUser.ImageUrl = profileImgPath;
                this.ltlUser.Text = $"<a href=\"UserPage.aspx?User={UserAccount}\">{UserAccount}</a>";
                this.ltlDistrict.Text = article.District;
                this.ltlArtContent.Text = article.ArticleContent;
                this.ltlCreateDate.Text = article.CreateTime.ToString("yyyy-MM-dd HH:mm");


                this.imgCover.Src = coverPath;
                foreach (var i in articleImagesList)
                {
                    this.rptPicture.DataSource = articleImagesList;
                    this.rptPicture.DataBind();

                    //this.rptPicturePage.DataSource = articleImagesList;
                    //this.rptPicturePage.DataBind();
                }







            }
            if (_account.UserID == article.UserID)
                this.plcEdit.Visible = true;



            CommentLimitEnum cmtLimitEnum = _mpB.GetCommentLimit(_articleID);
            if (article.CommentLimit != CommentLimitEnum.Public && cmtLimitEnum != article.CommentLimit && _account.UserID != article.UserID)
            {
                this.plcCanComment.Visible = false;
                this.ltlCommentMsg.Visible = true;
            }




















        }

        private bool IsAtcAuthorized(ArticleViewLimitEnum ViewLimit, ArticleViewLimitEnum limitEnum, Guid currentUserID, Guid atcUserID)
        {
            if (ViewLimit == ArticleViewLimitEnum.Public || limitEnum == ViewLimit || currentUserID == atcUserID)
                return true;
            else
                return false;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect($"PostArticle.aspx?Post={_articleID}&Editor={_account.UserID}");
        }

        protected void btnLike_Click(object sender, EventArgs e)
        {
            if (!_uaMgr.isLiked(_articleID, _account.UserID))
                _uaMgr.PressLike(_articleID, _account.UserID);
            else
                _uaMgr.DisLike(_articleID, _account.UserID);
            InitLike(_articleID);
        }

        protected void btnCollect_Click(object sender, EventArgs e)
        {
            if (!_uaMgr.isCollected(_articleID, _account.UserID))
                _uaMgr.PressCollect(_articleID, _account.UserID);

            else
                _uaMgr.DisCollect(_articleID, _account.UserID);
            InitCollect();
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            CommentModel comment = new CommentModel()
            {
                UserID = _account.UserID,
                ArticleID = _articleID,
                CommentContent = this.txtComment.Text.Trim()
            };
            _uaMgr.LeaveComment(comment);
            this.txtComment.Text = null;
            ArticleModel article = _mgr.GetArticle(_articleID);    // unnecessary??
            InitComment(_articleID);
        }
        private void InitComment(Guid articleID)
        {
            List<CommentModel> commentList = _uaMgr.GetCommentList(articleID);
            this.rptComment.DataSource = commentList;
            this.rptComment.DataBind();
            this.ltlCommentCount.Text = commentList.Count.ToString() + "則留言";

            ArticleModel article = _mgr.GetArticle(_articleID);
            foreach (RepeaterItem item in this.rptComment.Items)
            {
                GetCommentDelBtnVisibility(item, article);
            }
        }

        private void GetCommentDelBtnVisibility(RepeaterItem item, ArticleModel article)
        {
            LinkButton rptCmtDel = item.FindControl("rptCmtDel") as LinkButton;
            if (rptCmtDel != null)
            {
                if (_account.UserID == article.UserID)
                {
                    rptCmtDel.Visible = true;
                }
                else
                {
                    string commentIdText = rptCmtDel.CommandArgument as string;
                    if (Guid.TryParse(commentIdText, out Guid commentID))
                    {
                        CommentModel comment = _uaMgr.GetReplier(commentID);
                        if (comment.UserID == _account.UserID)
                        {
                            rptCmtDel.Visible = true;
                        }
                        else
                            rptCmtDel.Visible = false;
                    }
                }
            }
        }

        private void InitLike(Guid articleID)
        {
            List<string> likeAccount = _uaMgr.GetLikeList(articleID);
            this.lblLikeCount.Text = likeAccount.Count.ToString();
            if (!_uaMgr.isLiked(_articleID, _account.UserID))
            {
                //this.btnLike.Text = "讚";
                this.lbtnLike.Text = "<i class='fa fa-heart-o' aria-hidden='true'></i>";
            }
            else
            {
                //this.btnLike.Text = "取消讚";
                this.lbtnLike.Text = "<i class='fa fa-heart' aria-hidden='true'></i>";
            }
        }
        private void InitCollect()
        {
            if (!_uaMgr.isCollected(_articleID, _account.UserID))
            {
                //this.btnCollect.Text = "收藏";
                this.lbtnCollect.Text = "<i class='fa fa-star-o' aria-hidden='true'></i>";
            }
            else
            {
                //this.btnCollect.Text = "取消收藏";
                this.lbtnCollect.Text = "<i class='fa fa-star' aria-hidden='true'></i>";
            }
        }



        protected void rptComment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName == "DeleteButton")
                {
                    string commentIdText = e.CommandArgument as string;
                    if (Guid.TryParse(commentIdText, out Guid commentID))
                    {
                        _uaMgr.DeleteComment(commentID);
                        InitComment(_articleID);
                    }
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string commentIdText = e.CommandArgument as string;

                if (Guid.TryParse(commentIdText, out Guid commentID))
                {
                    CommentModel comment = _uaMgr.GetReplier(commentID);

                    if (e.CommandName == "ReButton")
                    {
                        string account = comment.Account;
                        this.txtComment.Text += $"@{account} ";
                    }
                }
            }
        }

        protected void btnDelArticle_Click(object sender, EventArgs e)
        {
            // 刪除文章互動 (刪除留言、收藏、讚)→包在刪除文章中
            //_mgr.DeleteNoticeOfArticle(_articleID);
            //_mgr.DeleteCommentOfArticle(_articleID);
            //_uaMgr.DeleteCollectOfArticle(_articleID);
            //_uaMgr.DeleteLikeOfArticle(_articleID);

            // 刪除文章
            _mgr.DeleteArticle(_articleID);
            Response.Redirect($"UserPage.aspx?User={_account.Account}");
        }


    }
}