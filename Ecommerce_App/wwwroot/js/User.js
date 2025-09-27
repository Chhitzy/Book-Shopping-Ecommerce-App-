var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tbl').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company", "width": "15%" },
            { "data": "role", "width": "8%" }, {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {

                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        //userlocked
                        return `
                    <div class ="text-center">
                    <a class ="btn btn-danger" onclick=LockUnlock('${data.id}')>
                    Unlock</a>
                    </div>  
                    `;
                    }
                    else {
                        //user unlocked

                        return `
                        <div class = "text-center">
                        <a class ="btn btn-success" onclick = LockUnlock('${data.id}')>
                        Lock
                        </a>
                        </div>
                        `;
                    }

                }
            }
        ]
    })
}

function LockUnlock(id) {
    $.ajax({
        url: "/Admin/User/LockUnlock",
        type: "POST",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
}