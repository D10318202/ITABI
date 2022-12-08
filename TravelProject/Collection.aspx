<%@ Page Title="" Language="C#" MasterPageFile="~/FrountMain.Master" AutoEventWireup="true" CodeBehind="Collection.aspx.cs" Inherits="TravelProject.Collection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>我的收藏</h2>
   <%--<asp:Repeater ID="rptCollection" runat="server">
        <ItemTemplate>
            <p>
                <a href="ViewArticle.aspx?Post=<%# Eval("ArticleID")  %>">
                    <%# Eval("District")  %>
                </a>
            </p>
        </ItemTemplate>
    </asp:Repeater>--%>

   
    <div class="row">        
        <asp:Repeater ID="rptCollectionCover" runat="server"><%--Repeater：單篇文章的圖片與連結--%>
            <ItemTemplate>
                <div class="cover col-lg-4 col-md-6 col-sm-12"><%--RWD：3/2/1個／每排--%>     
                    <img typeof="botton" runat="server" class="img-thumbnail" src='<%# Eval("PicturePath")%>'>
                    <a href="ViewArticle.aspx?Post=<%# Eval("ArticleID")%>">
                        <div class="mask d-flex align-items-center justify-content-center"><%--icon垂直置中--%>
                            <h3><i class="fa fa-heart" aria-hidden="true"></i><%# Eval("LikeCount")%></h3>
                            <h3><i class="fa fa-comment" aria-hidden="true"></i><%# Eval("CommentCount")%></h3>
                        </div>
                    </a>                         
                </div>                    
            </ItemTemplate>
        </asp:Repeater>
    </div>


</asp:Content>
