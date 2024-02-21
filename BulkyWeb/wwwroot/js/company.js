// Declare variable.
let dataTable;

$(document).ready(function () {
    // Call loadDataTable to initialize DataTable
    loadDataTable();
});

function loadDataTable() {

    // Initialize DataTable with proper AJAX URL and column definitions
    dataTable = $('#dataTableCompany').DataTable({
        "ajax": {
            url: '/Admin/Company/GetAllCompanies',
            type : "GET",
            dataType: "json", // Specify the data type of the response
            contentType: "application/json" // Specify the content type of the request
        },
        "columns": [
            { data: 'companyName', "width":"15%" },
            { data: 'companyStreetAddress', "width": "15%" },
            { data: 'companyCity', "width": "10 %" },
            { data: 'companyState', "width": "10%" },
            { data: 'companyPostalCode', "width": "12%" },
            { data: 'companyPhoneNumber', "width": "14%" },
            {
                data: 'companyID',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/Company/UpsertCompany?companyID=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=DeleteCompany('/Admin/Company/DeleteCompany?companyID=${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "27%"
            }
        ]
    });
}

// Code of Sweet Alert
function DeleteCompany(companyURL) {
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
                url: companyURL,
                type: 'DELETE',
                success: function () {
                    dataTable.ajax.reload();
                }
            })
        }
    });
}
