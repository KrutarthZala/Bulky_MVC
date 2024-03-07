// Declare Variable
let dataTable;

$(document).ready(function () {
    $("#displayOrder").click(function (event) {
        event.preventDefault();

        var startOrderDate = $("#startOrderDate").val(); // Replace "startOrderDateInput" with the actual ID of your start date input field
        var endOrderDate = $("#endOrderDate").val();

        if (startOrderDate === "") {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Please enter a Start Date"
            });
        } else if (startOrderDate <= endOrderDate) {
            // Call the function to load data table with the determined order status
            loadDataTable(startOrderDate, endOrderDate);
        }
        else {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Ending date is must greater than Starting Date"
            });
        }
    });
});

function loadDataTable(startOrderDate, endOrderDate) {

    if (dataTable) {
        dataTable.clear().draw();
        dataTable.ajax.url('/Admin/Report/GetOrderReport?startOrderDate=' + startOrderDate + '&endOrderDate=' + endOrderDate).load();
        return;
    }
    // Initialize DataTable with proper AJAX URL and column definitions
    dataTable = $('#dataTableOrderReport').DataTable({
        "ajax": {
            url: '/Admin/Report/GetOrderReport?startOrderDate=' + startOrderDate + '&endOrderDate=' + endOrderDate,
            type: "GET",
            dataType: "json", // Specify the data type of the response
            contentType: "application/json" // Specify the content type of the request
        },
        "columns": [
            { data: 'userName', "width": "20%" },
            { data: 'userPhoneNumber', "width": "20%" },
            { data: 'bulkyBookUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "15%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'orderDate',
                width: '20%',
                render: function (data, type, row) {
                    var date = new Date(data);
                    var day = date.getDate().toString().padStart(2, '0');
                    var month = (date.getMonth() + 1).toString().padStart(2, '0');
                    var year = date.getFullYear();
                    var formattedDate = day + ' - ' + month + ' - ' + year;
                    return formattedDate;
                }
            }
        ]
    });
}