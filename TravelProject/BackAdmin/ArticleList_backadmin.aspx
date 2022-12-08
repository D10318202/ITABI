<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/BackAdminMaster.Master" AutoEventWireup="true" CodeBehind="ArticleList_backadmin.aspx.cs" Inherits="TravelProject.BackAdmin.ArticleList_backadmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <label for="<% = this.txtkeyword.ClientID %>">關鍵字</label>
        <asp:TextBox ID="txtkeyword" runat="server" placeHolder="請輸入搜尋文字" Width="149px"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="搜尋" OnClick="btnSearch_Click" />
    </div>
<%--    <asp:Button runat="server" ID="btnDelete" Text="刪除" OnClick="btnDelete_Click" /><br />--%>

    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    
                    <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("ArticleID")%>' />

                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UserID" HeaderText="會員代號" />
            <asp:BoundField DataField="CreateTime" HeaderText="發佈日期" />
            <asp:BoundField DataField="District" HeaderText="地點" />
            <asp:BoundField DataField="ArticleContent" HeaderText="文章內文" />
        </Columns>
    </asp:GridView>
    <asp:PlaceHolder runat="server" ID="plcEmpty" Visible="false">
        <p>尚未有資料</p>
    </asp:PlaceHolder>
    <script src="../js/Searchkeyword.js"></script>
   
</asp:Content>