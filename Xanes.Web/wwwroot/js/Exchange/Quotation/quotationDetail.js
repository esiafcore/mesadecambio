let containerMain;
let dataTableTransfer, dataTableDeposit, parentId, amountTotalDeposit = 0, amountTotalTransfer = 0;
let tableRowLabelTransfer, tableRowLabelDeposit, amountHeader;
let selectBankSourceDeposit, selectBankSourceTransfer, selectBankTargetTransfer,
    amountDeposit, amountTransfer, idDetailDeposit, idDetailTransfer, TCHeader;
let inputExchangeRateBuyTransa, inputExchangeRateSellTransa, inputExchangeRateOfficialTransa;
let inputsFormatTransa, inputsFormatExchange, inputAmountTransa, inputDateTransa, currencies, currencyType, typeNumeral, typeNumerals;
document.addEventListener("DOMContentLoaded", async function () {
    currencies = document.querySelectorAll(".currenciesTransa");
    inputDateTransa = document.querySelector("#dateTransa");
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
    typeNumerals = document.querySelectorAll(".typeNumerals");
    currencies.forEach((item) => {
        if (item.checked) {
            currencyType = item.value;
        }
    });

    typeNumerals.forEach((item) => {
        if (item.checked) {
            typeNumeral = item.value;
        }
    });

    //Obtenemos el tipo de cambio en base a la fecha
    await fnTCByDate();

    inputDateTransa.addEventListener('change', async () => {
        await fnTCByDate();
    });

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
        fnCalculateRevenueCost();
    }));

    fnLoadDatatableDeposit();
    fnLoadDatatableTransfer();

    //Habilitar Tooltip
    fnEnableTooltip();
});

function formatOption(option) {
    if (!option.id) {
        return option.text;
    }

    var thumbnailUrl = option.element.dataset.thumbnail;

    if (thumbnailUrl) {
        var $option = $(
            '<span><img src="' + thumbnailUrl + '" class="img-thumbnail img-fluid" style="width: 40px; height: 40px; margin-right: 10px;" />' + option.text + '</span>'
        );
    } else {
        var $option = $('<span>' + option.text + '</span>');
    }

    return $option;
}

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

    if (typeNumeral == QuotationType.Buy) {
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

    } else if (typeNumeral == QuotationType.Sell) {
        if (exchangeRateSellTransa > exchangeRateOfficialTransa) {
            amountRevenue = (exchangeRateSellTransa - exchangeRateOfficialTransa) * amountTransa;
            divRevenue.hidden = false;
            divCost.hidden = true;
        } else {
            amountCost = (exchangeRateOfficialTransa - exchangeRateSellTransa) * amountTransa;
            divRevenue.hidden = true;
            divCost.hidden = false;
        }

        //Verificamos si el tcventa es cero para no realizar la division
        if (exchangeRateSellTransa == 0) {
            amountExchange.value = formatterAmount().format(0);
        } else {
            if (currencyType == CurrencyType.Foreign) {
                amountExchange.value = formatterAmount().format(amountTransa * exchangeRateSellTransa);
            } else if (currencyType == CurrencyType.Additional) {
                amountExchange.value = formatterAmount().format(amountTransa / exchangeRateSellTransa);
            }
        }
    }

    inputExchangeRateBuyTransa.value = formatterAmount(decimalExchange).format(exchangeRateBuyTransa);
    inputExchangeRateOfficialTransa.value = formatterAmount(decimalExchange).format(exchangeRateOfficialTransa);
    inputAmountCost.value = formatterAmount().format(amountCost);
    inputAmountRevenue.value = formatterAmount().format(amountRevenue);
    inputAmountTransa.value = formatterAmount().format(amountTransa);
};

