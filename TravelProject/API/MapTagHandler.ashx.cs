using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelProject.Managers;
using TravelProject.Models;

namespace TravelProject.API
{
    /// <summary>
    /// MapTagHandler 的摘要描述
    /// </summary>
    public class MapTagHandler : IHttpHandler
    {
        private static AccountManager _accMgr = new AccountManager();
        private static ArticleManager _artMgr = new ArticleManager();
        public void ProcessRequest(HttpContext context)
        {
            string account = context.Request.QueryString["User"];
            if (account != null)
            {
                UserAccountModel accountModel = _accMgr.GetAccountModel(account);
                List<ArticleModel> articleList = _artMgr.GetArticleList(accountModel.UserID);
                List<MapTag> TagList = new List<MapTag>();
                foreach (ArticleModel article in articleList)
                {
                    if (!string.IsNullOrWhiteSpace(article.PlaceID))
                    {
                        MapTag tag = new MapTag()
                        {
                            ArticleID = article.ArticleID,
                            DistrictName = article.District,
                            Longitude = article.Longitude,
                            Latitude = article.Latitude
                        };
                        TagList.Add(tag);
                    }
                }
                string result = Newtonsoft.Json.JsonConvert.SerializeObject(TagList);
                context.Response.ContentType = "application/json";
                context.Response.Write(result);
            }
            else
                return;
        }
        public class MapTag
        {
            public Guid ArticleID { get; set; }
            public string DistrictName { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
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