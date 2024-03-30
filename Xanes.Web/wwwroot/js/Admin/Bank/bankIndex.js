let dataTable;
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
                data: null,
                "render": (data, type, row) => {
                    return `<div class="btn-group" role="group">
                        <a href="/admin/bank/upsert?id=${row.id}" class="btn btn-primary py-1 px-3 my-0 mx-2"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a onClick="fnDeleteRow('/admin/bank/delete?id=${row.id}','${row.code}','${row.name}')" class="btn btn-danger py-1 px-3 my-0 mx-2" 
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
        "select":true,

    });
}

const fnDeleteRow = async (url,code,rowname) => {
    let fetchOptions;

    let result = await Swal.fire({
        title: `&#191;Está seguro de eliminar el Banco?`,
        html: `${code} ${rowname}<br>Este registro no se podrá recuperar`,
        icon: "warning",
        showCancelButton: true,
        reverseButtons: true,
        focusConfirm: false,
        confirmButtonText: ButtonsText.Delete,
        cancelButtonText: ButtonsText.Cancel,
        customClass: {
            confirmButton: "btn btn-danger px-3 mx-2",
            cancelButton: "btn btn-primary px-3 mx-2"
        },
        buttonsStyling: false
    });

    if (!result.isConfirmed) {
        return;
    }

    fetchOptions = {
        method: "DELETE"
    };

    const fetchResponse = await fetch(url, fetchOptions);

    if (fetchResponse.ok) {
        let jsonResponse = await fetchResponse.json();
        console.log(jsonResponse);

        if (jsonResponse.isSuccess) {
            dataTable.ajax.reload();
            toastr.success(jsonResponse.successMessages);
        } else {
            toastr.success(jsonResponse.errorMessages);
        }
    }
    else {
        console.log(fetchResponse);
    }
}