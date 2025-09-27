var dataTable;
$(document).ready(
    function() {
        loadDataTable();
    })

function loadDataTable() {
    dataTable = $("#tbl").dataTable({
        ajax: {
            url: "/Admin/CoverType/GetAll"
        },
        columns: [
            { data :"name", width : "40%" },
            {
                "data": "id",
                "render": function (data) {

                    return `
                        <div class="text-center">
                        <a href = "/Admin/CoverType/Upsert/${data}" class="btn btn-info">
                        <i class="fas fa-edit"></i>
                        </a>
                        <a class = "btn btn-danger" onclick = Delete("/Admin/CoverType/Delete/${data}")>
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
