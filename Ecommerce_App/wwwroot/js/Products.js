var dataTable;
$(document).ready(function () {
    loadDataTable();
})


function loadDataTable() {
    dataTable = $("#tbl").dataTable({

        ajax: {
            url: "/Admin/Products/GetAll"
        },
        columns:[
            { data: "title", width: "16%" },
            { data: "author", width: "16%" },
            { data: "description", width: "16%" },
            { data: "isbn", width: "16%" },
            { data: "price", width: "16%" },
            {
                data: "id",
                "render": function (data) {
                    return `
                    <div class ="text-center">
                    <a href= "/Admin/Products/Upsert/${data}" class="btn btn-info">
                    <i class ="fas fa-edit"></i>
                    </a>
                    <a class="btn btn-danger" onclick = Delete("/Admin/Products/Delete/${data}")>
                    <i class="fas fa-trash-alt"></i>
                    </a>
                    </div>
                    `;
                   
                }
            }
        ]
    })
}

function Delete(url) {
    //alert(url)
    //console.log(url)
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