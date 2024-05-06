let containerMain;
let dataTableTransfer, dataTableDeposit, parentId, amountTotalDeposit = 0, amountTotalTransfer = 0;
let tableRowLabelTransfer, tableRowLabelDeposit, amountHeader;
let selectBankSourceDeposit, selectBankSourceTransfer, selectBankTargetTransfer,
    amountDeposit, amountTransfer, idDetailDeposit, idDetailTransfer, TCHeader;
let inputsFormatTransa, inputsFormatExchange, typeNumeral, amountExchange;
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
    typeNumeral = document.querySelector("#type").value;
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
    let typeDetail = QuotationDetailType.Deposit;
    if (typeNumeral == QuotationType.Transfer) {
        typeDetail = QuotationDetailType.CreditTransfer;
    }

    dataTableDeposit = new DataTable("#tblDeposit", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/exchange/quotation/GetAllByParent?parentId=${parentId}&type=${typeDetail}`,
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
            },
            "complete": function () {
                fnEnableTooltip();
            }
        },
        "columns": [
            {
                data: 'lineNumber',
                "width": "2%",
                orderable: true
            },
            {
                data: null, "width":
                    "20%", orderable: false,
                "render": (data) => {
                    let dataCode;
                    if (typeNumeral != QuotationType.Transfer) {
                        dataCode = data.bankSourceTrx.code;
                    } else {
                        dataCode = data.bankTargetTrx.code;
                    }
                    return dataCode;
                }
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
            if (typeNumeral == QuotationType.Buy) {
                pending = fnparseFloat(amountHeader) - total;
            } else if (typeNumeral == QuotationType.Sell) {
                pending = fnparseFloat(fnparseFloat(amountHeader) * fnparseFloat(TCHeader.value)) - (total);
            }

            if (typeNumeral != QuotationType.Transfer) {

                tableRowLabelDeposit.innerHTML =
                    `Depositar: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)}`;
                tableRowLabelDeposit.value =
                    `Depositar: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)}`;
            } else {
                tableRowLabelDeposit.innerHTML = `TRC: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)}`;
                tableRowLabelDeposit.value = `TRC: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)}`;
            }

            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });
}
function fnLoadDatatableTransfer() {

    let typeDetail = QuotationDetailType.Transfer;
    if (typeNumeral == QuotationType.Transfer) {
        typeDetail = QuotationDetailType.DebitTransfer;
    }

    dataTableTransfer = new DataTable("#tblTransfer", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/exchange/quotation/GetAllByParent?parentId=${parentId}&type=${typeDetail}`,
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
            },
            "complete": function () {
                fnEnableTooltip();
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
            if (typeNumeral == QuotationType.Buy) {
                pending = fnparseFloat(fnparseFloat(amountHeader) * fnparseFloat(TCHeader.value)) - (total);
            } else if (typeNumeral == QuotationType.Sell) {
                pending = fnparseFloat(amountHeader) - total;
            }

            if (typeNumeral != QuotationType.Transfer) {
                amountExchange = document.querySelector("#amountExchange");
                tableRowLabelTransfer.innerHTML =
                    `Transferir: ${formatterAmount().format(fnparseFloat(amountExchange.value))}  -  Pendiente: ${formatterAmount().format(pending)
                    }`;
                tableRowLabelTransfer.value =
                    `Transferir: ${formatterAmount().format(fnparseFloat(amountExchange.value))}  -  Pendiente: ${formatterAmount().format(pending)
                    }`;
            } else {
                tableRowLabelTransfer.innerHTML =
                    `TRD: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)
                    }`;
                tableRowLabelTransfer.value =
                    `TRD: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)
                    }`;
            }

            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });

    if (typeNumeral == QuotationType.Transfer) {
        dataTableTransfer.column(2).visible(false);
    }
}
