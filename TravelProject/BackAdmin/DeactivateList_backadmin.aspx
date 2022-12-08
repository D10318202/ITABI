<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/BackAdminMaster.Master" AutoEventWireup="true" CodeBehind="DeactivateList_backadmin.aspx.cs" Inherits="TravelProject.BackAdmin.DeactivateList_backadmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    
                    <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("UserID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UserID" HeaderText="代碼" />
            <asp:BoundField DataField="ApplicationDate" HeaderText="申請時間" />
            <asp:BoundField DataField="Reason" HeaderText="停用原因" />
            <asp:BoundField DataField="DeactContent" HeaderText="停用內容" />
        </Columns>
    </asp:GridView>
    <asp:PlaceHolder runat="server" ID="plcEmpty" Visible="false">
        <p>尚未有資料</p>
    </asp:PlaceHolder>
</asp:Content>