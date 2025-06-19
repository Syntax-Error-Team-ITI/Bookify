$(document).ready(function () {
    RenderTable(1, 10, "");
});

function RenderTable(page = 1, recordsNum = 10, search = "") {
    $(".page-link").removeClass("active text-white");
    $("#page" + page).addClass("active text-white");

    $.ajax({
        url: "/Subscribers/GetSubscribers",
        type: "GET",
        data: {
            page: page,
            recordsNum: recordsNum,
            search: search.trim()
        },
        success: function (result) {
            $('#subscriberTable').html(result);
        },
        error: function (xhr, status, error) {
            console.error("Error loading table:", error);
        }
    });
}