using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TravelProject.Helpers;
using TravelProject.Models;

namespace TravelProject.Managers
{
    public class UserActiveManager
    {
        private static AccountManager _accMgr = new AccountManager();
        private static ArticleManager _artMgr = new ArticleManager();
        private static NotifyManager _nfMgr = new NotifyManager();
        #region comment
        public void LeaveComment(CommentModel comment)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [Comments] 
                        (CommentID, UserID, ArticleID, CommentContent, CreateTime)
                    VALUES  
                        (@CommentID, @UserID, @ArticleID, @CommentContent, @CreateTime) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        comment.CommentID = Guid.NewGuid();
                        command.Parameters.AddWithValue("@CommentID", comment.CommentID);
                        command.Parameters.AddWithValue("@ArticleID", comment.ArticleID);
                        command.Parameters.AddWithValue("@UserID", comment.UserID);
                        command.Parameters.AddWithValue("@CommentContent", comment.CommentContent);
                        command.Parameters.AddWithValue("@CreateTime", DateTime.Now);
                        command.ExecuteNonQuery();
                        _nfMgr.AddCommentNotify(comment.UserID, comment.ArticleID, comment.CommentID);

                        string[] commentSplited = comment.CommentContent.Split(' ');
                        foreach (string item in commentSplited)
                        {
                            if (item.StartsWith("@"))
                            {
                                string reAccount = item.Remove(0, 1);
                                if (_accMgr.GetAccountModel(reAccount) != null)
                                {
                                    UserAccountModel taggedUser = _accMgr.GetAccountModel(reAccount);
                                    _nfMgr.AddTagNotify(taggedUser.UserID, comment.UserID,comment.ArticleID, comment.CommentID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.LeaveComment", ex);
                throw;
            }
        }
        public void DeleteComment(Guid commentID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Comments] 
                    WHERE  
                        CommentID = @CommentID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        //先刪通知
                        _nfMgr.DeleteCommentNotify(commentID);

                        conn.Open();
                        command.Parameters.AddWithValue("@CommentID", commentID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.DeleteComment", ex);
                throw;
            }
        }
        public List<CommentModel> GetCommentList(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Comments]
                     WHERE ArticleID = @ArticleID 
                     ORDER BY CreateTime DESC ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@ArticleID", articleID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<CommentModel> commentList = new List<CommentModel>();
                        while (reader.Read())
                        {
                            Guid UserID = (Guid)reader["UserID"];
                            CommentModel comment = new CommentModel()
                            {
                                UserID = UserID,
                                CommentContent = reader["CommentContent"] as string,
                                CreateTime = (DateTime)reader["CreateTime"],
                                Account = _accMgr.GetAccount(UserID),
                                CommentID = (Guid)reader["CommentID"],

                            };
                            string[] commentSplited = comment.CommentContent.Split(' ');

                            foreach (string item in commentSplited)
                            {
                                if (item.StartsWith("@"))
                                {
                                    string reAccount = item.Remove(0, 1);
                                    if (_accMgr.GetAccountModel(reAccount) != null)
                                    {
                                        string newCommentContent = comment.CommentContent.Replace(item, $"<a href=\"UserPage.aspx?User={reAccount}\">{item}</a>");

                                        comment.CommentContent = newCommentContent;
                                    }
                                }
                            }
                            commentList.Add(comment);
                        }
                        return commentList;
                    }
                }
            
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.GetCommentList", ex);
                throw;
            }
        }
        public CommentModel GetReplier(Guid commentID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Comments]
                     WHERE CommentID = @CommentID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@CommentID", commentID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        CommentModel comment = new CommentModel();
                        if (reader.Read())
                        {
                            Guid UserID = (Guid)reader["UserID"];
                            comment.UserID = UserID;
                            comment.Account = _accMgr.GetAccount(UserID);
                            comment.CommentID = (Guid)reader["CommentID"];
                            //comment.CommentContent = reader["CommentContent"] as string;
                        }
                        return comment;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.GetReplier", ex);
                throw;
            }
        }
        #endregion
        #region like
        public void PressLike(Guid articleID, Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [Likes] 
                        (ArticleID, UserID)
                    VALUES  
                        (@ArticleID, @UserID) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.ExecuteNonQuery();
                        _nfMgr.AddLikeNotify(UserID,  articleID);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.PressLike", ex);
                throw;
            }
        }
        public void DisLike(Guid articleID, Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Likes]
                    WHERE ArticleID = @ArticleID AND UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.ExecuteNonQuery();
                        _nfMgr.DeleteLikeNotify(UserID, articleID);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.DisLike", ex);
                throw;
            }
        }
        public void DeleteLikeOfArticle(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Likes]
                    WHERE ArticleID = @ArticleID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.DeleteLikeOfArticle", ex);
                throw;
            }
        }
        public List<string> GetLikeList(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Likes]
                     WHERE ArticleID = @ArticleID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@ArticleID", articleID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<string> likeAccount = new List<string>();
                        while (reader.Read())
                        {
                            Guid UserID = (Guid)reader["UserID"];
                            string account = _accMgr.GetAccount(UserID);
                            likeAccount.Add(account);
                        }
                        return likeAccount;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.GetLikeList", ex);
                throw;
            }
        }
        public bool isLiked(Guid articleID, Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Likes]
                     WHERE ArticleID = @ArticleID AND UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.Parameters.AddWithValue("@UserID", UserID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.isLiked", ex);
                throw;
            }
        }
        #endregion
        #region follow
        public void PressFollow(Guid UserID, Guid FansID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [FollowLists] 
                        (UserID , FansID)
                    VALUES  
                        (@UserID, @FansID) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@FansID", FansID);
                        command.ExecuteNonQuery();
                        _nfMgr.AddFollowNotify(FansID,UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.PressFollow", ex);
                throw;
            }
        }
        public void UnFollow(Guid UserID, Guid FansID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [FollowLists] 
                    WHERE UserID = @UserID AND FansID = @FansID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@FansID", FansID);
                        command.ExecuteNonQuery();
                        _nfMgr.DeleteFollowNotify(FansID, UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.UnFollow", ex);
                throw;
            }
        }
        public List<UserAccountModel> GetFollowingList(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [FollowLists]
                     WHERE UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<UserAccountModel> FollowingList = new List<UserAccountModel>();
                        while (reader.Read())
                        {
                            UserAccountModel account = _accMgr.GetAccountModel((Guid)reader["FansID"]);

                            FollowingList.Add(account);
                        }
                        return FollowingList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.GetFollowingList", ex);
                throw;
            }
        }
        public List<UserAccountModel> GetFansList(Guid FansID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [FollowLists]
                     WHERE FansID = @FansID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@FansID", FansID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<UserAccountModel> FansList = new List<UserAccountModel>();
                        while (reader.Read())
                        {
                            UserAccountModel account = _accMgr.GetAccountModel((Guid)reader["UserID"]);
                            FansList.Add(account);
                        }
                        return FansList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.GetFansList", ex);
                throw;
            }
        }
        public List<string> GetFansStringList(Guid FansID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [FollowLists]
                     WHERE FansID = @FansID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@FansID", FansID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<string> FansList = new List<string>();
                        while (reader.Read())
                        {
                            Guid UserID = (Guid)reader["UserID"];
                            string account = _accMgr.GetAccount(UserID);
                            FansList.Add(account);
                        }
                        return FansList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.GetFansList", ex);
                throw;
            }
        }
        public bool isFollowed(Guid UserID, Guid FansID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [FollowLists]
                     WHERE UserID = @UserID AND FansID = @FansID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@FansID", FansID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.isFollowed", ex);
                throw;
            }
        }
        #endregion
        #region collect
        public void PressCollect(Guid articleID, Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [Collects] 
                        (ArticleID, UserID)
                    VALUES  
                        (@ArticleID, @UserID) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.PressCollect", ex);
                throw;
            }
        }
        public void DisCollect(Guid articleID, Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Collects]
                    WHERE ArticleID = @ArticleID AND UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.DisCollect", ex);
                throw;
            }
        }
        public void DeleteCollectOfArticle(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Collects]
                    WHERE ArticleID = @ArticleID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.DeleteCollectOfArticle", ex);
                throw;
            }
        }
        public List<ArticleModel> GetCollectList(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Collects]
                     WHERE UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<ArticleModel> articleList = new List<ArticleModel>();
                       
                        while (reader.Read())
                        {
                            Guid articleID = (Guid)reader["ArticleID"];
                            ArticleModel article = _artMgr.GetArticle(articleID);
                            
                           
                            articleList.Add(article);
                        }
                        return articleList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.GetCollectList", ex);
                throw;
            }
        }
        
        //4/6新增
        public List<ImageModel> GetCollectCoverList(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Collects]
                     WHERE UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<ImageModel> imgList = new List<ImageModel>();
                        while (reader.Read())
                        {
                            Guid articleID = (Guid)reader["ArticleID"];
                            ImageModel img = _artMgr.GetImg(articleID);

                            imgList.Add(img);
                        }
                        return imgList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.GetCollectList", ex);
                throw;
            }
        }
        

        public bool isCollected(Guid articleID, Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Collects]
                     WHERE ArticleID = @ArticleID AND UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.Parameters.AddWithValue("@UserID", UserID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.isCollected", ex);
                throw;
            }
        }
        #endregion

        public void ReportThis(ReportedModel rModel)
        {
            UserAccountModel uaModel = _accMgr.GetCurrentUser();

            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [Reporteds] 
                        (ID, ReportedID, UserID, ReportDate, Reason, ReasonContent, ReportType)
                    VALUES  
                        (@ID, @ReportedID, @UserID, @ReportDate, @Reason, @ReasonContent, @ReportType) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ID", Guid.NewGuid());
                        command.Parameters.AddWithValue("@ReportedID", rModel.ReportedID);
                        command.Parameters.AddWithValue("@UserID", uaModel.UserID);
                        command.Parameters.AddWithValue("@ReportDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Reason", Convert.ToInt32(rModel.Reason));
                        command.Parameters.AddWithValue("@ReasonContent", rModel.ReasonContent);
                        command.Parameters.AddWithValue("@ReportType", rModel.ReportType);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserActiveManager.ReportThis", ex);
                throw;
            }
        }


    }
}