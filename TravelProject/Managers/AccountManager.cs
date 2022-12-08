using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using TravelProject.Helpers;
using TravelProject.Models;

namespace TravelProject.Managers
{
    public class AccountManager
    {
        public bool TryLogin(string account, string password)
        {
            bool isAccRight = false;
            bool isPwdRight = false;

            UserAccountModel User = this.GetAccountModel(account);
            if (User == null)
                return false;
            int PWDkey = Convert.ToInt32(User.PWDkey);
            string EncodePWD = EncodePassword(password, PWDkey);
            if (string.Compare(User.Account, account, true) == 0)
                isAccRight = true;
            if (string.Compare(User.PWD, EncodePWD, true) == 0)
                isPwdRight = true;

            bool result = (isAccRight && isPwdRight);

            if (result)
            {
                User.PWD = null;
                HttpContext.Current.Session["UserAccount"] = User;
            }
            return result;
        }
        public bool IsLogin()
        {
            UserAccountModel account = GetCurrentUser();
            return (account != null);
        }
        public void Logout()
        {
            HttpContext.Current.Session.Remove("UserAccount");
        }
        public UserAccountModel GetCurrentUser()
        {
            UserAccountModel account = HttpContext.Current.Session["UserAccount"] as UserAccountModel;
            return account;
        }
        public UserAccountModel GetAccountModel(string account)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM [UserAccounts]
                    WHERE Account = @account ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@account", account.ToLower());
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            UserAccountModel User = BuildUserAccount(reader);
                            return User;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.GetAccount", ex);
                throw;
            }
        }
        public UserAccountModel GetAccountModel(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM [UserAccounts]
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

                        if (reader.Read())
                        {
                            UserAccountModel User = BuildUserAccount(reader);
                            return User;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.GetAccount", ex);
                throw;
            }
        }

        public string GetAccount(Guid UserID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM [UserAccounts]
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

                        if (reader.Read())
                        {
                            UserAccountModel User = BuildUserAccount(reader);
                            return User.Account;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.GetAccount", ex);
                throw;
            }
        }




        public string GetProfileImg(Guid UserID)
        {
            {
                string connStr = ConfigHelper.GetConnectionString();
                string commandText =
                    @"  SELECT ProfileImg
                    FROM [UserAccounts]
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

                            if (reader.Read())
                                return reader["ProfileImg"] as string;

                        }
                            return null;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("AccountManager.GetAccount", ex);
                    throw;
                }
            }
        }




        public void CreateAccount(UserAccountModel User)
        {
            if (GetAccountModel(User.Account) != null)
                throw new Exception("已存在相同帳號");

            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [UserAccounts] (UserID, Account, PWD, PWDkey, CreateDate,AccountStates, Email, UserLevel)
                    VALUES (@UserID, @Account, @PWD, @PWDkey, @CreateDate, @AccountStates, @Email, @UserLevel) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", Guid.NewGuid());
                        command.Parameters.AddWithValue("@Account", User.Account.ToLower());
                        command.Parameters.AddWithValue("@PWD", User.PWD);
                        command.Parameters.AddWithValue("@PWDkey", User.PWDkey);
                        command.Parameters.AddWithValue("@Email", User.Email);
                        command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@AccountStates", true);
                        command.Parameters.AddWithValue("@UserLevel", (int)User.UserLevel);

                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.CreateAccount", ex);
                throw;
            }
        }










        //改它↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        //改它↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        //改它↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        //改它↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        //public void UpdateAccountExcPWDAndCover(UserAccountModel user) //不更新密碼和照片
        //{
        //    string connStr = ConfigHelper.GetConnectionString();
        //    string commandText =
        //        @"  UPDATE [UserAccounts]
        //            SET
        //                Account = @Account,
        //                ProfileContent = @ProfileContent,
        //                ProfileImg = @ProfileImg,
        //                NickName = @NickName
        //            WHERE
        //                UserID = @UserID ";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            using (SqlCommand command = new SqlCommand(commandText, conn))
        //            {
        //                command.Parameters.AddWithValue("@UserID", user.UserID);
        //                command.Parameters.AddWithValue("@Account", user.Account);
        //                command.Parameters.AddWithValue("@ProfileContent", user.ProfileContent);
        //                command.Parameters.AddWithValue("@ProfileImg", user.ProfileImg);
        //                command.Parameters.AddWithValue("@NickName", user.NickName);

        //                conn.Open();
        //                command.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("AccountManager.UpdateAccountExcPWD", ex);
        //        throw;
        //    }
        //}   
        ////public void UpdateAccountExcPWD(UserAccountModel user) //不更新密碼
        ////{
        ////    string connStr = ConfigHelper.GetConnectionString();
        ////    string commandText =
        ////        @"  UPDATE [UserAccounts]
        ////            SET
        ////                Account = @Account,
        ////                ProfileContent = @ProfileContent,
        ////                NickName = @NickName,
        ////                ProfileImg = @ProfileImg
        ////            WHERE
        ////                UserID = @UserID ";
        ////    try
        ////    {
        ////        using (SqlConnection conn = new SqlConnection(connStr))
        ////        {
        ////            using (SqlCommand command = new SqlCommand(commandText, conn))
        ////            {
        ////                command.Parameters.AddWithValue("@UserID", user.UserID);
        ////                command.Parameters.AddWithValue("@Account", user.Account);
        ////                command.Parameters.AddWithValue("@ProfileContent", user.ProfileContent);
        ////                command.Parameters.AddWithValue("@NickName", user.NickName);
        ////                command.Parameters.AddWithValue("@ProfileImg", user.ProfileImg);

        ////                conn.Open();
        ////                command.ExecuteNonQuery();
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Logger.WriteLog("AccountManager.UpdateAccountExcPWD", ex);
        ////        throw;
        ////    }
        ////}
        //public void UpdateAccountInclPWDAndCover(UserAccountModel user) //更新密碼/和照片
        //{
        //    string connStr = ConfigHelper.GetConnectionString();
        //    string commandText =
        //        @"  UPDATE [UserAccounts]
        //            SET
        //                Account = @Account,
        //                PWD = @PWD,
        //                ProfileContent = @ProfileContent,
        //                NickName = @NickName,
        //                ProfileImg = @ProfileImg
        //            WHERE
        //                UserID = @UserID ";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            using (SqlCommand command = new SqlCommand(commandText, conn))
        //            {
        //                command.Parameters.AddWithValue("@UserID", user.UserID);
        //                command.Parameters.AddWithValue("@Account", user.Account);
        //                command.Parameters.AddWithValue("@PWD", user.PWD);
        //                command.Parameters.AddWithValue("@ProfileContent", user.ProfileContent);
        //                command.Parameters.AddWithValue("@NickName", user.NickName);
        //                command.Parameters.AddWithValue("@ProfileImg", user.ProfileImg);

        //                conn.Open();
        //                command.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("AccountManager.UpdateAccountInclPWD", ex);
        //        throw;
        //    }
        //}


        //改它↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        //改它↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        //改它↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        //改它↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        //改它↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑




        
        public void UpdateAccountExcPWD(UserAccountModel user)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE [UserAccounts]
                    SET
                        Account = @Account,
                        ProfileContent = @ProfileContent,
                        ProfileImg = @ProfileImg,
                        NickName = @NickName,
                        UserLevel = @UserLevel
                    WHERE
                        UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", user.UserID);
                        command.Parameters.AddWithValue("@Account", user.Account.ToLower());
                        command.Parameters.AddWithValue("@ProfileContent", user.ProfileContent);
                        command.Parameters.AddWithValue("@ProfileImg", user.ProfileImg);
                        command.Parameters.AddWithValue("@NickName", user.NickName);
                        command.Parameters.AddWithValue("@UserLevel", (int)user.UserLevel);

                        conn.Open();
                        command.ExecuteNonQuery();

                        UserAccountModel User = new UserAccountModel()
                        {
                            UserID = user.UserID,
                            Account = user.Account.ToLower()
                        };
                        HttpContext.Current.Session["UserAccount"] = User;
                        HttpContext.Current.Response.Redirect($"UserPage.aspx?User={User.Account}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.UpdateAccountExcPWD", ex);
                throw;
            }
        }
        public void UpdateAccountInclPWD(UserAccountModel user)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE [UserAccounts]
                    SET
                        Account = @Account,
                        PWD = @PWD,
                        PWDkey = @PWDkey,
                        ProfileContent = @ProfileContent,
                        ProfileImg = @ProfileImg,
                        NickName = @NickName,
                        UserLevel = @UserLevel
                    WHERE
                        UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", user.UserID);
                        command.Parameters.AddWithValue("@Account", user.Account.ToLower());
                        command.Parameters.AddWithValue("@PWD", user.PWD);
                        command.Parameters.AddWithValue("@PWDkey", user.PWDkey);
                        command.Parameters.AddWithValue("@ProfileContent", user.ProfileContent);
                        command.Parameters.AddWithValue("@ProfileImg", user.ProfileImg);
                        command.Parameters.AddWithValue("@NickName", user.NickName);
                        command.Parameters.AddWithValue("@UserLevel", (int)user.UserLevel);

                        conn.Open();
                        command.ExecuteNonQuery();

                        UserAccountModel User = new UserAccountModel()
                        {
                            UserID = user.UserID,
                            Account = user.Account.ToLower()
                        };
                        HttpContext.Current.Session["UserAccount"] = User;
                        HttpContext.Current.Response.Redirect($"UserPage.aspx?User={User.Account}");

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.UpdateAccountInclPWD", ex);
                throw;
            }
        }

        
















        #region 停用
        public void DeactivateAccount()
        {
            UserAccountModel uaModel = GetCurrentUser();

            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  UPDATE [UserAccounts]
                    SET
                        AccountStates = @AccountStates,
                        DeactivateDate = @DeactivateDate
                    WHERE
                        UserID = @UserID ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", uaModel.UserID);
                        command.Parameters.AddWithValue("@AccountStates", false);
                        command.Parameters.AddWithValue("@DeactivateDate", DateTime.Now.ToString("yyyy/MM/dd"));

                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.UpdateAccountExcPWD", ex);
                throw;
            }
        }
        public void AddToDeactivateList(DeactivateApplicationModel dModel)
        {
            UserAccountModel uaModel = GetCurrentUser();


            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  INSERT INTO [DeactivateApplications] 
                        (ID, UserID, ApplicationDate, Reason, DeactContent)
                    VALUES  
                        (@ID, @UserID, @ApplicationDate, @Reason, @DeactContent) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ID", Guid.NewGuid());
                        command.Parameters.AddWithValue("@UserID", uaModel.UserID);
                        command.Parameters.AddWithValue("@ApplicationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Reason", Convert.ToInt32(dModel.Reason));
                        command.Parameters.AddWithValue("@DeactContent", dModel.DeactContent);
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
        #endregion






        private static UserAccountModel BuildUserAccount(SqlDataReader reader)
        {
            UserAccountModel userModel = new UserAccountModel();
            userModel.UserID = (Guid)reader["UserID"];
            userModel.Account = reader["Account"] as string;
            userModel.PWD = reader["PWD"] as string;
            userModel.PWDkey = reader["PWDkey"] as string;
            userModel.CreateDate = (DateTime)reader["CreateDate"];
            userModel.AccountStates = (bool)reader["AccountStates"];
            userModel.ProfileContent = reader["ProfileContent"] as string;
            userModel.Email = reader["Email"] as string;
            userModel.NickName = reader["NickName"] as string;
            userModel.ProfileImg = reader["ProfileImg"] as string;
            userModel.UserLevel = (UserLevelEnumModel)reader["UserLevel"];

            return userModel;
        }
        public string EncodePassword(string pwd, out int key)
        {
            Random rnd = new Random();
            key = rnd.Next(10000, 99999);
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key.ToString());
            byte[] messageBytes = encoding.GetBytes(pwd);
            using (var hmacSHA256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacSHA256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
            }
        }
        public string EncodePassword(string pwd, int key)
        {
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key.ToString());
            byte[] messageBytes = encoding.GetBytes(pwd);
            using (var hmacSHA256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacSHA256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
            }
        }



        #region 後臺功能


        //取得帳號列表
        public List<UserAccountModel> GetAccountList(string keyword) //增加3.31
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                @"  SELECT *
                    FROM UserAccounts ";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                commandText += " WHERE [Account] LIKE '%'+@keyword+'%'";
            }

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
                        List<UserAccountModel> list = new List<UserAccountModel>();

                        while (reader.Read())
                        {
                            UserAccountModel model = BuildUserAccount(reader);
                            model.PWD = null;
                            list.Add(model);
                        }

                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.GetAccountList", ex);
                throw;
            }
        }
        public List<DeactivateApplicationModel> GetDeactivateList()
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@" SELECT *
                    FROM DeactivateApplications
                    ORDER BY ApplicationDate DESC ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<DeactivateApplicationModel> deaList = new List<DeactivateApplicationModel>();    // 將資料庫內容轉為自定義型別清單
                        while (reader.Read())
                        {
                            DeactivateApplicationModel dea = new DeactivateApplicationModel()
                            {
                                UserID = (Guid)reader["UserID"],
                                ApplicationDate = (DateTime)reader["ApplicationDate"],
                                DeactContent = reader["DeactContent"] as string,
                                Reason = reader["Reason"] as string,
                            };
                            deaList.Add(dea);
                        }
                        return deaList;
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