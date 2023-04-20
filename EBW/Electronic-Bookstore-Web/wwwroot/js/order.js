var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("orderpending")) {
        loadDataTable("orderpending");
    }
    else {
        if (url.includes("paymentpending")) {
            loadDataTable("paymentpending");
        }
        else {
            if (url.includes("orderapproved")) {
                loadDataTable("orderapproved");
            }
            else {
                if (url.includes("paymentapproved")) {
                    loadDataTable("paymentapproved");
                }
                else {
                    if (url.includes("orderprocessing")) {
                        loadDataTable("orderprocessing");
                    }
                    else {
                        if (url.includes("paymentrejected")) {
                            loadDataTable("paymentrejected");
                        }
                        else {
                            if (url.includes("ordershipped")) {
                                loadDataTable("ordershipped");
                            }
                            else {
                                loadDataTable("all");
                            }
                        }
                    }
                }
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#orderTable').DataTable({
        "ajax": { url:  '/Admin/Order/GetAll?status=' + status },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "lastName", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "applicationUser.email", "width": "5%" },
            { "data": "orderStatus", "width": "10%" },
            { "data": "paymentStatus", "width": "10%" },
            { "data": "totalOrderPrice", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group text-center" role="group">
                            <a href="/Admin/Order/Details?id=${data}"  <i class="bi bi-pencil-square"></i> </a>
					    </div>
                        `
                },
                "width": "3%"
            }
        ],
        "responsive": true,
    });
}

$(window).on('resize', function () {
    dataTable.columns.adjust().draw();
});
