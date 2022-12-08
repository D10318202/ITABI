<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/BackAdminMaster.Master" AutoEventWireup="true" CodeBehind="AccountList_backadmin.aspx.cs" Inherits="TravelProject.BackAdmin.AccountList_backadmin" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <label for="<% = this.txtkeyword.ClientID %>">關鍵字</label>
        <asp:TextBox ID="txtkeyword" runat="server" placeHolder="請輸入搜尋文字" Width="149px"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="搜尋" OnClick="btnSearch_Click" />
    </div>
<%--    <asp:Button ID="btnDelete" runat="server" Text="刪除" OnClick="btnDelete_Click" /><br />--%>

    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    
                    <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("UserID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UserID" HeaderText="代碼" />
            <asp:BoundField DataField="Account" HeaderText="帳號" />
            <%--            <asp:BoundField DataField="UserLevel" HeaderText="會員等級" />--%>
        </Columns>
    </asp:GridView>
    <script src="../js/Searchkeyword.js"></script>
    
</asp:Content>