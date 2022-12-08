using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelProject.Models
{
    public enum UserLevelEnumModel  
    {
        Super = 1,  //文章列表和會員列表都可以進得去
        Admin = 2,  //只有會員列表都可以進得去
        New = 0
    }
}