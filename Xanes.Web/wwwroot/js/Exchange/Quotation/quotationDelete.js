let containerMain;
let dataTableTransfer, dataTableDeposit, parentId, amountTotalDeposit = 0, amountTotalTransfer = 0;
let tableRowLabelTransfer, tableRowLabelDeposit, amountHeader;
let selectBankSourceDeposit, selectBankSourceTransfer, selectBankTargetTransfer,
    amountDeposit, amountTransfer, idDetailDeposit, idDetailTransfer, TCHeader;
let inputsFormatTransa, inputsFormatExchange;
document.addEventListener("DOMContentLoaded", function () {

    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid";
    parentId = document.querySelector("#parentId").value;
    amountHeader = document.querySelector("#amountHeader").value;
    TCHeader = document.querySelector("#TCHeader");
    tableRowLabelDeposit = document.querySelector("#tableRowLabelDeposit");
    tableRowLabelTransfer = document.querySelector("#tableRowLabelTransfer");
    inputsFormatTransa = document.querySelectorAll(".decimalTransa");
    inputsFormatExchange = document.querySelectorAll(".decimalTC");

    inputsFormatTransa.forEach((item) => {
        item.value = formatterAmount().format(fnparseFloat(item.value));
    });

    inputsFormatExchange.forEach((item) => {
        item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value));
    });

    fnLoadDatatableDeposit();
    fnLoadDatatableTransfer();
    fnEnableTooltip();
});


const fndeleteRow = async (id) => {
    let result = await Swal.fire({
        title: `&#191;Está seguro de eliminar la cotización?`,
        html: `Este registro no se podrá recuperar`,
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

    try {

        let url = `/exchange/quotation/Delete?id=${id}`;

        const response = await fetch(url, {
            method: 'POST'
        });

        const jsonResponse = await response.json();
        if (!jsonResponse.isSuccess) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: jsonResponse.errorMessages
            });
        } else {
            if (jsonResponse.urlRedirect) {
                window.location.href = jsonResponse.urlRedirect;
            }
        }

    } catch (e) {
        Swal.fire({
            icon: 'error',
            title: "Error en la conexión",
            text: e
        });
    }
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
                data: 'lineNumber', "width": "2%", orderable: true
            },
            {
                data: 'bankSourceTrx.code', "width": "20%", orderable: false
            },
            {
                data: 'amountDetail', "width": "15%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            }
        ],
        "searching": false,
        "paging": false,
        "select": selectOptions,
        "language": {
            infoEmpty: "No hay datos para mostrar",
            zeroRecords: "No se encontraron coincidencias",
            processing: "Procesando...",
            loadingRecords: "Cargando...",
            emptyTable: "No hay datos disponibles en la tabla",
            select: {
                cells: {
                    "1": "1 celda seleccionada",
                    "_": "%d celdas seleccionadas"
                },
                columns: {
                    "1": "1 columna seleccionada",
                    "_": "%d columnas seleccionadas"
                },
                rows: {
                    "1": "1 fila seleccionada",
                    "_": "%d filas seleccionadas"
                }
            }
        },
        "footer": true,
        "footerCallback": function (row, data) {
            let footerCell = $(this.api().column(0).footer());
            footerCell.removeClass();
            footerCell.addClass('footer-left text-end');
            let total = 0, pending = 0;
            data.forEach((item) => {
                total += item.amountDetail;
            });
            pending = parseFloat(amountHeader) - total;
            tableRowLabelDeposit.innerHTML = `Depósitos: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
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
                data: 'lineNumber', "width": "2%", orderable: true
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
            }
        ],
        "searching": false,
        "paging": false,
        "select": selectOptions,
        "language": {
            infoEmpty: "No hay datos para mostrar",
            zeroRecords: "No se encontraron coincidencias",
            processing: "Procesando...",
            loadingRecords: "Cargando...",
            emptyTable: "No hay datos disponibles en la tabla",
            select: {
                cells: {
                    "1": "1 celda seleccionada",
                    "_": "%d celdas seleccionadas"
                },
                columns: {
                    "1": "1 columna seleccionada",
                    "_": "%d columnas seleccionadas"
                },
                rows: {
                    "1": "1 fila seleccionada",
                    "_": "%d filas seleccionadas"
                }
            }
        },
        "footer": true,
        "footerCallback": function (row, data) {
            let footerCell = $(this.api().column(0).footer());
            footerCell.removeClass();
            footerCell.addClass('footer-left text-end');
            let total = 0, pending = 0;
            data.forEach((item) => {
                total += item.amountDetail;
            });
            pending = parseFloat(amountHeader) - total;
            tableRowLabelTransfer.innerHTML = `Transferencias: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });
}
