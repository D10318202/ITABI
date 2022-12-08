using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TravelProject.Helpers;
using TravelProject.Models;

namespace TravelProject.Managers
{
    //0=>follow 1=>like 2=>comment 3=>tag
    public class NotifyManager
    {
        private static AccountManager _accMgr = new AccountManager();
        private static ArticleManager _artMgr = new ArticleManager();
        public void AddFollowNotify(Guid notifyUserID, Guid actUserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [Notices] 
                        (UserID, ActUserID, NotifyType, CreateTime, IsViewed, ExpireTime)
                    VALUES  
                        (@UserID, @ActUserID, @NotifyType, @CreateTime, @IsViewed, @ExpireTime) ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@UserID", notifyUserID);
                        command.Parameters.AddWithValue("@ActUserID", actUserID);
                        command.Parameters.AddWithValue("@NotifyType", NotifyType.Follow);
                        command.Parameters.AddWithValue("@IsViewed", false);
                        command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                        command.Parameters.AddWithValue("@ExpireTime", DateTime.Now.AddYears(1));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.AddFollowNotify", ex);
                throw;
            }
        }
        public void AddLikeNotify(Guid actUserID, Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [Notices] 
                        (UserID, ActUserID, NotifyType, ArticleID, CreateTime, IsViewed, ExpireTime)
                    VALUES  
                        (@UserID, @ActUserID, @NotifyType, @ArticleID, @CreateTime, @IsViewed, @ExpireTime) ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        ArticleModel article = _artMgr.GetArticle(articleID);
                        command.Parameters.AddWithValue("@UserID", article.UserID);
                        command.Parameters.AddWithValue("@ActUserID", actUserID);
                        command.Parameters.AddWithValue("@NotifyType", NotifyType.Like);
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.Parameters.AddWithValue("@IsViewed", false);
                        command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                        command.Parameters.AddWithValue("@ExpireTime", DateTime.Now.AddYears(1));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.AddLikeNotify", ex);
                throw;
            }
        }
        public void AddCommentNotify(Guid actUserID, Guid articleID, Guid commentID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [Notices] 
                        (UserID, ActUserID, NotifyType, ArticleID, CommentID, CreateTime, IsViewed, ExpireTime)
                    VALUES  
                        (@UserID, @ActUserID, @NotifyType, @ArticleID, @CommentID, @CreateTime, @IsViewed, @ExpireTime) ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        //get article owner
                        ArticleModel article = _artMgr.GetArticle(articleID);
                        command.Parameters.AddWithValue("@UserID", article.UserID);
                        command.Parameters.AddWithValue("@ActUserID", actUserID);
                        command.Parameters.AddWithValue("@NotifyType", NotifyType.Comment);
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.Parameters.AddWithValue("@CommentID", commentID);
                        command.Parameters.AddWithValue("@IsViewed", false);
                        command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                        command.Parameters.AddWithValue("@ExpireTime", DateTime.Now.AddYears(1));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.AddCommentNotify", ex);
                throw;
            }
        }
        public void AddTagNotify(Guid taggedUserID, Guid actUserID, Guid articleID, Guid commentID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [Notices] 
                        (UserID, ActUserID, NotifyType, ArticleID, CommentID, CreateTime, IsViewed, ExpireTime)
                    VALUES  
                        (@UserID, @ActUserID, @NotifyType, @ArticleID, @CommentID, @CreateTime, @IsViewed, @ExpireTime) ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@UserID", taggedUserID);
                        command.Parameters.AddWithValue("@ActUserID", actUserID);
                        command.Parameters.AddWithValue("@NotifyType", NotifyType.Tag);
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.Parameters.AddWithValue("@CommentID", commentID);
                        command.Parameters.AddWithValue("@IsViewed", false);
                        command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                        command.Parameters.AddWithValue("@ExpireTime", DateTime.Now.AddYears(1));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.AddTagNotify", ex);
                throw;
            }
        }
        public void DeleteLikeNotify( Guid actUserID, Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Notices]
                    WHERE NotifyType = @NotifyType AND ActUserID = @ActUserID AND ArticleID = @ArticleID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@NotifyType", NotifyType.Like);
                        command.Parameters.AddWithValue("@ActUserID", actUserID);
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.DeleteLikeNotify", ex);
                throw;
            }
        }
        public void DeleteFollowNotify(Guid notifyUserID, Guid actUserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Notices]
                    WHERE NotifyType = @NotifyType AND ActUserID = @ActUserID AND UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@NotifyType", NotifyType.Follow);
                        command.Parameters.AddWithValue("@ActUserID", actUserID);
                        command.Parameters.AddWithValue("@UserID", notifyUserID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.DeleteFollowNotify", ex);
                throw;
            }
        }
        public void DeleteCommentNotify(Guid commentID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Notices]
                    WHERE CommentID = @CommentID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@CommentID", commentID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.DeleteCommentNotify", ex);
                throw;
            }
        }
        public void ViewedNotify(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE [Notices] 
                    SET IsViewed = @IsViewed, ExpireTime = @ExpireTime
                    WHERE UserID = @UserID  AND IsViewed = 'false' ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@IsViewed", true);
                        //100天後不顯示已讀通知
                        command.Parameters.AddWithValue("@ExpireTime", DateTime.Now.AddDays(100));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.ViewedNotify", ex);
                throw;
            }
        }
        public List<NoticeModel> GetNotifyList(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Notices]
                     WHERE UserID = @UserID
                     ORDER BY CreateTime DESC ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<NoticeModel> notifyList = new List<NoticeModel>();
                        while (reader.Read())
                        {
                            NoticeModel notify = BuildNotifyModel(reader);
                            notifyList.Add(notify);
                        }
                        return notifyList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.GetCollectList", ex);
                throw;
            }
        }
        private NoticeModel BuildNotifyModel(SqlDataReader reader)
        {
            string articleID = reader["ArticleID"].ToString();
            UserAccountModel userModel = _accMgr.GetAccountModel((Guid)reader["ActUserID"]);
            DateTime expireTime = (DateTime)reader["ExpireTime"];
            DateTime createTime = (DateTime)reader["CreateTime"];
            if (expireTime > DateTime.Now)
            {
                NoticeModel notify = new NoticeModel();
                notify.UserID = (Guid)reader["UserID"];
                notify.ActAccount = userModel.Account;
                notify.ProfileImg = userModel.ProfileImg;
                notify.IsViewed = (bool)reader["IsViewed"];
                notify.CreateTime = createTime.ToString("yyyy/MM/dd HH:mm");
                int nfType = Convert.ToInt32(reader["NotifyType"]);
                switch (nfType)
                {
                    case 0:
                        notify.NfType = NotifyType.Follow;
                        notify.Url = "UserPage.aspx?User=" + notify.ActAccount;
                        notify.NotifyContent = "開始追蹤您";
                        break;
                    case 1:
                        notify.NfType = NotifyType.Like;
                        notify.Url = "ViewArticle.aspx?Post=" + articleID;
                        notify.NotifyContent = "對您的文章表示喜歡";
                        break;
                    case 2:
                        notify.NfType = NotifyType.Comment;
                        notify.Url = "ViewArticle.aspx?Post=" + articleID;
                        notify.NotifyContent = "在您的文章下留言";
                        break;
                    case 3:
                        notify.NfType = NotifyType.Tag;
                        notify.Url = "ViewArticle.aspx?Post=" + articleID;
                        notify.NotifyContent = "在一則留言中提到您";
                        break;
                }
                return notify;
            }
            return new NoticeModel();
        }
    }
}