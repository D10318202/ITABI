<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TravelProject.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body style="background-color: #FFFDC9">
    <form id="form1" runat="server">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-6">
                    <img style="width:100%" src="loginPhoto.png" />
                </div>
                <div class="col-6">
                    <h2>會員登入 Member Login</h2>
                    <p>尚未成為I TABI的會員嗎?</p>
                    <p>現在就創立帳號以查看此網站上的受限頁面與內容。</p>
                    <p><a href="Regester.aspx">前往註冊頁面→</a></p>
                    <asp:Literal ID="ltlMsg" runat="server"></asp:Literal><br />
                    帳號<asp:TextBox ID="txtAcc" runat="server"></asp:TextBox><br />
                    密碼<asp:TextBox ID="txtPWD" runat="server" TextMode="Password"></asp:TextBox><br />
                    <asp:Button CssClass="btn btn-itabi" ID="btnLogin" runat="server" Text="登入" OnClick="btnLogin_Click" /><br />
                    <p>若您忘記密碼請跟我們聯絡</p>
                    <strong>聯絡信箱:</strong><a href="mailto:ITABI@gmail.com">ITABI@gmail.com</a>
                </div>
            </div>
        </div>
    </form>



    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script>
            alert('<%= AlertDeactMsg %>');
        </script>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="PlaceHolder2" runat="server">
        <script>
            alert('<%= AlertRegMsg %>');
        </script>
    </asp:PlaceHolder>


     <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="JS/jquery.min.js"></script>
    <script src="JS/bootstrap.min.js"></script>
    <link href="CSS/MainStyle.css" rel="stylesheet" />
    <link href="Content/font-awesome.min.css" rel="stylesheet" />
</body>

   
</html>
