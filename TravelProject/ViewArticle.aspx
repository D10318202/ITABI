<%@ Page Title="" Language="C#" MasterPageFile="~/FrountMain.Master" AutoEventWireup="true" CodeBehind="ViewArticle.aspx.cs" Inherits="TravelProject.ViewArticle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row align-items-center">

        <%--照片欄位（border只是為了方便看範圍用的）--%>
        <div class="col-lg-6 col-md-6 col-sm-12">
            <asp:Literal ID="ltlPhoto" runat="server"></asp:Literal>
            <!-- 輪播 -->
            <div id="demo" class="carousel slide" data-bs-ride="carousel" style="width: 100%">

                <!-- 指示符 -->
                <%-- <div class="carousel-indicators">
                    <button type="button" data-bs-target="#demo" data-bs-slide-to="0" class="active"></button>

                    <asp:Repeater ID="rptPicturePage" runat="server">
                        <ItemTemplate>
                            <button type="button" data-bs-target="#demo" data-bs-slide-to="<%#Eval("PictureNumber") %>"></button>
                        </ItemTemplate>
                    </asp:Repeater>

                </div>--%>

                <!-- 圖片 -->
                <div class="carousel-inner" style="width: 100%">

                    <div class="carousel-item active">
                        <img id="imgCover" src="" class="d-block" style="max-height: 80vh; max-width: 100%" runat="server">
                    </div>

                    <asp:Repeater ID="rptPicture" runat="server">
                        <ItemTemplate>
                            <div class="carousel-item">
                                <img src="<%#Eval("PicturePath") %>" class="d-block" style="max-height: 80vh; max-width: 100%">
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <!-- 左右切换按鈕 -->
                <button class="carousel-control-prev" type="button" data-bs-target="#demo" data-bs-slide="prev">
                    <span class="carousel-control-prev-icon"></span>
                </button>
                <button class="carousel-control-next" type="button" data-bs-target="#demo" data-bs-slide="next">
                    <span class="carousel-control-next-icon"></span>
                </button>
            </div>
        </div>


        <%--文字介面（border只是為了方便看範圍用的）--%>
        <div class="col-lg-6 col-md-6 col-sm-12" style="align-self: start">

            <div class="float-end">

            <asp:PlaceHolder ID="plcEdit" runat="server" Visible="false">
                <asp:Button CssClass="btn btn-itabi" ID="btnEdit" runat="server" Text="編輯" OnClick="btnEdit_Click" />
                <asp:Button  ID="btnDelArticle" runat="server" Text="刪除" OnClick="btnDelArticle_Click" CssClass="btnDel btn btn-itabi" />
            </asp:PlaceHolder>
            </div>
            
            <div class="row">
                <%--發布者:--%>
                <div class="col-4">
                    <div class="cover square">
                        <asp:Image CssClass="cover square" ID="ImgUser" runat="server" Style="width: 100%" />
                    </div>
                    <div class="col-8">
                        <asp:Literal ID="ltlUser" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <div style="color: #2a999c">
                <i class="fa fa-map-marker" aria-hidden="true" style="color: #78C2C4"></i>
                <asp:Literal ID="ltlDistrict" runat="server"></asp:Literal>
            </div>
            <br />
            <div>
                <%--內文:--%><asp:Literal ID="ltlArtContent" runat="server"></asp:Literal>
            </div>
            <br />
            <div style="color: #2a999c">
                <%--發布日期:--%>
                <i class="fa fa-clock-o" aria-hidden="true" style="color: #78C2C4"></i>
                <asp:Literal ID="ltlCreateDate" runat="server"></asp:Literal>
            </div>

            <div>
                <asp:LinkButton ForeColor="#FF4346" ID="lbtnLike" runat="server" OnClick="btnLike_Click">讚</asp:LinkButton>
                <asp:Label ID="lblLikeCount" runat="server" Text="Label"></asp:Label>&emsp;   

                <i class="fa fa-commenting" aria-hidden="true" style="color: #78C2C4"></i>
                <asp:Literal ID="ltlCommentCount" runat="server"></asp:Literal>&emsp;


                <asp:LinkButton ForeColor="#78C2C4" ID="lbtnCollect" runat="server" OnClick="btnCollect_Click">收藏</asp:LinkButton>
                <asp:HiddenField ID="hfID2" runat="server" Value="" />

                <linkbutton style="color: #78C2C4" runat="server" id="lbtnReportA" type="button" data-bs-toggle="modal" data-bs-target="#ReportArticleModal"><i class="fa fa-flag" aria-hidden="true"></i></linkbutton>
                <br />
                <br />

                <div>
                    <asp:PlaceHolder ID="plcCanComment" runat="server">
                        <asp:TextBox ID="txtComment" runat="server" Width="400"></asp:TextBox>
                        <asp:Button CssClass="btn btn-itabi" ID="btnComment" runat="server" Text="留言" OnClick="btnComment_Click" /><br />
                    </asp:PlaceHolder>
                    <asp:Literal ID="ltlCommentMsg" runat="server" Visible="false">您沒有權限回覆本貼文。</asp:Literal>

                    <asp:Repeater ID="rptComment" runat="server" OnItemCommand="rptComment_ItemCommand">
                        <ItemTemplate>
                            <div>
                                <a href="UserPage.aspx?User=<%# Eval("Account") %>"><%# Eval("Account") %></a> : <%# Eval("CommentContent") %>
                                <br />
                                <p style="color: #2a999c; text-align: right">
                                    <i class="fa fa-clock-o" aria-hidden="true" style="color: #78C2C4"></i>
                                    <%--發布時間--%><%# Eval("CreateTime") %>
                                    &emsp;
                                <%--<div style="color: #78C2C4; float: right">--%>
                                    <%--<asp:Button runat="server" Text="回覆" CommandArgument='<%# Eval("CommentID") %>' CommandName="ReButton" />--%>
                                    <asp:LinkButton ForeColor="#78C2C4" runat="server" CommandArgument='<%# Eval("CommentID") %>' CommandName="ReButton"><i class="fa fa-reply"  aria-hidden="true"></i></asp:LinkButton>
                                    <%--<asp:Button runat="server" Text="刪除" Visible="false" CommandArgument='<%# Eval("CommentID") %>' CommandName="DeleteButton" CssClass="btnDel" ID="rptCmtDel" />--%>
                                    <asp:LinkButton ForeColor="#78C2C4" runat="server" Visible="false" CommandArgument='<%# Eval("CommentID") %>' CommandName="DeleteButton" CssClass="btnDel" ID="rptCmtDel"><i class="fa fa-trash" aria-hidden="true"></i></asp:LinkButton>

                                    <input type="hidden" id="hfID" value='<%# Eval("CommentID") %>' />
                                    <%--<button runat="server" id="btnReportComment" type="button" data-bs-toggle="modal" data-bs-target="#ReportCommentModal">檢舉</button>--%>
                                    <linkbutton runat="server" id="btnReportComment" type="button" data-bs-toggle="modal" data-bs-target="#ReportCommentModal"><i class="fa fa-flag" aria-hidden="true" style="color: #78C2C4"></i></linkbutton>
                                    <%--</div>--%>
                                </p>
                            </div>

                        </ItemTemplate>
                        <SeparatorTemplate>
                            <hr />
                        </SeparatorTemplate>
                    </asp:Repeater>
                </div>
            </div>

        </div>

    </div>




    <div class="modal" id="ReportArticleModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <asp:Literal ID="Literal1" runat="server">檢舉</asp:Literal></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal ID="Literal2" runat="server">您確定要檢舉嗎？<br />若不想檢舉，請點擊關閉。</asp:Literal>
                    </p>

                    <div class="mb-3">

                        <label for="exampleFormControlInput1" class="form-label">檢舉理由</label>

                        <select id="SelectReasonA" name="SelectReasonA" cssclass="form-control">
                            <option value="1" selected="selected">請選擇</option>
                            <option value="2">我認為這冒犯了我</option>
                            <option value="3">我的著作權被侵犯了</option>
                            <option value="4">這個內容有違法行為</option>
                            <option value="5">其他</option>
                        </select>

                    </div>
                    <div class="mb-3">
                        <label for="ReportArticleModalTextarea" class="form-label">附加說明</label>
                        <textarea class="form-control" name="txtContent" id="ReportArticleModalTextarea" rows="3" placeholder="請詳細說明檢舉理由"></textarea>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                    <button type="button" id="btnSendReportofA" class="btn btn-primary">送出</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal" id="ReportCommentModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <asp:Literal ID="ltlModalTitle" runat="server">檢舉</asp:Literal></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal ID="ltlModalContent" runat="server">您確定要檢舉嗎？<br />若不想檢舉，請點擊關閉。</asp:Literal>
                    </p>

                    <div class="mb-3">

                        <label for="exampleFormControlInput1" class="form-label">檢舉理由</label>

                        <select id="SelectReason" name="SelectReason" cssclass="form-control">
                            <option value="1" selected="selected">請選擇</option>
                            <option value="2">我認為這冒犯了我</option>
                            <option value="3">我的著作權被侵犯了</option>
                            <option value="4">這個內容有違法行為</option>
                            <option value="5">其他</option>
                        </select>

                    </div>
                    <div class="mb-3">
                        <label for="exampleFormControlTextarea1" class="form-label">附加說明</label>
                        <textarea class="form-control" name="txtContent" id="exampleFormControlTextarea1" rows="3" placeholder="請詳細說明檢舉理由"></textarea>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                    <button type="button" id="btnSendReport" class="btn btn-primary">送出</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            $("#btnSendReportofA").click(function () {
                var container = $("#ReportArticleModal");
                if ($('#SelectReasonA option:selected').val() == "1") {
                    alert('請選擇檢舉理由');
                    return;
                }
                var reportReason = {
                    "ID": $("#<%= this.hfID2.ClientID %>").val(),
                    "ReportType": "Article",
                    "Reason": $('#SelectReasonA option:selected').val(),
                    "Content": $("textarea[name=txtContent]", container).val()
                };
                $.ajax({
                    url: "/API/ReportCommentHandler.ashx?Action=SendA",
                    method: "POST",
                    data: reportReason,
                    success: function (txtMsg) {
                        console.log(txtMsg);
                        if (txtMsg == "OK") {
                            alert("謝謝您，已收到您的檢舉。");
                            sessionStorage.clear();
                            history.go(0);
                        }
                        else {
                            alert("檢舉失敗，請聯絡管理員。");
                        }
                    },
                    error: function (msg) {
                        console.log(msg);
                        alert("通訊失敗，請聯絡管理員。");
                    }
                });
            });
            $("#btnSendReport").click(function () {
                var container = $("#ReportCommentModal");
                if ($('#SelectReason option:selected').val() == "1") {
                    alert('請選擇檢舉理由');
                    return;
                }
                var parentDiv = $(this).closest("div");
                var hf = parentDiv.find("input.hfID");
                var reportReason = {
                    "ID": $("#hfID").val(),
                    "ReportType": "Comment",
                    "Reason": $('#SelectReason option:selected').val(),
                    "Content": $("textarea[name=txtContent]", container).val()
                };
                $.ajax({
                    url: "/API/ReportCommentHandler.ashx?Action=SendC",
                    method: "POST",
                    data: reportReason,
                    //dataType: "JSON",
                    success: function (txtMsg) {
                        console.log(txtMsg);
                        if (txtMsg == "OK") {
                            alert("謝謝您，已收到您的檢舉。");
                            history.go(0);
                        }
                        else {
                            alert("檢舉失敗，請聯絡管理員。");
                        }

                    },
                    error: function (msg) {
                        console.log(msg);
                        alert("通訊失敗，請聯絡管理員。");
                    }
                });
            });
        })
    </script>




</asp:Content>