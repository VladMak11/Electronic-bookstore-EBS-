var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#coverTypeTable').DataTable({
        "ajax": {
            "url": "/Admin/CoverType/GetAll"
        },
        "columns": [
            { "data": "name", "width": "auto" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group text-center" role="group">
                            <a href="/Admin/CoverType/Upsert?id=${data}"  <i class="bi bi-pencil-square"></i> </a>
                            &nbsp;&nbsp;
                            <a onClick=Delete('/Admin/CoverType/Delete/${data}') <i class="bi bi-trash3-fill"></i> </a>
					    </div>
                        `
                },
                "width": "5%"
            }
        ],
        "responsive": true,
    });
}

$(window).on('resize', function () {
    dataTable.columns.adjust().draw();
});

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}