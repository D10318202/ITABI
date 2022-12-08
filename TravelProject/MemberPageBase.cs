using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelProject.Models;
using TravelProject.Managers;

namespace TravelProject
{
    public class MemberPageBase : System.Web.UI.Page
    {
        private UserActiveManager _uaMgr = new UserActiveManager();
        private AccountManager _accMgr = new AccountManager();
        private static UserAccountModel _nowUser;
        private ArticleManager _artMgr = new ArticleManager();
        private ArticleModel _atc;

        public ArticleViewLimitEnum GetFollowerViewLimit(Guid articleID)
        {
            // ArticleID → Get Article Author → (v)GetFansList(_author.UserID) → return ViewLimitEnum

            // GetUserID of Article
            _atc = _artMgr.GetArticle(articleID);
            _nowUser = _accMgr.GetCurrentUser();

            List<string> fansList = _uaMgr.GetFansStringList(_atc.UserID);
            if (fansList.Contains(_nowUser.Account))
                return ArticleViewLimitEnum.FollowersLimit;
            else
                return ArticleViewLimitEnum.Private;
        }

        public CommentLimitEnum GetCommentLimit(Guid articleID)
        {
            _atc = _artMgr.GetArticle(articleID);
            _nowUser = _accMgr.GetCurrentUser();

            List<string> fansList = _uaMgr.GetFansStringList(_atc.UserID);
            if (fansList.Contains(_nowUser.Account))
                return CommentLimitEnum.FollowersLimit;
            else
                return CommentLimitEnum.Private;
        }
    }
}