//Funcion para obtener el tipo de cambio oficial
const fnTCByDate = async () => {
    let url = `/Admin/CurrencyExchangeRate/GetCurrencyExchangeRate?date=${inputDateTransa.value}`;

    try {

        const response = await fetch(url, {
            method: "POST"
        });

        const jsonResponse = await response.json();

        if (jsonResponse.isSuccess) {
            if (currencyType == CurrencyType.Foreign) {
                inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyForeign?.officialRate ?? 1;
            } else if (currencyType == CurrencyType.Additional) {
                inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyAdditional?.officialRate ?? 1;
            }
        } else {
            inputExchangeRateOfficialTransa.value = 1;
        }

    } catch (error) {
        alert(error);
    }
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

const fnShowModalDeposit = async () => {
    fnClearModalDeposit();
    document.querySelector("#staticBackdropLabelDeposit").innerHTML = "Nueva Cotización";
    document.querySelector("#infoModalDeposit").innerHTML = tableRowLabelDeposit.value;
    // Obtener la cantidad total de elementos en la lista
    var totalOptions = $('#selectBankSourceDeposit option').length;

    // Mostrar el modal
    $('#modalCreateDeposit').modal('show');

    // Esperar a que el modal esté completamente visible
    $('#modalCreateDeposit').on('shown.bs.modal', function () {
        // Inicializar Select2 en el elemento de selección
        $(selectBankSourceDeposit).select2({
            templateResult: formatOption,
            language: customMessagesSelect,
            allowClear: true,
            minimumResultsForSearch: -1, // Mostrar todos los elementos sin barra de búsqueda
            dropdownAutoWidth: true, // Ajustar automáticamente el ancho del menú desplegable
            dropdownParent: $('#modalCreateDeposit'),
            maximumSelectionLength: totalOptions // Establecer el tamaño máximo del menú desplegable
        });
        $(selectBankSourceDeposit).select2('open');
    });
};

const fnShowModalTransfer = () => {
    fnClearModalTransfer();
    document.querySelector("#staticBackdropLabelTransfer").innerHTML = "Nueva Transferencia";
    document.querySelector("#infoModalTransfer").innerHTML = tableRowLabelTransfer.value;
    // Obtener la cantidad total de elementos en la lista
    var totalOptionsSource = $('#selectBankSourceTransfer option').length;
    // Obtener la cantidad total de elementos en la lista
    var totalOptionsTarget = $('#selectBankTargetTransfer option').length;
    $('#modalCreateTransfer').modal('show');

    $('#modalCreateTransfer').on('shown.bs.modal', function () {
        // Inicializar Select2 en el elemento de selección
        $(selectBankSourceTransfer).select2({
            templateResult: formatOption,
            language: customMessagesSelect,
            allowClear: true,
            minimumResultsForSearch: -1, // Mostrar todos los elementos sin barra de búsqueda
            dropdownAutoWidth: true, // Ajustar automáticamente el ancho del menú desplegable
            dropdownParent: $('#modalCreateTransfer'),
            maximumSelectionLength: totalOptionsSource // Establecer el tamaño máximo del menú desplegable
        });
        $(selectBankSourceTransfer).select2('open');
        $(selectBankTargetTransfer).select2({
            templateResult: formatOption,
            language: customMessagesSelect,
            allowClear: true,
            minimumResultsForSearch: -1, // Mostrar todos los elementos sin barra de búsqueda
            dropdownAutoWidth: true, // Ajustar automáticamente el ancho del menú desplegable
            dropdownParent: $('#modalCreateTransfer'),
            maximumSelectionLength: totalOptionsTarget // Establecer el tamaño máximo del menú desplegable
        });
        $(selectBankTargetTransfer).select2('open');
    });
    //$('#selectBankSourceTransfer').select2({
    //    templateResult: formatOption,
    //    language: customMessagesSelect,
    //    allowClear: true,
    //    minimumResultsForSearch: Infinity,
    //    width: "resolve",
    //    dropdownParent: $('#modalCreateTransfer')
    //});
    //$('#selectBankTargetTransfer').select2({
    //    templateResult: formatOption,
    //    language: customMessagesSelect,
    //    allowClear: true,
    //    minimumResultsForSearch: Infinity,
    //    width: "resolve",
    //    dropdownParent: $('#modalCreateTransfer')
    //});
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

const fnApproved = async (id) => {
    //let result = await Swal.fire({
    //    title: `&#191;Está seguro de aprobar la cotizatión?`,
    //    html: `Este registro no se podrá recuperar`,
    //    icon: "warning",
    //    showCancelButton: true,
    //    reverseButtons: true,
    //    focusConfirm: false,
    //    confirmButtonText: ButtonsText.Approved,
    //    cancelButtonText: ButtonsText.Cancel,
    //    customClass: {
    //        confirmButton: "btn btn-info px-3 mx-2",
    //        cancelButton: "btn btn-primary px-3 mx-2"
    //    },
    //    buttonsStyling: false
    //});

    //if (!result.isConfirmed) {
    //    return;
    //}

    try {

        let url = `/exchange/quotation/Approved?id=${id}`;

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
            if (typeNumeral == QuotationType.Buy) {
                pending = parseFloat(amountHeader) - total;
            } else if (typeNumeral == QuotationType.Sell) {
                pending = parseFloat(amountHeader * fnparseFloat(TCHeader.value)) - (total);
            }

            if (pending == 0) {
                document.querySelector("#btnCreateDetailDeposit").hidden = true;
            } else {
                document.querySelector("#btnCreateDetailDeposit").hidden = false;
            }
            tableRowLabelDeposit.innerHTML = `Depositar: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
            tableRowLabelDeposit.value = `Depositar: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
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
            if (typeNumeral == QuotationType.Buy) {
                pending = parseFloat(amountHeader * fnparseFloat(TCHeader.value)) - (total);
            } else if (typeNumeral == QuotationType.Sell) {
                pending = parseFloat(amountHeader) - total;
            }

            if (pending == 0) {
                document.querySelector("#btnCreateDetailTransfer").hidden = true;
            } else {
                document.querySelector("#btnCreateDetailTransfer").hidden = false;

            }
            tableRowLabelTransfer.innerHTML = `Transferir: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
            tableRowLabelTransfer.value = `Transferir: ${formatterAmount().format(total)}  -  Pendiente: ${formatterAmount().format(pending)}`;
            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });
}
