using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelProject.Models;
using TravelProject.Managers;

namespace TravelProject.API
{
    /// <summary>
    /// ReadNoticeHandler 的摘要描述
    /// </summary>
    public class ReadNoticeHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        private static NotifyManager _nfMgr = new NotifyManager();
        public void ProcessRequest(HttpContext context)
        {
            UserAccountModel nowUser = HttpContext.Current.Session["UserAccount"] as UserAccountModel;
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0)
            {
                _nfMgr.ViewedNotify(nowUser.UserID);
                context.Response.ContentType = "text/plain";
                context.Response.Write("notices viewed");
            }

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