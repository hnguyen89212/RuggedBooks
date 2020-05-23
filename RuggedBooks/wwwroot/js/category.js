var dataTable = null;

$(document).ready(function () {
    loadDataTable();
});

/**
 * Constructs and renders the table of all categories via making AJAX request.
 */
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            { "data": "categoryName", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Category/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/Admin/Category/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                },
                "width": "40%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure about deleting this category?",
        text: "You cannot undo this action later.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then(response => {
        if (response) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}