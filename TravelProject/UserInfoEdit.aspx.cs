using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TravelProject.Managers;
using TravelProject.Models;

namespace TravelProject
{
    public partial class UserInfoEdit : System.Web.UI.Page
    {
        private ArticleManager _mgr = new ArticleManager();
        private AccountManager _AccMgr = new AccountManager();
        private UserActiveManager _uaMgr = new UserActiveManager();
        private static UserAccountModel _nowUser;
        private static UserAccountModel _pageOwner;
        private MemberPageBase _mpB = new MemberPageBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            string ownerAccount = this.Request.QueryString["User"];
            if (!string.IsNullOrWhiteSpace(ownerAccount))
            {
                if (!IsPostBack)
                {
                    _pageOwner = _AccMgr.GetAccountModel(ownerAccount);
                    if (_pageOwner != null)
                    {
                        _nowUser = HttpContext.Current.Session["UserAccount"] as UserAccountModel;

                        if (_pageOwner.UserID != _nowUser.UserID)
                            Response.Redirect("index.aspx");
                        else
                            InitProfile();
                    }
                }
            }
            else
                Response.Redirect("index.aspx");

        }

        private void InitProfile()
        {
            this.txtAccount.Text = _pageOwner.Account;
            this.txtPWD.Text = "";
            this.txtChkPWD.Text = "";
            this.ltlEmail.Text = _pageOwner.Email;
            this.txtNickname.Text = _pageOwner.NickName;
            this.hfProfileImg.Value = _pageOwner.ProfileImg;
            string content = "";
            if (!string.IsNullOrWhiteSpace(_pageOwner.ProfileContent))
                content = _pageOwner.ProfileContent.Replace("<br />", "\n");
            this.txtProfile.Text = content;
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string account = this.txtAccount.Text.Trim();
            string password = this.txtPWD.Text.Trim();
            string chkPassword = this.txtChkPWD.Text.Trim();
            string nickName = this.txtNickname.Text.Trim();
            string content = this.txtProfile.Text;

            if (!CheckAccount(account, out string errorAcc))
            {
                this.lblAccMsg.Text = errorAcc;
                return;
            }

            string[] contentArr = content.Split('\n');
            string contentWithBr = string.Join("<br />", contentArr);
            UserAccountModel user = new UserAccountModel();
            user.UserID = _nowUser.UserID;
            user.Account = account;
            user.NickName = nickName;
            user.ProfileContent = contentWithBr;
            user.ProfileImg = GetPicturePath();

            if (string.IsNullOrWhiteSpace(password))
            {
                // 不更新密碼欄位
                _AccMgr.UpdateAccountExcPWD(user);
                InitProfile();
            }
            else if (!CheckPWD(password, chkPassword, out string errorPWD))
            {
                this.lblPWDMsg.Text = errorPWD;
                return;
            }
            else
            {
                //更新全部欄位
                string encodePwd = _AccMgr.EncodePassword(password, out int key);
                user.PWD = encodePwd;
                user.PWDkey = key.ToString();
                _AccMgr.UpdateAccountInclPWD(user);
                InitProfile();
            }
            Response.Redirect($"UserPage.aspx?User={_nowUser.Account}");
        }
        private string GetPicturePath()
        {
            string profileImg = this.hfProfileImg.Value;
            if (this.FuProfileImg.HasFile)
            {
                string folderPath = "/Picture/Photo/";
                string filename = (Guid.NewGuid()).ToString() + System.IO.Path.GetExtension(this.FuProfileImg.FileName);

                folderPath = HostingEnvironment.MapPath(folderPath);
                // 假如資料夾不存在，先建立
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFilePath = System.IO.Path.Combine(folderPath, filename);
                this.FuProfileImg.SaveAs(newFilePath);
                profileImg = "/Picture/Photo/" + filename;
            }
            return profileImg;
        }
        private bool CheckAccount(string account, out string errorMsg)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                errorMsg = "帳號不可為空白";
                return false;
            }
            else if (_AccMgr.GetAccountModel(account) != null)
            {
                if (_nowUser.Account != account)
                {
                    errorMsg = "已存在相同帳號";
                    return false;
                }
            }
            Regex regex = new Regex(@"^(?!.*[^\x21-\x7e])(?=.*[a-z]).{5,15}$");
            if (!regex.IsMatch(account))
            {
                errorMsg = "帳號格式不正確";
                return false;
            }
            errorMsg = null;
            return true;
        }
        private bool CheckPWD(string password, string chkPassword, out string errorMsg)
        {
            Regex regex = new Regex(@"^(?!.*[^\x21-\x7e])(?=.*[a-zA-Z])(?=.*\d).{8,15}$");
            if (!regex.IsMatch(password))
            {
                errorMsg = "密碼格式不正確";
                return false;
            }
            else if (string.Compare(password, chkPassword, false) != 0)
            {
                errorMsg = "請輸入相同密碼";
                return false;
            }
            errorMsg = null;
            return true;
        }

        protected void Logout()
        {
            new AccountManager().Logout();
        }
    }
}