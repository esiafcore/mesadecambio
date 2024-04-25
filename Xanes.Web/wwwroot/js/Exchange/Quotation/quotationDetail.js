let containerMain;
let dataTableTransfer, dataTableDeposit, parentId;
document.addEventListener("DOMContentLoaded", function () {
    
    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid";
    parentId = document.querySelector("#parentId").value;
    fnLoadDatatableDeposit();
    fnLoadDatatableTransfer();
    //Habilitar Tooltip
    fnEnableTooltip();
});


const fnShowModalDeposit = () => {
    $('#modalCreateDeposit').modal('show');
};


const fnShowModalTransfer = () => {
    $('#modalCreateTransfer').modal('show');
};
function fnLoadDatatableDeposit() {
    dataTableDeposit = new DataTable("#tblDeposit", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/exchange/quotation/GetAllDepositByParent?parentId=${parentId}`,
            "dataSrc": function (data) {
                if (data.isSuccess) {
                    return data.data;
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Error",
                        text: data.errorMessages
                    });
                    return [];
                }
            }
        },
        "columns": [
            {
                data: 'lineNumber', "width": "5%", orderable: true
            },
            {
                data: 'bankSourceTrx.code', "width": "20%", orderable: false
            },
            {
                data: 'amountDetail', "width": "15%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            },
            {
                data: null, "width": "5%", orderable: false
                , "render": (data, type, row) => {
                    return `<div class="btn-group" role="group">        
                        <a class="btn btn-primary py-1 px-3 my-0 mx-1"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a class="btn btn-danger py-1 px-3 my-0 mx-1" 
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Eliminar">
                            <i class="bi bi-trash-fill fs-5"></i>
                        </a>
                    </div>`;
                }
            }
        ],
        "searching": true,
        "select": selectOptions
    });
}
function fnLoadDatatableTransfer() {
    dataTableTransfer = new DataTable("#tblTransfer", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/exchange/quotation/GetAllTransferByParent?parentId=${parentId}`,
            "dataSrc": function (data) {
                if (data.isSuccess) {
                    return data.data;
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Error",
                        text: data.errorMessages
                    });
                    return [];
                }
            }
        },
        "columns": [
            {
                data: 'lineNumber', "width": "5%", orderable: true
            },
            {
                data: 'bankSourceTrx.code', "width": "20%", orderable: false
            },
            {
                data: 'bankTargetTrx.code', "width": "20%", orderable: false
            },
            {
                data: 'amountDetail', "width": "15%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            },
            {
                data: null, "width": "5%", orderable: false
                , "render": (data, type, row) => {
                    return `<div class="btn-group" role="group">        
                        <a class="btn btn-primary py-1 px-3 my-0 mx-1"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a class="btn btn-danger py-1 px-3 my-0 mx-1" 
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Eliminar">
                            <i class="bi bi-trash-fill fs-5"></i>
                        </a>
                    </div>`;
                }
            }
        ],
        "searching": true,
        "select": selectOptions
    });
}
