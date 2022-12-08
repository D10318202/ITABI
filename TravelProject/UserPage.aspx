<%@ Page Title="" Language="C#" MasterPageFile="~/FrountMain.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="TravelProject.MyPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .follow {
            position: fixed;
            right: 30px;
            top: 30px;
            width: 250px;
            height: 300px;
            border: 1px solid #000000;
        }

        table {
            width: 100%;
        }




        tr {
            vertical-align: central;
        }

        #map {
            height: 500px;
        }

        html, body {
            height: 100%;
            margin: 0;
            padding: 0;
        }
    </style>

    <script src="https://polyfill.io/v3/polyfill.min.js?features=default"></script>
    <link rel="stylesheet" type="text/css" href="./style.css" />
    <script src="./index.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row align-items-center">
        <%--頭像--%>
        <div class="cover square col-3">
            <a runat="server" id="aLinkPic" title="" target="_blank">
                <img runat="server" id="PageOwnerimg" class="ownerProfileImg"/></a>
        </div>
        <div class="col-9">
        <%--暱稱與個人簡介--%>
        <asp:HiddenField ID="hfUserAccount" runat="server" />
        <h3><asp:Literal ID="ltlUserAcc" runat="server"></asp:Literal></h3><br />
            文章:<asp:Literal ID="ltlArticleCount" runat="server"></asp:Literal>|
             追蹤中:<asp:LinkButton ID="lkbFollowingCount" runat="server" data-bs-toggle="modal" data-bs-target="#FollowingModal"></asp:LinkButton>|
            粉絲:
        <asp:LinkButton ID="lkbFansCount" runat="server" data-bs-toggle="modal" data-bs-target="#FansModal"></asp:LinkButton>
        <br />
                <asp:Literal ID="ltlNickName" runat="server"></asp:Literal><br />
        <asp:Literal ID="ltlProfileContent" runat="server"></asp:Literal><br />


             <asp:PlaceHolder ID="plcPageOwner" runat="server" Visible="false">
        <asp:Button CssClass="btn btn-itabi backgroundcolor-itabiB" ID="btnChange" runat="server" Text="修改個人資料" OnClick="btnChange_Click" />
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcGuest" runat="server" Visible="true">
        <asp:Button CssClass="btn btn-itabi" ID="btnFollow" runat="server" Text="追蹤" OnClick="btnFollow_Click" />
    </asp:PlaceHolder>

    </div>
        </div>

    <hr />

    <asp:LinkButton ID="lkbShowArticle" runat="server" OnClick="lkbShowArticle_Click" Enabled="false">文章一覽</asp:LinkButton>|
    <asp:LinkButton ID="lkbShowMap" runat="server" OnClick="lkbShowMap_Click">足跡一覽</asp:LinkButton><br />
    <asp:PlaceHolder ID="plcShowArticle" runat="server" Visible="true">
        <%--<asp:Repeater ID="rptUserPage" runat="server">
            <ItemTemplate>
                <p>
                    <a href="ViewArticle.aspx?Post=<%# Eval("ArticleID")  %>">
                        <%# Eval("PicturePath")  %>
                    </a>
                </p>
            </ItemTemplate>
        </asp:Repeater>--%>

         <div class="row">        
        <asp:Repeater ID="rptUserPage" runat="server"><%--Repeater：單篇文章的圖片與連結--%>
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
    </asp:PlaceHolder>


    <asp:PlaceHolder ID="plcShowMap" runat="server" Visible="false">
        <div id="map"></div>
    </asp:PlaceHolder>



   







   



    <div class="modal fade" id="FollowingModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">追蹤中</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <table>
                        <asp:Repeater ID="rptFollowing" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="50px">
                                        <div class="cover">
                                            <asp:Image ID="imgFollowingList" CssClass="profileImg" runat="server" ImageUrl='<%#Eval("ProfileImg") %>' />
                                        </div>
                                    </td>
                                    <td >
                                        　<a href="UserPage.aspx?User=<%#Eval("Account") %>">
                                            <%#Eval("Account") %>
                                        </a>
                                    </td>
                                    <td width="100px">
                                        <input type="hidden" id="hfAcc" class="hfUserID" runat="server" value='<%#Eval("Account") %>' />
                                        <input type="button" id="btnListFollow" class="btn btn-primary" runat="server" value='追蹤' onclick="this.disabled='disable';this.value='已追蹤'" />
                                        <input type="button" id="btnListUnFollow" class="btn btn-secondary" runat="server" value='取消追蹤' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="FansModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="FansModalModalLabel">粉絲</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <table>
                        <asp:Repeater ID="rptFans" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="50px">
                                        <div class="cover">
                                            <asp:Image ID="imgFansList" CssClass="profileImg" runat="server" ImageUrl='<%#Eval("ProfileImg") %>' />
                                        </div>
                                    </td>
                                    <td>
                                        　<a href="UserPage.aspx?User=<%#Eval("Account") %>">
                                            <%#Eval("Account") %>
                                        </a>
                                    </td>
                                    <td width="100px">
                                        <input type="hidden" id="hfAcc" class="hfUserID" runat="server" value='<%#Eval("Account") %>' />
                                        <input type="button" id="btnListFollow" class="btn btn-primary" runat="server" value='追蹤' onclick="this.disabled='disable';this.value='已追蹤'" />
                                        <input type="button" id="btnListUnFollow" class="btn btn-secondary" runat="server" value='取消追蹤' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
    </div>


    <script
        src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBpKXjseZ3apfLl3iAPkwXHUWUcTGj1Hqg&callback=initMap&v=weekly"
        async></script>

    <script>
        $(document).ready(function () {
            $("input[id*=btnListFollow]").click(function () {
                var parentTr = $(this).closest("tr");
                var hf = parentTr.find("input.hfUserID");
                var postData = {
                    "UserAccount": hf.val()
                };
                $.ajax({
                    url: "/API/FollowHandler.ashx?Action=Follow",
                    method: "POST",
                    data: postData,
                    success: function (txtMsg) {
                        console.log(txtMsg);
                        if (txtMsg == "success follow") {
                        }
                    },
                    error: function (msg) {
                        console.log(msg);
                        alert("通訊失敗，請聯絡管理員。");
                    }
                });
            });
            $("input[id*=btnListUnFollow]").click(function () {
                var parentTr = $(this).closest("tr");
                var hf = parentTr.find("input.hfUserID");
                var postData = {
                    "UserAccount": hf.val()
                };
                console.log(postData, this);
                if (confirm("確定取消追蹤?")) {
                    this.value = '已退追';
                    this.disabled = 'disable';
                }
                else
                    return false;
                $.ajax({
                    url: "/API/FollowHandler.ashx?Action=UnFollow",
                    method: "POST",
                    data: postData,
                    success: function (txtMsg) {
                        console.log(txtMsg);
                        if (txtMsg == "success unfollow") {
                        }
                    },
                    error: function (msg) {
                        console.log(msg);
                        alert("通訊失敗，請聯絡管理員。");
                    }
                });
            });
        });
        let map;
        function initMap() {
            const myLatLng = { lat: 25.0586058, lng: 121.5320456 };
            map = new google.maps.Map(document.getElementById("map"), {
                center: myLatLng,
                zoom: 3,
            });
        }
        var markerArr = [];
        var infoArr = [];
        function getData() {
            console.log("data gocha");
            var UserAccount = $("input[id*=hfUserAccount]").val();
            $.ajax({
                url: `/API/MapTagHandler.ashx?User=${UserAccount}`,
                dataType: "json",
                success: function (articleList) {
                    console.log(articleList);
                    for (var item of articleList) {
                        marker = new google.maps.Marker({
                            position: {
                                lat: parseFloat(item.Latitude),
                                lng: parseFloat(item.Longitude)
                            },
                            map,
                            title: item.DistrictName,
                            icon: "https://emos.plurk.com/f9689f2e904929fbd98bc1ba092c2dd5_w46_h30.gif"
                        });
                        const contentString = `<a href="/ViewArticle.aspx?Post=${item.ArticleID}">${item.DistrictName}</a>`;
                        const infowindow = new google.maps.InfoWindow({
                            content: contentString
                        });
                        //marker.idx = i;
                        marker.addListener("click", function () {
                            infowindow.open({
                                anchor: this,
                                map,
                                shouldFocus: true
                            });
                        });
                        // marker.addListener("mouseout", () => {
                        //     infowindow.close();
                        // });
                        markerArr.push(marker);
                        infoArr.push(infowindow);
                    }
                    console.log(markerArr);
                    console.log(infoArr);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }
        window.onload = function () {
            getData();
        };
    </script>
</asp:Content>
