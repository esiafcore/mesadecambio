let containerMain;
let dataTableTransfer, dataTableDeposit, parentId, amountTotalDeposit = 0, amountTotalTransfer = 0;
let tableRowLabelTransfer, tableRowLabelDeposit, amountHeader;
let selectBankSourceDeposit, selectBankSourceTransfer, selectBankTargetTransfer,
    amountDeposit, amountTransfer, idDetailDeposit, idDetailTransfer, TCHeader;
let inputExchangeRateBuyTransa, inputExchangeRateSellTransa, inputExchangeRateOfficialTransa;
let inputsFormatTransa, inputsFormatExchange, inputAmountTransa;
document.addEventListener("DOMContentLoaded", function () {

    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid";
    parentId = document.querySelector("#parentId").value;
    amountHeader = document.querySelector("#amountHeader").value;
    tableRowLabelDeposit = document.querySelector("#tableRowLabelDeposit");
    tableRowLabelTransfer = document.querySelector("#tableRowLabelTransfer");
    selectBankSourceDeposit = document.querySelector("#selectBankSourceDeposit");
    selectBankSourceTransfer = document.querySelector("#selectBankSourceTransfer");
    selectBankTargetTransfer = document.querySelector("#selectBankTargetTransfer");
    amountDeposit = document.querySelector("#amountDeposit");
    amountTransfer = document.querySelector("#amountTransfer");
    idDetailDeposit = document.querySelector("#idDetailDeposit");
    idDetailTransfer = document.querySelector("#idDetailTransfer");
    TCHeader = document.querySelector("#TCHeader");
    inputsFormatTransa = document.querySelectorAll(".decimalTransa");
    inputsFormatExchange = document.querySelectorAll(".decimalTC");
    inputExchangeRateBuyTransa = document.querySelector("#exchangeRateBuyTransa");
    inputExchangeRateSellTransa = document.querySelector("#exchangeRateSellTransa");
    inputExchangeRateOfficialTransa = document.querySelector("#exchangeRateOfficialTransa");
    inputAmountTransa = document.querySelector("#amountTransa");

    //Aplicar select2
    $("#selectCustomer").select2(select2Options);

    //Setear enfoque en el search input
    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });

    inputsFormatTransa.forEach((item) => {
        item.value = formatterAmount().format(fnparseFloat(item.value));

        item.addEventListener("change", () => {
            item.value = formatterAmount().format(fnparseFloat(item.value));
        });
    });

    inputsFormatExchange.forEach((item) => {
        item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value));

        item.addEventListener("change", () => {
            item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value));
        });
    });

    document.querySelectorAll("#amountTransa, #exchangeRateSellTransa, #exchangeRateBuyTransa").forEach((item) => item.addEventListener("change", () => {

    }));


    //TCHeader.value = formatterAmount().format(TCHeader.value);
    //amountHeader.value = formatterAmount().format(amountHeader.value);
    //amountTotalDeposit = parseFloat(document.querySelector("#totalAmountDeposit").value);
    //amountTotalTransfer = parseFloat(document.querySelector("#totalAmountTransfer").value);
    fnLoadDatatableDeposit();
    fnLoadDatatableTransfer();
    //Habilitar Tooltip
    fnEnableTooltip();
});


//Funcion para calcular el costo
const fnCalculateRevenueCost = () => {
    let divRevenue = document.querySelector("#divRevenue");
    let divCost = document.querySelector("#divCost");
    let amountExchange = document.querySelector("#amountExchange");
    let exchangeRateBuyTransa = fnparseFloat(inputExchangeRateBuyTransa.value);
    let exchangeRateSellTransa = fnparseFloat(inputExchangeRateSellTransa.value);
    let exchangeRateOfficialTransa = fnparseFloat(inputExchangeRateOfficialTransa.value);
    let amountTransa = fnparseFloat(inputAmountTransa.value);
    let amountCost = 0, amountRevenue = 0;

    if (inputTypeNumeral.value == QuotationType.Buy) {
        if (exchangeRateBuyTransa > exchangeRateOfficialTransa) {
            amountCost = (exchangeRateBuyTransa - exchangeRateOfficialTransa) * amountTransa;
            divRevenue.hidden = true;
            divCost.hidden = false;
        } else {
            amountRevenue = (exchangeRateOfficialTransa - exchangeRateBuyTransa) * amountTransa;
            divRevenue.hidden = false;
            divCost.hidden = true;
        }
        amountExchange.value = formatterAmount().format(amountTransa * exchangeRateBuyTransa);
    } else if (inputTypeNumeral.value == QuotationType.Sell) {
        if (exchangeRateSellTransa > exchangeRateOfficialTransa) {
            amountRevenue = (exchangeRateSellTransa - exchangeRateOfficialTransa) * amountTransa;
            divRevenue.hidden = false;
            divCost.hidden = true;
        } else {
            amountCost = (exchangeRateOfficialTransa - exchangeRateSellTransa) * amountTransa;
            divRevenue.hidden = true;
            divCost.hidden = false;
        }
        amountExchange.value = formatterAmount().format(amountTransa * exchangeRateSellTransa);
    }
    inputExchangeRateBuyTransa.value = formatterAmount(decimalExchange).format(exchangeRateBuyTransa);
    inputExchangeRateOfficialTransa.value = formatterAmount(decimalExchange).format(exchangeRateOfficialTransa);
    inputAmountCost.value = formatterAmount().format(amountCost);
    inputAmountRevenue.value = formatterAmount().format(amountRevenue);
    inputAmountTransa.value = formatterAmount().format(amountTransa);
};

