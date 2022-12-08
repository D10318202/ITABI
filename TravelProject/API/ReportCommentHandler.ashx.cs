using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelProject.Managers;
using TravelProject.Models;

namespace TravelProject.API
{
    /// <summary>
    /// ReportCommentHandler 的摘要描述
    /// </summary>
    public class ReportCommentHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        private UserActiveManager _uaMgr = new UserActiveManager();

        public void ProcessRequest(HttpContext context)
        {
            // Report Article
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("SendA", context.Request.QueryString["Action"], true) == 0)
            {
                string id = context.Request.Form["ID"];
                string reportType = context.Request.Form["ReportType"];
                string reason = context.Request.Form["Reason"];
                string content = context.Request.Form["Content"];

                if (Guid.TryParse(id, out Guid reportedID))
                {
                    ReportedModel rModel = new ReportedModel()
                    {
                        ReportedID = reportedID,
                        ReportType = reportType,
                        Reason = reason,
                        ReasonContent = content
                    };
                    this._uaMgr.ReportThis(rModel);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("OK");
                }
                else
                    return;
            }

            // Report Comment
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("SendC", context.Request.QueryString["Action"], true) == 0)
            {
                string id = context.Request.Form["ID"];
                string reportType = context.Request.Form["ReportType"];
                string reason = context.Request.Form["Reason"];
                string content = context.Request.Form["Content"];

                if (Guid.TryParse(id, out Guid reportedID))
                {
                    ReportedModel rModel = new ReportedModel()
                    {
                        ReportedID = reportedID,
                        ReportType = reportType,
                        Reason = reason,
                        ReasonContent = content
                    };
                    this._uaMgr.ReportThis(rModel);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("OK");
                }
                else
                    return;
            }
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