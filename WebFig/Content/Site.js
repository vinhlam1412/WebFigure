////$(document).ready(function () {
////    $("#btnShow").mousedown(function () {
////        $("#password").attr("type", "text");
////    });

////    $("btnShow").on("mouseenter", function () {
////        $("#password").attr("type", "password");
////    });
////});
$("#showHidePassword").click(function () {
    if ($(this).val() == "Show") {
        $(this).val("Hide");
        $("#password").attr("type", "text");
    } else {
        $(this).val("Show");
        $("#password").attr("type", "password");
    }
});