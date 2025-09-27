var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/customer/OrderHistory/GetAll",
            "dataSrc": "all"
        },
        "columns": [
            { "data": "orderDate", "width": "10%" },
            { "data": "shippingDate", "width": "15%" },
            { "data": "orderTotal", "width": "11%" },
            { "data": "state", "width": "15%" },
            { "data": "phoneNumber", "width": "10%" },
           
        ]
    })
}

