var dataTable;
document.addEventListener("DOMContentLoaded", function () {
    loadDatatable();

    //Habilitar Tooltip
    fnEnableTooltip();
});

function loadDatatable() {
    dataTable = new DataTable("#tblData",{
        "ajax": { url: '/admin/bank/getall' },
        "dataSrc": 'data',
        "columns": [
            { data: 'code', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'bankingCommissionPercentage', "width": "5%" },
            { data: 'isCompany', "width": "5%" },
            { data: 'orderPriority', "width": "5%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="btn-group" role="group">
                        <a href="/admin/bank/upsert?id=${data}" class="btn btn-primary py-1 px-3 my-0 mx-2"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a onClick=Delete('/admin/bank/delete?id=${data}') class="btn btn-danger py-1 px-3 my-0 mx-2" 
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Eliminar">
                            <i class="bi bi-trash-fill fs-5"></i>
                        </a>

                    </div>`
                },
                "width": "15%" 
            }
        ],
        "info": false,
        "ordering": true,
        "paging": true,
        "order": [[1, 'asc'], [0, 'desc']],
        "columnDefs": [
            {
                target: 0,
                visible: true,
                searchable: true,
                orderable: true
            },
            {
                target: 1,
                orderable: false
            },
            {
                target: 2,
                orderable: false
            },
            {
                target: 3,
                orderable: false
            },
            {
                target: 4,
                orderable: false
            },

        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}