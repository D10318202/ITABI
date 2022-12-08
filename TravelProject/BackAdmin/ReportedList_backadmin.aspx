<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/BackAdminMaster.Master" AutoEventWireup="true" CodeBehind="ReportedList_backadmin.aspx.cs" Inherits="TravelProject.BackAdmin.ReportedList_backadmin" %>

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
            <asp:BoundField DataField="ReportedID" HeaderText="檢舉代號" />
            <asp:BoundField DataField="ReportDate" HeaderText="檢舉時間" />
            <asp:BoundField DataField="ReportType" HeaderText="檢舉種類" />
            <asp:BoundField DataField="Reason" HeaderText="檢舉原因" />
            <asp:BoundField DataField="ReasonContent" HeaderText="檢舉內容" />
        </Columns>
    </asp:GridView>
    <asp:PlaceHolder ID="plcEmpty" runat="server">
        <p>尚未有資料</p>
    </asp:PlaceHolder>
</asp:Content>