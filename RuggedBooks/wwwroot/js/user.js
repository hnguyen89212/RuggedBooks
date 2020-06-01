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
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "12%" },
            { "data": "email", "width": "12%" },
            { "data": "phoneNumber", "width": "12%" },
            { "data": "company.name", "width": "12%" },
            { "data": "role", "width": "12%" },
            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var todayDate = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > todayDate) {
                        // User is currently locked
                        return `
                            <div class="text-center">
                                <a onclick=LockOrUnlock('${ data.id }') class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-lock-open"></i> Unlock
                                </a>
                            </div>
                           `;
                    } else {
                        return `
                            <div class="text-center">
                                <a onclick=LockOrUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-lock"></i> Lock
                                </a>
                            </div>
                           `;
                    }
                },
                "width": "25%"
            }
        ]
    });
}

function LockOrUnlock(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/User/LockOrUnlock",
        data: JSON.stringify(id),
        contentType: "application/json",
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