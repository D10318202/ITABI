<%@ Page Title="" Language="C#" MasterPageFile="~/FrountMain.Master" AutoEventWireup="true" CodeBehind="PostArticle.aspx.cs" Inherits="TravelProject.PostArticle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 350px;
            height: 350px;
        }
        .img {
            max-width: 150px;
            max-height: 150px;
            margin: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
    <h2>發布文章</h2>
        <p>有顯示 (*) 為必填欄位</p>
        <div>
            地點:
       
            <asp:TextBox ID="txtDistrict" runat="server" placeholder="請輸入地點"></asp:TextBox>
            <asp:Button ID="btnSearchDistrict" runat="server" Text="搜尋" OnClick="btnSearchDistrict_Click" /><br />
            <asp:PlaceHolder ID="plcSearch" runat="server" Visible="false">
                <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
                <asp:Button ID="btnDistrictClick" runat="server" Text="確認地點" OnClick="btnDistrictClick_Click" />
                <%--<div id="map" style="width:100px; height:100px;"></div>--%>
        </asp:PlaceHolder>
            <asp:HiddenField ID="hfLat" runat="server" />
            <asp:HiddenField ID="hfLng" runat="server" />
            <asp:HiddenField ID="hfPlaceID" runat="server" />
        </div>
        <div>
            <asp:PlaceHolder ID="plcPic" runat="server">
            *<asp:FileUpload ID="ArticlePicture" runat="server" type="file" accept="image/*" onchange="loadFile(event)" AllowMultiple="true" />
            <asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red" ></asp:Label><br />
            <img id="output" class="auto-style1" src="" /><br />

            </asp:PlaceHolder>
        </div>
        
        <div>
            <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Height="647px" Width="815px">文章內容</asp:TextBox>
        </div>
        <div>
            瀏覽權限:
       
        <asp:DropDownList ID="ddlViewLimit" runat="server">
            <asp:ListItem Text="公開" Value="1" Selected="True"></asp:ListItem>
            <asp:ListItem Text="僅限追蹤者" Value="2"></asp:ListItem>
            <asp:ListItem Text="只限本人" Value="3"></asp:ListItem>
        </asp:DropDownList>
        </div>
        <div>
            留言權限:
       
        <asp:DropDownList ID="ddlCommemtLimit" runat="server">
            <asp:ListItem Text="開放" Value="1" Selected="True"></asp:ListItem>
            <asp:ListItem Text="僅限追蹤者" Value="2"></asp:ListItem>
            <asp:ListItem Text="不開放" Value="3"></asp:ListItem>
        </asp:DropDownList>
        </div>
        <asp:Button ID="btnPost" runat="server" Text="發佈文章" OnClick="btnPost_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="取消發布" OnClick="btnCancel_Click" />
    </div>
    <script
        src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBA9A_Ry9G67EMNHQHYwh3aAE9ubAkaLdU&callback=initMap&v=weekly"
        async></script>
    <script>
        var loadFile = function (event) {
            var output = document.getElementById('output');
            output.src = URL.createObjectURL(event.target.files[0]);
            output.onload = function () {
                URL.revokeObjectURL(output.src) // free memory
            }
        };
    </script>
</asp:Content>