var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    //console.log(url);
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
  
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll?status="+status,
            "type": "GET",
            "datatype": "json",
            "error": function (xhr, status, error) {
                //console.error("Error fetching data:", error);
            }
        },
        "columns": [
            { "data": 'id', "width": '5%' },
            { "data": 'name', "width": '20%' },
            { "data": 'phoneNumber', "width": '20%' },
            {
                "data": 'applicationUser',
                "render": function (data) {
                    return data ? data.email : "N/A"; // Avoid null reference errors
                },
                "width": '15%'
            },
            { "data": 'orderStatus', "width": '20%' },
            { "data": 'orderTotal', "width": '10%' },
            {
                "data": 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/Order/Details?orderId=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                    </div>`;
                },
                "width": "10%"
            }
        ],
        "initComplete": function (settings, json) {
            //console.log("JSON Response:", json); // ✅ Logs the response correctly
        }
    });
}
