using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelProject.Managers;
using TravelProject.Models;

namespace TravelProject.API
{
    /// <summary>
    /// FollowHandler 的摘要描述
    /// </summary>
    public class FollowHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        private static AccountManager _accNgr = new AccountManager();
        private static UserActiveManager _uaMgr = new UserActiveManager();
        public void ProcessRequest(HttpContext context)
        {
            UserAccountModel nowUser = HttpContext.Current.Session["UserAccount"] as UserAccountModel;
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 &&
                string.Compare("Follow", context.Request.QueryString["Action"], true) == 0)
            {
                string account = context.Request.Form["UserAccount"];
                UserAccountModel userModel = _accNgr.GetAccountModel(account);
                _uaMgr.PressFollow(nowUser.UserID, userModel.UserID);
                context.Response.ContentType = "text/plain";
                context.Response.Write("success follow");
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 &&
                    string.Compare("UnFollow", context.Request.QueryString["Action"], true) == 0)
            {
                string account = context.Request.Form["UserAccount"];
                UserAccountModel userModel = _accNgr.GetAccountModel(account);
                _uaMgr.UnFollow(nowUser.UserID, userModel.UserID);
                context.Response.ContentType = "text/plain";
                context.Response.Write("success unfollow");
            }
            else
                return;


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}