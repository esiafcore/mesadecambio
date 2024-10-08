﻿let containerMain;
let dataTableTransfer, dataTableDeposit, parentId, amountTotalDeposit = 0, amountTotalTransfer = 0;
let tableRowLabelTransfer, tableRowLabelDeposit, amountHeader;
let selectBankSourceDeposit, selectBankSourceTransfer, selectBankTargetTransfer,
    amountDeposit, amountTransfer, idDetailDeposit, idDetailTransfer, TCHeader;
let inputsFormatTransa, inputsFormatExchange, typeNumeral, amountExchangeDetail, currencyTypeDeposit, currencyType, currencyTypeTransfer;
document.addEventListener("DOMContentLoaded", function () {

    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid";
    parentId = document.querySelector("#parentId").value;
    amountHeader = document.querySelector("#amountHeader").value;
    TCHeader = document.querySelector("#TCHeader");
    amountExchangeDetail = document.querySelector("#amountExchangeDetail");
    tableRowLabelDeposit = document.querySelector("#tableRowLabelDeposit");
    tableRowLabelTransfer = document.querySelector("#tableRowLabelTransfer");
    currencyTypeDeposit = document.querySelector("#currencyDepositType").value;
    currencyType = document.querySelector("#currencyTransaType").value;
    currencyTypeTransfer = document.querySelector("#currencyTransferType").value;
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
    if (typeNumeral == QuotationType.Transport) {
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
                    if (typeNumeral != QuotationType.Transport) {
                        dataCode = data.bankSourceCode;
                    } else {
                        dataCode = data.bankTargetCode;
                    }
                    return dataCode;
                }
            },
            {
                data: 'amountDetail', "width": "15%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            },
            {
                data: null, "width": "25%", orderable: false,
                render: (data, type, row) => {
                    let trx = data.transactionBcoFullName;
                    let asi = data.journalEntryFullName;
                    let asiAnu = data.JournalEntryVoidId ? data.journalEntryVoidFullName : '';

                    return `
                            <div>
                                ${trx}<br>
                                ${asi}<br>
                                ${asiAnu ? asiAnu + '<br>' : ''}
                            </div>`;
                }
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
            let total = 0, pending = 0, deposit = 0, label = "Depositar";
            data.forEach((item) => {
                total += fnparseFloat(item.amountDetail.toFixed(decimalTransa));
            });
            if (typeNumeral == QuotationType.Buy) {
                pending = fnparseFloat(amountHeader) - total;
                deposit = amountHeader;
            } else if (typeNumeral == QuotationType.Sell) {

                if (currencyType == CurrencyType.Foreign) {
                    deposit = fnparseFloat(amountHeader) * fnparseFloat(TCHeader.value);
                    pending = (deposit.toFixed(decimalTransa)) - (total.toFixed(decimalTransa));
                } else if (currencyType == CurrencyType.Additional) {
                    if (currencyTypeDeposit == CurrencyType.Foreign) {
                        deposit = fnparseFloat(amountHeader) * fnparseFloat(TCHeader.value);
                        pending = (deposit.toFixed(decimalTransa)) - (total.toFixed(decimalTransa));
                    } else if (currencyTypeDeposit == CurrencyType.Base) {
                        deposit = fnparseFloat(amountHeader) * fnparseFloat(TCHeader.value);
                        pending = (deposit.toFixed(decimalTransa)) - (total.toFixed(decimalTransa));
                    }
                }
            } else {
                label = "TRC";
                deposit = amountHeader;
            }
            tableRowLabelDeposit.innerHTML =
                `${label}: ${formatterAmount().format(fnparseFloat(deposit))}  -  Pendiente: ${formatterAmount().format(pending)
                }`;
            tableRowLabelDeposit.value =
                `${label}: ${formatterAmount().format(fnparseFloat(deposit))}  -  Pendiente: ${formatterAmount().format(pending)
                }`;

            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });
}
function fnLoadDatatableTransfer() {

    let typeDetail = QuotationDetailType.Transfer;
    if (typeNumeral == QuotationType.Transport) {
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
                data: 'bankSourceCode', "width": "20%", orderable: false
            },
            {
                data: 'bankTargetCode', "width": "20%", orderable: false
            },
            {
                data: 'amountDetail', "width": "15%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            },
            {
                data: null, "width": "25%", orderable: false,
                render: (data, type, row) => {
                    let trx = data.transactionBcoFullName;
                    let asi = data.journalEntryFullName;
                    let asiAnu = data.JournalEntryVoidId ? data.journalEntryVoidFullName : '';

                    return `
                            <div>
                                ${trx}<br>
                                ${asi}<br>
                                ${asiAnu ? asiAnu + '<br>' : ''}
                            </div>`;
                }
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
            let amount = 0;
            footerCell.removeClass();
            footerCell.addClass('footer-left text-end');
            let total = 0, pending = 0, transfer = 0, label = "Transferir";
            data.forEach((item) => {
                total += fnparseFloat(item.amountDetail.toFixed(decimalTransa));
            });
            if (typeNumeral == QuotationType.Buy) {
                amount = fnparseFloat(amountHeader) * fnparseFloat(TCHeader.value);
                pending = (amount.toFixed(decimalTransa)) - (total.toFixed(decimalTransa));
            } else if (typeNumeral == QuotationType.Sell) {
                if (currencyType == CurrencyType.Foreign) {
                    pending = fnparseFloat(amountHeader) - total.toFixed(decimalTransa);
                } else if (currencyType == CurrencyType.Additional) {
                    if (currencyTypeDeposit == CurrencyType.Base) {
                        pending = fnparseFloat(amountHeader) - total.toFixed(decimalTransa);
                    } else if (currencyTypeDeposit == CurrencyType.Foreign) {
                        pending = fnparseFloat(amountHeader) - total.toFixed(decimalTransa);
                    }
                }
            }

            if (typeNumeral != QuotationType.Transport) {

                if (typeNumeral == QuotationType.Buy) {
                    if (currencyType == CurrencyType.Foreign) {
                        transfer = amountExchangeDetail.value;
                    } else {
                        if (currencyTypeTransfer == CurrencyType.Base) {
                            transfer = amountExchangeDetail.value;
                        } else {
                            transfer = amountExchangeDetail.value;
                        }
                    }
                } else {
                    if (currencyType == CurrencyType.Foreign) {
                        transfer = amountHeader;
                    } else if (currencyType == CurrencyType.Additional) {
                        if (currencyTypeDeposit == CurrencyType.Base) {
                            transfer = amountHeader;
                        } else if (currencyTypeDeposit == CurrencyType.Foreign) {
                            transfer = amountHeader;
                        }
                    }
                }
            } else {
                label = "TRD";
                transfer = amountHeader;
            }

            tableRowLabelTransfer.innerHTML =
                `${label}: ${formatterAmount().format(fnparseFloat(transfer))}  -  Pendiente: ${formatterAmount().format(pending)
                }`;
            tableRowLabelTransfer.value =
                `${label}: ${formatterAmount().format(fnparseFloat(transfer))}  -  Pendiente: ${formatterAmount().format(pending)
                }`;
            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });

    if (typeNumeral == QuotationType.Transport) {
        dataTableTransfer.column(2).visible(false);
    }
}
