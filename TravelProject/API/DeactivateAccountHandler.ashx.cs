using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelProject.Managers;
using TravelProject.Models;

namespace TravelProject.API
{
    /// <summary>
    /// DeactivateAccountHandler 的摘要描述
    /// </summary>
    public class DeactivateAccountHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        private AccountManager _accMgr = new AccountManager();

        public void ProcessRequest(HttpContext context)
        {
            // Deactivate Account
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("Send", context.Request.QueryString["Action"], true) == 0)
            {
                string reason = context.Request.Form["Reason"];
                string content = context.Request.Form["Content"];

                DeactivateApplicationModel dModel = new DeactivateApplicationModel()
                {
                    Reason = reason,
                    DeactContent = content
                };
                this._accMgr.AddToDeactivateList(dModel);
                this._accMgr.DeactivateAccount();

                context.Response.ContentType = "text/plain";
                context.Response.Write("OK");

                context.Session["DeactMsg"] = "已受理您的帳號停用申請。";

                // 登出並導回登入頁
                new AccountManager().Logout();
                //context.Response.Redirect("login.aspx");

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