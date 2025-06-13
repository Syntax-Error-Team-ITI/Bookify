$(document).ready(function () {
    RenderTable(1, 10, " ");
});

function RenderTable(page = 1, recordsNum = 10, search = undefined) {
    // Get search value if not provided
    if (search === undefined) {
        search = $('#search').val();
    }
    console.log(search);
    // Update active page button
    $(".page-link").removeClass("active text-white");
    $("#page" + page).addClass("active text-white");

    $.ajax({
        url: "/Books/GetBooks",
        type: "GET",
        data: {
            page: page,
            recordsNum: recordsNum,
            search: search.trim() || ""
        },
        success: function (result) {
            $('#bookTable').html(result);
        },
        error: function (xhr, status, error) {
            console.error("Error loading table:", error);
        }
    });
}
