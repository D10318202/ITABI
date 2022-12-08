﻿$(document).ready(function () {
    $("#" + initObj.btnSearchID).click(function () {
        var val = $("#" + initObj.txtSearchID).val();

        if (val.length == 0) {
            alert('尚未輸入值');
            return false;
        }
        else
            return true;
    })
});