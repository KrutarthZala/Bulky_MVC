// Declare Variable
let dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess")
    }
    else {
        if (url.includes("pending")) {
            loadDataTable("pending")
        }
        else {
            if (url.includes("completed")) {
                loadDataTable("completed")
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved")
                }
                else {
                    loadDataTable("all")
                }
            }
        }
    }
});

function loadDataTable(orderStatus) {

    // Initialize DataTable with proper AJAX URL and column definitions
    dataTable = $('#dataTableOrder').DataTable({
        "ajax": {
            url: '/Admin/Order/GetAllOrders?orderStatus=' + orderStatus,
            type : "GET",
            dataType: "json", // Specify the data type of the response
            contentType: "application/json" // Specify the content type of the request
        },
        "columns": [
            { data: 'orderHeaderID', "width":"5%" },
            { data: 'userName', "width": "20%" },
            { data: 'userPhoneNumber', "width": "15%" },
            { data: 'bulkyBookUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "15%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'orderHeaderID',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/Order/OrderDetails?orderID=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                "width": "10%"
            }
        ]
    });
}

// Code of Sweet Alert 
//function DeleteProduct(productURL) {
//    Swal.fire({
//        title: "Are you sure?",
//        text: "You won't be able to revert this!",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Yes, delete it!"
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: productURL,
//                type: 'DELETE',
//                success: function () {
//                    dataTable.ajax.reload();
//                }
//            })
//        }
//    });
//}
