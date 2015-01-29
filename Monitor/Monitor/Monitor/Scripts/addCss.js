$(document).ready(function () {
    $(":text").addClass("form-control");
    $("#repairTime").attr("type", "date");
    $("#repairTime_Date").attr("type", "date");
    $("textarea").addClass("form-control");
    $("textarea").attr("rows", "5");
    $("input[type='submit']").addClass("btn btn-default");
});
function checkinput() {
    if (confirm("确认提交？"))
        return ture;
    else
        return false;
} 