const fnClearModalDeposit = () => {
    document.querySelector("#idDetailDeposit").value = 0;
    document.querySelector("#amountDeposit").value = formatterAmount().format(0);
    document.querySelector("#selectBankSourceDeposit").selectedIndex = 0;
};

const fnClearModalTransfer = () => {
    document.querySelector("#idDetailTransfer").value = 0;
    document.querySelector("#amountTransfer").value = formatterAmount().format(0);
    document.querySelector("#selectBankSourceTransfer").selectedIndex = 0;
    document.querySelector("#selectBankTargetTransfer").selectedIndex = 0;
};

const fnShowModalUpdateHeader = () => {
    $('#modalUpdateHeader').modal('show');
};

const fnShowModalDeposit = () => {
    fnClearModalDeposit();
    document.querySelector("#staticBackdropLabelDeposit").innerHTML = "Nueva Cotización";
    document.querySelector("#infoModalDeposit").innerHTML = tableRowLabelDeposit.value;
    $('#modalCreateDeposit').modal('show');
};

const fnShowModalTransfer = () => {
    fnClearModalTransfer();
    document.querySelector("#staticBackdropLabelTransfer").innerHTML = "Nueva Transferencia";
    document.querySelector("#infoModalTransfer").innerHTML = tableRowLabelTransfer.value;
    $('#modalCreateTransfer').modal('show');
};

const fndeleteRow = async (id) => {
    let result = await Swal.fire({
        title: `&#191;Está seguro de eliminar el detalle?`,
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

        let url = `/exchange/quotation/DeleteDetail?id=${id}`;

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
            toastr.success(jsonResponse.successMessages);
            dataTableDeposit.ajax.reload();
            dataTableTransfer.ajax.reload();
        }

    } catch (e) {
        Swal.fire({
            icon: 'error',
            title: "Error en la conexión",
            text: e
        });
    }
};

const fnupdateRow = (id, amount, bankSource, bankTarget, quotationDetailType) => {
    if (quotationDetailType == QuotationDetailType.Deposit) {
        document.querySelector("#staticBackdropLabelDeposit").innerHTML = "Actualizar Cotización";
        idDetailDeposit.value = id;
        amountDeposit.value = formatterAmount().format(amount);
        [...selectBankSourceDeposit.options].forEach((opt) => {
            if (opt.value == bankSource) {
                opt.selected = true;
                return;
            }
        });
        document.querySelector("#infoModalDeposit").innerHTML = tableRowLabelDeposit.value;
        $('#modalCreateDeposit').modal('show');
    } else {
        document.querySelector("#staticBackdropLabelTransfer").innerHTML = "Actualizar Transferencia";
        idDetailTransfer.value = id;
        amountTransfer.value = formatterAmount().format(amount);
        [...selectBankSourceTransfer.options].forEach((opt) => {
            if (opt.value == bankSource) {
                opt.selected = true;
                return;
            }
        });
        [...selectBankTargetTransfer.options].forEach((opt) => {
            if (opt.value == bankTarget) {
                opt.selected = true;
                return;
            }
        });
        document.querySelector("#infoModalTransfer").innerHTML = tableRowLabelTransfer.value;
        $('#modalCreateTransfer').modal('show');
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
                data: 'amountDetail', "width": "15%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            },
            {
                data: null, "width": "5%", orderable: false
                , "render": (data, type, row) => {
                    return `<div class="btn-group" role="group">        
                        <a class="btn btn-primary py-1 px-3 my-0 mx-1"
                         onclick="fnupdateRow(${data.id}, ${data.amountDetail}, ${data.bankSourceId}, ${data.bankTargetId}, '${QuotationDetailType.Deposit}')"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a class="btn btn-danger py-1 px-3 my-0 mx-1"
                            onclick="fndeleteRow(${data.id})"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Eliminar">
                            <i class="bi bi-trash-fill fs-5"></i>
                        </a>
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
            let total = 0, pending = 0;
            data.forEach((item) => {
                total += item.amountDetail;
            });
            pending = parseFloat(amountHeader) - total;
            if (pending == 0) {
                document.querySelector("#btnCreateDetailDeposit").hidden = true;
            } else {
                document.querySelector("#btnCreateDetailDeposit").hidden = false;
            }
            tableRowLabelDeposit.innerHTML = `Depósitos: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
            tableRowLabelDeposit.value = `Depósitos: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
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
            },
            {
                data: null, "width": "5%", orderable: false
                , "render": (data, type, row) => {
                    return `<div class="btn-group" role="group">        
                        <a class="btn btn-primary py-1 px-3 my-0 mx-1"
                            onclick="fnupdateRow(${data.id}, ${data.amountDetail}, ${data.bankSourceId}, ${data.bankTargetId},'${QuotationDetailType.Transfer}')"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a class="btn btn-danger py-1 px-3 my-0 mx-1"
                            onclick="fndeleteRow(${data.id})"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Eliminar">
                            <i class="bi bi-trash-fill fs-5"></i>
                        </a>
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
            let total = 0, pending = 0;
            data.forEach((item) => {
                total += item.amountDetail;
            });
            pending = parseFloat(amountHeader * fnparseFloat(TCHeader.value)) - (total);
            if (pending == 0) {
                document.querySelector("#btnCreateDetailTransfer").hidden = true;
            } else {
                document.querySelector("#btnCreateDetailTransfer").hidden = false;

            }
            tableRowLabelTransfer.innerHTML = `Transferencias: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
            tableRowLabelTransfer.value = `Transferencias: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });
}
