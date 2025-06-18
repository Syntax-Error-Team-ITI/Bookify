$(document).ready(function () {
    RenderSubscribers(1, 10, "");
});

function RenderSubscribers(page = 1, recordsNum = 10, search = "") {
   $.ajax({
        url: "/Subscribers/GetSubscribers",
        type: "GET",
        data: {
            page: page,
            recordsNum: recordsNum,
            search: search.trim()
        },
        success: function (result) {
            $('#subscribersTable').html(result);
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });

}
