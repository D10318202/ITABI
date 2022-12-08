<%@ Page Title="" Language="C#" MasterPageFile="~/FrountMain.Master" AutoEventWireup="true" CodeBehind="UserInfoEdit.aspx.cs" Inherits="TravelProject.UserInfoEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <h2>個人資料編輯</h2>
        <div class="col-lg-4 col-md-6 col-sm-12">
            <asp:HiddenField ID="hfProfileImg" runat="server" />
            頭像：<asp:FileUpload ID="FuProfileImg" runat="server" type="file" accept="image/*" onchange="loadFile(event)" /><br />
            <div class="cover square" style="width: 20vw">
                <img id="opProfileImg" class="ownerProfileImg" src='<%=this.hfProfileImg.Value %>' />
                <br />
            </div>
        </div>
        <div class="col-lg-8 col-md-6 col-sm-12">
            帳號：<asp:TextBox ID="txtAccount" runat="server"></asp:TextBox>
            <asp:Label ID="lblAccMsg" runat="server" ForeColor="Red"></asp:Label>
            <button class="btn btn-danger" runat="server" type="button" data-bs-toggle="modal" data-bs-target="#deactivateModal">停用我的帳號</button><br />
            密碼：<asp:TextBox ID="txtPWD" runat="server" TextMode="Password"></asp:TextBox>
            (若無需變更密碼，請留空)<br />
            確認新密碼：<asp:TextBox ID="txtChkPWD" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Label ID="lblPWDMsg" runat="server" ForeColor="Red"></asp:Label><br />
            信箱:<asp:Literal ID="ltlEmail" runat="server"></asp:Literal><br />
            暱稱:<asp:TextBox ID="txtNickname" runat="server"></asp:TextBox><br />
            個人簡介：<br />
            <asp:TextBox ID="txtProfile" runat="server" TextMode="MultiLine"></asp:TextBox><br />

            <br />
            <asp:Button CssClass="btn btn-itabi" ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" />
        </div>

        <div class="modal" id="deactivateModal" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">
                            <asp:Literal ID="ltlModalTitle" runat="server">停用帳號確認</asp:Literal></h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Literal ID="ltlModalContent" runat="server">
                            很遺憾您即將離開我們。<br />
                            30天內重新登入可復原您的帳號。
                            </asp:Literal>
                        </p>
                        <label for="exampleFormControl1" class="form-label">離開</label>
                        <select id="SelectReason" class="form-select" aria-label="Default select example">
                            <option selected="selected" value="0">請選取原因</option>
                            <option value="1">我只想暫時離開一陣子</option>
                            <option value="2">我有隱私上的考量</option>
                            <option value="3">我不喜歡這裡</option>
                            <option value="4">其他</option>
                        </select>
                        <label for="deactivateFormControlTextarea1" class="form-label">意見欄</label>
                        <textarea class="form-control" name="txtContent" id="deactivateFormControlTextarea1" placeholder="有任何意見都請不吝指教" rows="3"></textarea>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                        <%--<asp:Button ID="btnDeact" runat="server" class="btn btn-primary" Text="送出" />--%>
                        <button type="button" id="btnDeact" class="btn btn-primary">送出</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>

        var loadFile = function (event) {
            var output = document.getElementById('opProfileImg');
            output.src = URL.createObjectURL(event.target.files[0]);
            output.onload = function () {
                URL.revokeObjectURL(output.src) // free memory
            }
        };

        $(document).ready(function () {
            $("#btnDeact").click(function () {
                var container = $("#deactivateModal");
                if ($('#SelectReason option:selected').val() == "0") {
                    alert('請選取停用原因');
                    return;
                }
                var reportReason = {
                    "Reason": $('#SelectReason option:selected').val(),
                    "Content": $("textarea[name=txtContent]", container).val()
                };
                $.ajax({
                    url: "/API/DeactivateAccountHandler.ashx?Action=Send",
                    method: "POST",
                    data: reportReason,
                    success: function (txtMsg) {
                        console.log(txtMsg);
                        if (txtMsg == "OK") {
                            var modalToggle = document.getElementById('deactivateModal');
                            var aaa = bootstrap.Modal.getInstance(modalToggle);
                            aaa.hide();
                            location.replace('UserInfoEdit.aspx');
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
        });




    </script>

</asp:Content>
