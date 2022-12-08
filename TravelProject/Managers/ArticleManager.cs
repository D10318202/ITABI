using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelProject.Models;
using TravelProject.Helpers;
using System.Data.SqlClient;

namespace TravelProject.Managers
{
    public class ArticleManager
    {
        private static UserActiveManager _uaMgr = new UserActiveManager();
        public ArticleModel GetArticle(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Articles]
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

                        ArticleModel article = new ArticleModel();
                        if (reader.Read())
                        {
                            article = BuildArticle(reader);
                        }
                        return article;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetArticle", ex);
                throw;
            }
        }

        //4/6新增
        
        public ImageModel GetImg(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Pictures]
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

                        ImageModel img = new ImageModel();
                        if (reader.Read())
                        {
                            img = BuildImage(reader);
                        }
                        return img;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetImg", ex);
                throw;
            }
        }
    

        public List<ArticleModel> GetArticleList(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT *
                     FROM [Articles]
                     WHERE UserID = @UserID 
                     ORDER BY CreateTime DESC";
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
                            ArticleModel article = BuildArticle(reader);
                            articleList.Add(article);
                        }
                        return articleList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetArticleList", ex);
                throw;
            }
        }
        private static ArticleModel BuildArticle(SqlDataReader reader)
        {
            ArticleModel articleModel = new ArticleModel();
            articleModel.ArticleID = (Guid)reader["ArticleID"];
            articleModel.UserID = (Guid)reader["UserID"];
            articleModel.District = reader["District"] as string;
            if (reader["Longitude"] != null && reader["Latitude"] != null)
            {
                articleModel.Longitude = reader["Longitude"].ToString();
                articleModel.Latitude = reader["Latitude"].ToString();
            }
            articleModel.PlaceID = reader["PlaceID"] as string;
            articleModel.ArticleContent = reader["ArticleContent"] as string;
            articleModel.CreateTime = (DateTime)reader["CreateTime"];
            articleModel.ViewLimit = (ArticleViewLimitEnum)(reader["ViewLimit"]);
            articleModel.CommentLimit = (CommentLimitEnum)(reader["CommentLimit"]);
            return articleModel;
        }
        
        //4/6新增
        private static ImageModel BuildImage(SqlDataReader reader)
        {
            ImageModel imageModel = new ImageModel();
            imageModel.ArticleID = (Guid)reader["ArticleID"];
            imageModel.PictureID = (Guid)reader["PictureID"];
            imageModel.PicturePath = reader["PicturePath"] as string;
            imageModel.PictureNumber = (int)reader["PictureNumber"];
            return imageModel;
        }
        
        public void CreateArticle(ArticleModel articel)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [Articles] 
                        (ArticleID, UserID, District, Longitude, Latitude, PlaceID, ArticleContent, ViewLimit, CommentLimit)
                    VALUES  
                        (@ArticleID, @UserID, @District, @Longitude, @Latitude, @PlaceID, @ArticleContent, @ViewLimit, @CommentLimit) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ArticleID", articel.ArticleID);
                        command.Parameters.AddWithValue("@UserID", articel.UserID);
                        command.Parameters.AddWithValue("@District", articel.District);
                        command.Parameters.AddWithValue("@Longitude", articel.Longitude);
                        command.Parameters.AddWithValue("@Latitude", articel.Latitude);
                        command.Parameters.AddWithValue("@PlaceID", articel.PlaceID);
                        command.Parameters.AddWithValue("@ArticleContent", articel.ArticleContent);
                        command.Parameters.AddWithValue("@ViewLimit", articel.ViewLimit);
                        command.Parameters.AddWithValue("@CommentLimit", articel.CommentLimit);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.CreateArticle", ex);
                throw;
            }
        }

        public void UploadArticleImage(ImageModel image)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@" INSERT INTO Pictures                                                
                        (ArticleID, PictureID, PicturePath, PictureNumber)
                    VALUES      
                        (@ArticleID,@PictureID, @PicturePath, @PictureNumber)
                  ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@PictureID", image.PictureID);
                        command.Parameters.AddWithValue("@ArticleID", image.ArticleID);
                        command.Parameters.AddWithValue("@PicturePath", image.PicturePath);
                        command.Parameters.AddWithValue("@PictureNumber", image.PictureNumber);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.UploadArticleImage", ex);
                throw;
            }
        }

       
        public void UpdateArticle(ArticleModel articel)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE [Articles] 
                    SET District = @District, 
                        Longitude = @Longitude,
                        Latitude = @Latitude,
                        PlaceID = @PlaceID,
                        ArticleContent = @ArticleContent, 
                        ViewLimit = @ViewLimit, 
                        CommentLimit = @CommentLimit, 
                        UpdateTime = @UpdateTime 
                    WHERE ArticleID = @ArticleID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ArticleID", articel.ArticleID);
                        command.Parameters.AddWithValue("@District", articel.District);
                        command.Parameters.AddWithValue("@Longitude", articel.Longitude);
                        command.Parameters.AddWithValue("@Latitude", articel.Latitude);
                        command.Parameters.AddWithValue("@PlaceID", articel.PlaceID);
                        command.Parameters.AddWithValue("@ArticleContent", articel.ArticleContent);
                        command.Parameters.AddWithValue("@ViewLimit", articel.ViewLimit);
                        command.Parameters.AddWithValue("@CommentLimit", articel.CommentLimit);
                        command.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.UpdateArticle", ex);
                throw;
            }
        }
        public void DeleteArticle(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Articles] 
                    WHERE  
                        ArticleID = @ArticleID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        DeleteNoticeOfArticle(articleID);
                        DeleteCommentOfArticle(articleID);
                        _uaMgr.DeleteCollectOfArticle(articleID);
                        _uaMgr.DeleteLikeOfArticle(articleID);
                        DeleteImageOfArticle(articleID);
                        conn.Open();
                        command.Parameters.AddWithValue("@ArticleID", articleID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.DeleteArticle", ex);
                throw;
            }
        }
        public void DeleteCommentOfArticle(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Comments] 
                    WHERE  
                        ArticleID = @ArticleID";
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
                Logger.WriteLog("ArticleManager.DeleteCommentOfArticle", ex);
                throw;
            }
        }

        public void DeleteImageOfArticle(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Pictures] 
                    WHERE  
                        ArticleID = @ArticleID";
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
                Logger.WriteLog("ArticleManager.DeleteImageOfArticle", ex);
                throw;
            }
        }

        public void DeleteNoticeOfArticle(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  DELETE FROM [Notices] 
                    WHERE  
                        ArticleID = @ArticleID";
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
                Logger.WriteLog("ArticleManager.DeleteNoticeOfArticle", ex);
                throw;
            }
        }

        //public List<ImageModel> GetSearchArticleList(string keyword)
        //{
        //    //string whereCondition = string.Empty;
        //    //if (!string.IsNullOrWhiteSpace(keyword))
        //    //    whereCondition = " ";

        //    string connStr = ConfigHelper.GetConnectionString();
        //    string commandText =
        //        $@"SELECT Pictures.PicturePath,Pictures.ArticleID
        //           FROM Articles
        //           INNER JOIN Pictures ON Articles.ArticleID = Pictures.ArticleID
        //           WHERE 
        //                (Pictures.PictureNumber = 1) AND 
        //                District LIKE '%'+@keyword+'%'
        //           ORDER BY Articles.CreateTime DESC";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            using (SqlCommand command = new SqlCommand(commandText, conn))
        //            {

        //                command.Parameters.AddWithValue("@keyword", keyword);

        //                conn.Open();
        //                SqlDataReader reader = command.ExecuteReader();

        //                List<ImageModel> searchList = new List<ImageModel>();    // 將資料庫內容轉為自定義型別清單
        //                while (reader.Read())
        //                {
        //                    ImageModel img = new ImageModel()
        //                    {
        //                        PicturePath = reader["PicturePath"] as string,
        //                        ArticleID = (Guid)reader["ArticleID"]
        //                    };
        //                    searchList.Add(img);
        //                }
        //                return searchList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("ArticleManager.GetIndexArticleList", ex);
        //        throw;
        //    }
        //}
        //public List<ImageModel> GetIndexArticleList()
        //{

        //    string connStr = ConfigHelper.GetConnectionString();
        //    string commandText =
        //        $@"SELECT Pictures.PicturePath,Pictures.ArticleID
        //           FROM Articles
        //           INNER JOIN Pictures ON Articles.ArticleID = Pictures.ArticleID
        //           WHERE (Pictures.PictureNumber = 1)
        //           ORDER BY Articles.CreateTime DESC";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            using (SqlCommand command = new SqlCommand(commandText, conn))
        //            {
        //                conn.Open();
        //                SqlDataReader reader = command.ExecuteReader();

        //                List<ImageModel> imgList = new List<ImageModel>();    // 將資料庫內容轉為自定義型別清單
        //                while (reader.Read())
        //                {
        //                    ImageModel img = new ImageModel()
        //                    {
        //                        PicturePath = reader["PicturePath"] as string,
        //                        ArticleID = (Guid)reader["ArticleID"]
        //                    };
        //                    imgList.Add(img);
        //                }
        //                return imgList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("ArticleManager.GetIndexArticleList", ex);
        //        throw;
        //    }
        //}


        public List<ImageModel> GetRankArticleList()
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"SELECT [Articles].ArticleID, CreateTime, LikeCount
                   FROM [Articles]
                   JOIN(
                    SELECT COUNT(ArticleID) AS LikeCount, ArticleID
                     FROM [Likes]
                     GROUP BY [ArticleID] 
                    )AS [Ranks]
                   ON [Articles].ArticleID = Ranks.ArticleID 
                   ORDER BY LikeCount DESC";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<ImageModel> imgList = new List<ImageModel>();
                        while (reader.Read())
                        {
                            ImageModel img = new ImageModel();
                            img.ArticleID = (Guid)reader["ArticleID"];
                            img.LikeCount = _uaMgr.GetLikeList(img.ArticleID).Count();
                            img.CommentCount = _uaMgr.GetCommentList(img.ArticleID).Count();
                            img.CreateTime = (DateTime)reader["CreateTime"];
                            img.PicturePath = GetCoverImgPath(img.ArticleID);

                            if (img.CreateTime.AddDays(7) > DateTime.Now)
                                imgList.Add(img);
                        }
                        return imgList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetRankArticleList", ex);
                throw;
            }
        }
        public List<ImageModel> GetSearchArticleList(string keyword)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"SELECT [Pictures].PicturePath, [Pictures].ArticleID, [Articles].CreateTime
                   FROM Articles
                   INNER JOIN Pictures ON Articles.ArticleID = Pictures.ArticleID
                   WHERE 
                        (Pictures.PictureNumber = 1) AND 
                        (District LIKE '%'+@keyword+'%' OR ArticleContent LIKE '%'+@keyword+'%')                        
                   ORDER BY Articles.CreateTime DESC";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {

                        command.Parameters.AddWithValue("@keyword", keyword);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<ImageModel> searchList = new List<ImageModel>();
                        while (reader.Read())
                        {
                            ImageModel img = new ImageModel();
                            img.ArticleID = (Guid)reader["ArticleID"];
                            img.PicturePath = reader["PicturePath"] as string;
                            img.LikeCount = _uaMgr.GetLikeList(img.ArticleID).Count();
                            img.CommentCount = _uaMgr.GetCommentList(img.ArticleID).Count();
                            img.CreateTime = (DateTime)reader["CreateTime"];
                            searchList.Add(img);
                        }
                        return searchList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetIndexArticleList", ex);
                throw;
            }
        }


        //我追蹤的文章列表
        public List<ImageModel> GetFollowingArticleList(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"SELECT [FollowLists].UserID, [Articles].ArticleID, [Articles].CreateTime   
                   FROM[FollowLists]
                   JOIN(
                    SELECT ArticleID, UserID , CreateTime     
                    FROM Articles
                   )AS [Articles]
                   ON [FollowLists].FansID = [Articles].UserID
                   WHERE [Articles].UserID= @UserID
                   ORDER BY [Articles].CreateTime DESC";
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
                            ImageModel img = new ImageModel();
                            img.ArticleID = (Guid)reader["ArticleID"];
                            img.CreateTime = (DateTime)reader["CreateTime"];
                            img.LikeCount = _uaMgr.GetLikeList(img.ArticleID).Count();
                            img.CommentCount = _uaMgr.GetCommentList(img.ArticleID).Count();
                            img.PicturePath = GetCoverImgPath(img.ArticleID);

                            imgList.Add(img);
                        }
                        return imgList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetIndexArticleList", ex);
                throw;
            }
        }

        //首頁圖片
        public List<ImageModel> GetIndexArticleList()
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"SELECT [Pictures].PicturePath, [Pictures].ArticleID, [Articles].CreateTime
                   FROM Articles
                   INNER JOIN [Pictures]
                   ON [Articles].ArticleID = [Pictures].ArticleID
                   WHERE ([Pictures].PictureNumber = 1) AND ([Articles].[ViewLimit] =1)
                   ORDER BY [Articles].CreateTime DESC";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<ImageModel> imgList = new List<ImageModel>();
                        while (reader.Read())
                        {
                            ImageModel img = new ImageModel();
                            img.ArticleID = (Guid)reader["ArticleID"];
                            img.PicturePath = reader["PicturePath"] as string;
                            img.LikeCount = _uaMgr.GetLikeList(img.ArticleID).Count();
                            img.CommentCount = _uaMgr.GetCommentList(img.ArticleID).Count();
                            img.CreateTime = (DateTime)reader["CreateTime"];
                            imgList.Add(img);
                        }
                        return imgList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetIndexArticleList", ex);
                throw;
            }
        }

        //取得每篇文章第一張圖（封面）
        public string GetCoverImgPath(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@" SELECT PicturePath
                    FROM [Pictures]
                    WHERE PictureNumber = 1 AND ArticleID = @ArticleID";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@ArticleID", articleID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                            return reader["PicturePath"] as string;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetCoverImgPath", ex);
                throw;
            }
        }
        public List<ImageModel> GetCoverImgPathList(Guid articleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"SELECT [Pictures].PicturePath, [Pictures].ArticleID, [Articles].CreateTime
                   FROM Articles
                   INNER JOIN [Pictures]
                   ON [Articles].ArticleID = [Pictures].ArticleID
                   WHERE ([Pictures].PictureNumber = 1)
                   ORDER BY [Articles].CreateTime DESC";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<ImageModel> imgList = new List<ImageModel>();
                        while (reader.Read())
                        {
                            ImageModel img = new ImageModel();
                            img.ArticleID = (Guid)reader["ArticleID"];
                            img.PicturePath = reader["PicturePath"] as string;
                            img.LikeCount = _uaMgr.GetLikeList(img.ArticleID).Count();
                            img.CommentCount = _uaMgr.GetCommentList(img.ArticleID).Count();
                            img.CreateTime = (DateTime)reader["CreateTime"];
                            imgList.Add(img);
                        }
                        return imgList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetCoverImgPathList", ex);
                throw;
            }
        }

        public List<ImageModel> GetUserPageArticleList(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT ArticleID, CreateTime
                     FROM [Articles]
                     WHERE UserID = @UserID 
                     ORDER BY CreateTime DESC";
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
                            ImageModel img = new ImageModel();
                            img.ArticleID = (Guid)reader["ArticleID"];
                            img.LikeCount = _uaMgr.GetLikeList(img.ArticleID).Count();
                            img.CommentCount = _uaMgr.GetCommentList(img.ArticleID).Count();
                            img.CreateTime = (DateTime)reader["CreateTime"];
                            img.PicturePath = GetCoverImgPath(img.ArticleID);
                            imgList.Add(img);
                        }
                        return imgList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetArticleList", ex);
                throw;
            }
        }

        public ImageModel GetUserPageImageModel(Guid ArticleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT ArticleID, PicturePath
                     FROM [Pictures]
                     WHERE ArticleID = @ArticleID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@ArticleID", ArticleID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            ImageModel img = new ImageModel();
                            img.ArticleID = (Guid)reader["ArticleID"];
                            img.LikeCount = _uaMgr.GetLikeList(img.ArticleID).Count();
                            img.CommentCount = _uaMgr.GetCommentList(img.ArticleID).Count();
                            img.PicturePath = reader["PicturePath"]as string;
                            
                        return img;
                        }
                    return new ImageModel();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetArticleList", ex);
                throw;
            }
        }


        //輪播圖片（該文章非封面的所有圖片）
        public List<ImageModel> GetArticleImageList(Guid ArticleID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  SELECT ArticleID, PicturePath, PictureNumber
                     FROM [Pictures]
                     WHERE ArticleID = @ArticleID AND PictureNumber >1
				     ORDER BY PictureNumber ASC ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@ArticleID", ArticleID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<ImageModel> imgList = new List<ImageModel>();
                        while (reader.Read())
                        {
                            ImageModel img = new ImageModel();
                            img.ArticleID = (Guid)reader["ArticleID"];
                            img.PictureNumber = (int)reader["PictureNumber"];
                            img.PicturePath = (string)reader["PicturePath"];
                            imgList.Add(img);
                        }
                        return imgList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetArticleImageList", ex);
                throw;
            }

        }




        #region
        //後台   增加
        public List<ArticleModel> GetAdminArticleList(string keyword)
        {
            string whereCondition = string.Empty;
            if (!string.IsNullOrWhiteSpace(keyword))
                whereCondition = " WHERE District LIKE '%'+@keyword+'%' ";

            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@" SELECT *
                    FROM Articles
                    {whereCondition}
                    ORDER BY CreateTime DESC ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(keyword))
                        {
                            command.Parameters.AddWithValue("@keyword", keyword);
                        }

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<ArticleModel> retList = new List<ArticleModel>();    // 將資料庫內容轉為自定義型別清單
                        while (reader.Read())
                        {
                            ArticleModel info = BuildArticle(reader);
                            retList.Add(info);
                        }

                        return retList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetAdminArticleList", ex);
                throw;
            }
        }
        public List<ReportedModel> GetAdminReportedList()
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@" SELECT *
                    FROM Reporteds
                    ORDER BY ReportDate DESC ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<ReportedModel> retList = new List<ReportedModel>();    // 將資料庫內容轉為自定義型別清單
                        while (reader.Read())
                        {
                            ReportedModel rpt = new ReportedModel()
                            {
                                UserID = (Guid)reader["UserID"],
                                ReportedID = (Guid)reader["ReportedID"],
                                ReportType = reader["ReportType"] as string,
                                ReportDate = (DateTime)reader["ReportDate"],
                                Reason = reader["Reason"] as string,
                                ReasonContent = reader["ReasonContent"] as string
                            };
                            retList.Add(rpt);
                        }
                        return retList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ArticleManager.GetAdminReportedList", ex);
                throw;
            }
        }
        #endregion
    }
}