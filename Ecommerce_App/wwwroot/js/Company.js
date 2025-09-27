var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tbl').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "11%" },
            { "data": "state", "width": "15%" },
            { "data": "phoneNumber", "width": "10%" },
            {
                "data": "isAuthorizedCompany", "width": "5%",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" checked disabled/>`;
                    }
                    else {
                        return `<input type="checkbox" disabled />`;
                    }
                }
            },

            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class ="text-center">
                    <a href ="/Admin/Company/Upsert/${data}" class ="btn btn-info">
                    <i class = "fas fa-edit"></i>
                    </a>
                    <a class= "btn btn-danger" onclick = Delete('/Admin/Company/Delete/${data}')>
                    <i class = "fas fa-trash-alt "></i>
                    </a>
                    </div>  
                    `;
                }
            }
        ]
    })
}

function Delete(url) {
    swal({
        title: "Want To Delete Data?",
        text: "Delete Information !!!",
        icon: "warning",
        dangerModel: true,
        buttons: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else
                        toastr.error(data.message);
                }
            })
        }
    }
    )
}