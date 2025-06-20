$(document).ready(function () {
    console.log(isAdmin);
    if (typeof isAdmin === 'undefined') {
        isAdmin = false;
        //var currentUser = {
        //    isAuthenticated: false,
        //    isAdmin: false,
        //    isManager: false
        //};
    }
    console.log(isAdmin);
    $('#booksTable').DataTable({  // Make sure this matches your table ID
        serverSide: true,
        processing: true,
        stateSave: true,
        language: {
            processing: '<div class="d-flex justify-content-center text-primary align-items-center dt-spinner"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div><span class="text-muted ps-2">Loading...</span></div>'
        },
        ajax: {
            url: '/Books/GetBooks',
            type: 'POST',
            dataType: 'json'
        },
        columns: [
            {
                "data": "id",
                "name": "Id",
                "className": "d-none",
                "visible": false,
                "searchable": false
            },
            {
                "data": null,
                "name": "Title",
                "className": "align-middle p-0",
                "render": function (data, type, row) {
                    return `<tr>
                              <td>
                                <div class="d-flex px-2 py-1">
                                  <div>
                                    <img src="${row.imageThumbnailUrl || '/images/no-book.jpg'}" class="avatar avatar-sm me-3">
                                  </div>
                                  <div class="d-flex flex-column justify-content-center">
                                    <h2 class="mb-0 text-sm"><a id="link" href="/Books/Details/${row.id}" class="fw-bold text-decoration-none text-dark text-truncate d-block" style="max-width: 120px !important;">
                                        ${row.title}
                                    </a></h2>
                                    <p class="text-sm text-secondary mb-0 text-truncate" style="max-width: 120px !important;">${row.author}</p>
                                  </div>
                                </div>
                              </td>
                            `;
                }
            },
            {
                "data": "publisher",
                "name": "Publisher",
                "className": "",
                "render": function (data, type, row) {
                    return `<p class="mb-0 text-sm text-truncate" style="max-width: 120px !important;"> ${row.publisher}</p>`;
                }
            },
            {
                "data": "hall", "name": "Hall",
                "render": function (data, type, row) {
                    return `<p class="mb-0 text-sm text-truncate" style="max-width: 120px !important;"> ${row.hall}</p>`;

                }
            },
            {
                "data": "isDeleted",
                "name": "IsDeleted",
                "render": function (data, type, row) {
                    return ` <span id="status${row.id}" class="badge bg-${((row.isDeleted) ? "danger" : "success")}">
                               ${((row.isDeleted) ? "Deleted" : "Available")}
                            </span>`;
                }
            },
            {
                "data": "isAvailableForRental",  
                "name": "IsAvailableForRental",
                "render": function (data, type, row) {
                    return `<span class="badge bg-${row.isAvailableForRental ? 'success' : 'danger'}">
                                ${row.isAvailableForRental ? 'Available' : 'Rented'}
                            </span>`;
                }
            },
            {
                "data": null,
                "className": 'text-end',
                "orderable": false,
                "render": function (data, type, row) {
                    return `<div class="btn-group ${isAdmin?'':'d-none'}">
                                <button type="button" class="btn btn-sm btn-primary rounded-3 m-0" data-bs-toggle="dropdown" aria-expanded="false">
                                   Options
                                    <span class="svg-icon svg-icon-5 m-0">
                                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M11.4343 12.7344L7.25 8.55005C6.83579 8.13583 6.16421 8.13584 5.75 8.55005C5.33579 8.96426 5.33579 9.63583 5.75 10.05L11.2929 15.5929C11.6834 15.9835 12.3166 15.9835 12.7071 15.5929L18.25 10.05C18.6642 9.63584 18.6642 8.96426 18.25 8.55005C17.8358 8.13584 17.1642 8.13584 16.75 8.55005L12.5657 12.7344C12.2533 13.0468 11.7467 13.0468 11.4343 12.7344Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                </button>
                                <ul class="dropdown-menu dropdown-menu-end p-0">
                                    <li><a class="btn btn-info m-0 fw-bolder w-100 h-100 rounded-0 border-0 " href ="/Books/Edit/${row.id}">Edit</a></li>
                                    <li><a class="btn btn-dark m-0 fw-bolder w-100 h-100 rounded-0 border-0 " href ="javascript:;" onclick= "confirmChange(${row.id})">Change Status</a></li>
                                </ul>
                            </div>`;
                }
            }
        ]
    });

});

function confirmChange(id) {
    // Show a confirmation dialog
    var flag = false;
    bootbox.confirm({
        message: 'Are you sure you want to change the status of this book?',
        buttons: {
            confirm: {
                label: 'Yes, Toggle',
                className: 'btn-warning'
            },
            cancel: {
                label: 'No, Cancel',
                className: 'btn-secondary'
            }
        },
        callback: function (result) {
            if (result)
                toggleStatus(id);
        }
    });   
}
function toggleStatus(id) {

    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Books/ToggleStatus',
        type: 'POST',
        data: { id: id },
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.success) {

                var status = $(`#status${id}`);
                status.toggleClass("bg-danger");
                status.toggleClass("bg-success");

                // Refresh the DataTable
                $('#booksTable').DataTable().ajax.reload(null, false);
            } else {

            }
        },
        error: function (xhr, status, error) {
            toastr.error('An error occurred while updating the status: ' + error);
        }
    });
}






//`< div class="card border-0" >
//                        <div class="row g-0 align-items-center">
//                            <div class="col-2 col-md-2">
//                                <img src="${row.imageThumbnailUrl || '/images/books/no-book.jpg'}" class='img-fluid rounded-circle object-fit-cover'>
//                            </div>
//                            <div class="col-10 col-md-10">
//                                <div class="card-body p-0 ps-2">
//                                    <a href="/Books/Details/${row.id}" class="fw-bold text-decoration-none text-dark text-truncate d-block" style="max-width: 120px !important;">
//                                        ${row.title}
//                                    </a>
//                                    <p class="text-muted small mb-0 text-truncate d-block" style="max-width: 120px !important;">${row.author}</p>
//                                </div>
//                            </div>
//                        </div>
//                            </div >


//{
//    "name": "Title",
//        "className": "",
//            "render": function (data, type, row) {
//                return ``;
//            },
//}

//$(document).ready(function () {
//    RenderTable(1, 10, " ");
//});

//function RenderTable(page = 1, recordsNum = 10, search = undefined) {
//    // Get search value if not provided
//    if (search === undefined) {
//        search = $('#search').val();
//    }
//    console.log(search);
//    // Update active page button
//    $(".page-link").removeClass("active text-white");
//    $("#page" + page).addClass("active text-white");


//    $('#Books').DataTable({
//        "processing": true,
//        "serverSide": true,
//        "filter": true,
//        "ajax": {
//            "url": "/Books/GetBooks",
//            "type": "POST",
//            "datatype": "json"
//        },
//        success: function (result) {
//                    console.log(result)
//        },
//        error: function (xhr, status, error) {
//                    console.error("Error loading table:", error);
//                },

//        "columns": [
//            //{ "data": "Id", "name": "Id", "autoWidth": true },
//            { "data": "Title", "name": "Title", "autoWidth": true },
//            { "data": "Publisher", "name": "Publisher", "autoWidth": true },
//            { "data": "Hall", "name": "Hall", "autoWidth": true },
//            { "data": "Status", "name": "Status", "autoWidth": true },
//        ],
//        "paging": true,
//        "pageLength": 10,
//        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
//    });

//}
