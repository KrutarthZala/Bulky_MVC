// Declare Variable
let dataTable;

$(document).ready(function () {
    // Call loadDataTable to initialize DataTable
    loadDataTable();
});

function loadDataTable() {

    // Initialize DataTable with proper AJAX URL and column definitions
    dataTable = $('#dataTableProduct').DataTable({
        "ajax": {
            url: '/Admin/Product/GetAllProducts',
            type : "GET",
            dataType: "json", // Specify the data type of the response
            contentType: "application/json" // Specify the content type of the request
        },
        "columns": [
            { data: 'productTitle', "width":"15%" },
            { data: 'productISBN', "width": "15%" },
            { data: 'productAuthor', "width": "15%" },
            { data: 'productListPrice', "width": "10%" },
            { data: 'category.categoryName', "width": "10%" },
            {
                data: 'productID',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/Product/UpsertProduct?productID=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=DeleteProduct('/Admin/Product/DeleteProduct?productID=${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

// Code of Sweet Alert 
function DeleteProduct(productURL) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: productURL,
                type: 'DELETE',
                success: function () {
                    dataTable.ajax.reload();
                }
            })
        }
    });
}
