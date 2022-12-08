<%@ Page Title="" Language="C#" MasterPageFile="~/FrountMain.Master" AutoEventWireup="true" CodeBehind="MessageList.aspx.cs" Inherits="TravelProject.MessageList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-12">

            <ul>
                <br />
                <asp:Repeater ID="rptNotify" runat="server">
                    <ItemTemplate>
                        <a class="dropdown-item" href="<%#Eval("Url") %>">
                            <div class="cover square col-1">
                                <asp:Image  ID="imgFollowingList" CssClass="profileImg" runat="server" ImageUrl='<%#Eval("ProfileImg") %>' />
                            </div>
                            <div class="col-6">
                                <%#Eval("ActAccount") %>  <%#Eval("NotifyContent") %><br />
                                <span id="notifyDate"><%#Eval("CreateTime") %></span>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>

            </ul>
        </div>
    </div>
</asp:Content>
