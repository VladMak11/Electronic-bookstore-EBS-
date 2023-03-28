var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#productTable').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "listPrice", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "category.name" , "width": "15%" },
            { "data": "coverType.name", "width": "15%" },
            { "data": "author.fullName", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group text-center" role="group">
                            <a href="/Admin/Product/Upsert?id=${data}"  <i class="bi bi-pencil-square"></i> </a>
                            &nbsp;&nbsp;
                            <a onClick=Delete('/Admin/Product/Delete/${data}') <i class="bi bi-trash3-fill"></i> </a>
					    </div>
                        `
                },
                "width": "5%"
            }
        ]
    });
}


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