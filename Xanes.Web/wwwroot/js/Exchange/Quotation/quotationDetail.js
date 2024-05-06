let containerMain;
let dataTableTransfer, dataTableDeposit, parentId, amountTotalDeposit = 0, amountTotalTransfer = 0;
let tableRowLabelTransfer, tableRowLabelDeposit, amountHeader;
let inputBankSourceDeposit, inputBankTargetDeposit, inputBankSourceTransfer, inputBankTargetTransfer,
    amountDeposit, amountTransfer, idDetailDeposit, idDetailTransfer, TCHeader;
let inputExchangeRateBuyTransa, inputExchangeRateSellTransa, inputExchangeRateOfficialTransa;
let inputsFormatTransa, inputsFormatExchange, inputAmountTransa, inputDateTransa, currencies, currencyType, typeNumeral, typeNumerals, amountExchangeDetail;
let selectCustomer, divCurrencyTransa, divAmountExchange, divCommission, divCurrencyTransfer, divCurrencyDeposit, elementsTransfer, elementsSell, elementsBuy,
    selectBankAccountSource, selectBankAccountTarget, inputAmountCost, inputAmountRevenue, currencyTypeDeposit, currencyTypeTransfer, currenciesTransa, currenciesDeposit, currenciesTransfer;
let dataTableBankSourceDeposit, dataTableBankSourceTransfer, dataTableBankTargetTransfer;
let indexDataTableBankSourceTransfer, indexDataTableBankTargetTransfer;
document.addEventListener("DOMContentLoaded", async function () {
    currencies = document.querySelectorAll(".currenciesTransa");
    currenciesDeposit = document.querySelectorAll(".currenciesTransfer");
    currenciesTransfer = document.querySelectorAll(".currenciesDeposit");
    inputDateTransa = document.querySelector("#dateTransa");
    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid";
    parentId = document.querySelector("#parentId").value;
    amountHeader = document.querySelector("#amountHeader").value;
    tableRowLabelDeposit = document.querySelector("#tableRowLabelDeposit");
    tableRowLabelTransfer = document.querySelector("#tableRowLabelTransfer");
    inputBankSourceDeposit = document.querySelector("#bankSourceDeposit");
    inputBankTargetDeposit = document.querySelector("#bankTargetDeposit");
    inputBankSourceTransfer = document.querySelector("#bankSourceTransfer");
    inputBankTargetTransfer = document.querySelector("#bankTargetTransfer");
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
    amountExchangeDetail = document.querySelector("#amountExchangeDetail");
    typeNumerals = document.querySelectorAll(".typeNumerals");
    selectCustomer = document.querySelector("#selectCustomer");
    divCurrencyTransa = document.querySelector("#divCurrencyTransa");
    divAmountExchange = document.querySelector("#divAmountExchange");
    divCommission = document.querySelector("#divCommission");
    divCurrencyDeposit = document.querySelector("#divCurrencyDeposit");
    divCurrencyTransfer = document.querySelector("#divCurrencyTransfer");
    elementsBuy = document.querySelectorAll(".typeBuy");
    elementsTransfer = document.querySelectorAll(".typeTransfer");
    elementsSell = document.querySelectorAll(".typeSell");
    selectBankAccountSource = document.querySelector("#selectBankAccountSource");
    selectBankAccountTarget = document.querySelector("#selectBankAccountTarget");
    inputAmountCost = document.querySelector("#amountCost");
    inputAmountRevenue = document.querySelector("#amountRevenue");

    currencies.forEach((item) => {
        if (item.checked) {
            currencyType = item.value;
        }
    });

    currenciesDeposit.forEach((item) => {
        if (item.checked) {
            currencyTypeDeposit = parseInt(item.value);
        }
    });

    currenciesTransfer.forEach((item) => {
        if (item.checked) {
            currencyTypeTransfer = parseInt(item.value);
        }
    });


    typeNumerals.forEach((item) => {
        if (item.checked) {
            typeNumeral = item.value;
        }
    });

    fnLoadInputsByType(typeNumeral);

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
    $(selectBankAccountSource).select2(select2Options);
    $(selectBankAccountTarget).select2(select2Options);

    fnLoadDatatableDeposit();
    fnLoadDatatableTransfer();
    fnLoadDatatableBankDeposit();
    fnLoadDatatableBankTransferSource();
    fnLoadDatatableBankTransferTarget();

    dataTableBankSourceDeposit.on('select', async function (e, dt, type, indexes) {
        if (type === 'row') {
            inputBankSourceDeposit.value = await fnGetBankId(dataTableBankSourceDeposit);
            inputBankTargetDeposit.value = inputBankSourceDeposit.value;
        }
    });

    dataTableBankSourceTransfer.on('select', async function (e, dt, type, indexes) {
        if (type === 'row') {
            inputBankSourceTransfer.value = await fnGetBankId(dataTableBankSourceTransfer);
            var selectedRowAfter = dataTableBankSourceTransfer.row(indexDataTableBankSourceTransfer);
            var selectedRowNodeAfter = selectedRowAfter.node();
            $(selectedRowNodeAfter).removeClass('bg-success bg-opacity-75 bg-gradient');

            indexDataTableBankSourceTransfer = indexes;

            // Selecciona la fila deseada
            var selectedRow = dataTableBankSourceTransfer.row(indexes);
            var selectedRowNode = selectedRow.node();
            $(selectedRowNode).addClass('bg-success bg-opacity-75 bg-gradient');
        }
    });

    dataTableBankTargetTransfer.on('select', async function (e, dt, type, indexes) {
        if (type === 'row') {
            inputBankTargetTransfer.value = await fnGetBankId(dataTableBankTargetTransfer);
            var selectedRowAfter = dataTableBankTargetTransfer.row(indexDataTableBankTargetTransfer);
            var selectedRowNodeAfter = selectedRowAfter.node();
            $(selectedRowNodeAfter).removeClass('bg-success bg-opacity-75 bg-gradient');

            indexDataTableBankTargetTransfer = indexes;

            // Selecciona la fila deseada
            var selectedRow = dataTableBankTargetTransfer.row(indexes);
            var selectedRowNode = selectedRow.node();
            $(selectedRowNode).addClass('bg-success bg-opacity-75 bg-gradient');
        }
    });
    //Habilitar Tooltip
    fnEnableTooltip();
});

const fnChangeCustomers = async (onlyCompanies) => {
    let url = `/Exchange/Quotation/GetCustomers?onlyCompanies=${onlyCompanies}`;

    try {

        const response = await fetch(url, {
            method: "POST"
        });

        const jsonResponse = await response.json();

        if (jsonResponse.isSuccess) {
            selectCustomer.innerHTML = "";

            jsonResponse.data.forEach((item) => {
                let option = document.createElement("option");
                option.value = item.value;
                option.text = item.text;
                selectCustomer.appendChild(option);
            });

            $(selectCustomer).select2(select2Options);
            var options = selectCustomer.getElementsByTagName('option');
            if (options.length > 0) {
                // Selecciona el primer elemento
                selectCustomer.value = options[0].value;
            }

        } else {
            alert(jsonResponse.errorMessages);
        }

    } catch (error) {
        alert(error);
    }
};

const fnLoadInputsByType = (type) => {
    if (type == QuotationType.Buy) {
        divCurrencyTransa.hidden = false;
        divAmountExchange.hidden = false;
        divCommission.hidden = true;
        divCurrencyTransfer.hidden = false;
        divCurrencyDeposit.hidden = true;
        elementsBuy.forEach((item) => item.hidden = false);
        elementsSell.forEach((item) => item.hidden = true);
        elementsTransfer.forEach((item) => item.hidden = true);
        fnChangeCustomers(false);
        fnCalculateRevenueCost();

    } else if (type == QuotationType.Sell) {
        divCurrencyTransa.hidden = false;
        divAmountExchange.hidden = false;
        divCommission.hidden = true;
        divCurrencyTransfer.hidden = true;
        divCurrencyDeposit.hidden = false;
        elementsBuy.forEach((item) => item.hidden = true);
        elementsSell.forEach((item) => item.hidden = false);
        elementsTransfer.forEach((item) => item.hidden = true);
        fnChangeCustomers(false);
        fnCalculateRevenueCost();

    } else {
        divAmountExchange.hidden = true;
        divCurrencyTransa.hidden = true;
        divCurrencyTransfer.hidden = true;
        divCurrencyDeposit.hidden = true;
        divCommission.hidden = false;
        elementsBuy.forEach((item) => item.hidden = true);
        elementsSell.forEach((item) => item.hidden = true);
        elementsTransfer.forEach((item) => item.hidden = false);
        fnChangeCustomers(true);
    }
};
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
    let exchangeRateSellTransa;
    let exchangeRateOfficialTransa = fnparseFloat(inputExchangeRateOfficialTransa.value);
    let amountTransa = fnparseFloat(inputAmountTransa.value);
    let amountCost = 0, amountRevenue = 0;
    let exchangeRateBuyTransa;

    if (typeNumeral == QuotationType.Buy) {
        exchangeRateBuyTransa = fnparseFloat(inputExchangeRateBuyTransa.value);
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
        exchangeRateSellTransa = fnparseFloat(inputExchangeRateSellTransa.value);
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
                amountExchange.value = formatterAmount().format(amountTransa / exchangeRateSellTransa);
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
};

const fnClearModalTransfer = () => {
    document.querySelector("#idDetailTransfer").value = 0;
    document.querySelector("#amountTransfer").value = formatterAmount().format(0);
};

const fnShowModalUpdateHeader = () => {
    $('#modalUpdateHeader').modal('show');
};

const fnShowModalDeposit = async () => {
    fnClearModalDeposit();
    document.querySelector("#staticBackdropLabelDeposit").innerHTML = "Nueva Cotización";
    document.querySelector("#infoModalDeposit").innerHTML = tableRowLabelDeposit.value;
    fnLoadDatatableBankDeposit();
    // Mostrar el modal
    $('#modalCreateDeposit').modal('show');
};

const fnGetBankId = async (table) => {
    let dataRow = table.rows({ selected: true }).data().toArray();
    let id;
    dataRow.forEach(async (item) => {
        id = item.id;
    });

    return id;
};

const fnShowModalTransfer = () => {
    fnClearModalTransfer();
    document.querySelector("#staticBackdropLabelTransfer").innerHTML = "Nueva Transferencia";
    document.querySelector("#infoModalTransfer").innerHTML = tableRowLabelTransfer.value;
    fnLoadDatatableBankTransferSource();
    fnLoadDatatableBankTransferTarget();
    $('#modalCreateTransfer').modal('show');

    //$('#modalCreateTransfer').on('shown.bs.modal', function () {
    //    // Inicializar Select2 en el elemento de selección
    //    $(selectBankSourceTransfer).select2({
    //        templateResult: formatOption,
    //        language: customMessagesSelect,
    //        allowClear: true,
    //        minimumResultsForSearch: -1, // Mostrar todos los elementos sin barra de búsqueda
    //        dropdownAutoWidth: true, // Ajustar automáticamente el ancho del menú desplegable
    //        dropdownParent: $('#modalCreateTransfer'),
    //        maximumSelectionLength: totalOptionsSource, // Establecer el tamaño máximo del menú desplegable
    //        openOnEnter: true
    //    });
    //    $(selectBankTargetTransfer).select2({
    //        templateResult: formatOption,
    //        language: customMessagesSelect,
    //        allowClear: true,
    //        minimumResultsForSearch: -1, // Mostrar todos los elementos sin barra de búsqueda
    //        dropdownAutoWidth: true, // Ajustar automáticamente el ancho del menú desplegable
    //        dropdownParent: $('#modalCreateTransfer'),
    //        maximumSelectionLength: totalOptionsTarget // Establecer el tamaño máximo del menú desplegable
    //    });
    //    $(selectBankSourceTransfer).select2('open');
    //    $(selectBankTargetTransfer).select2('open');
    //});
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

        // Busca la fila que contiene el ID especificado
        let rowDataToSelect = null;
        dataTableBankSourceDeposit.rows().every(function (index, element) {
            var rowData = this.data();
            if (rowData.id === bankSource) {
                rowDataToSelect = index; // Asigna el índice de la fila directamente
                return false; // Termina la iteración una vez que se encuentra la fila
            }
        });

        // Verifica si se encontró la fila
        if (rowDataToSelect !== null) {
            // Selecciona la fila en la tabla
            fnLoadDatatableBankDeposit(rowDataToSelect);
        }

        amountDeposit.value = formatterAmount().format(amount);
        document.querySelector("#infoModalDeposit").innerHTML = tableRowLabelDeposit.value;
        $('#modalCreateDeposit').modal('show');
    } else {
        document.querySelector("#staticBackdropLabelTransfer").innerHTML = "Actualizar Transferencia";
        idDetailTransfer.value = id;
        amountTransfer.value = formatterAmount().format(amount);

        // Busca la fila que contiene el ID especificado
        let rowDataToSelectSource = null;
        dataTableBankSourceTransfer.rows().every(function (index, element) {
            var rowData = this.data();
            if (rowData.id === bankSource) {
                rowDataToSelectSource = index; // Asigna el índice de la fila directamente
                return false; // Termina la iteración una vez que se encuentra la fila
            }
        });

        // Verifica si se encontró la fila
        if (rowDataToSelectSource !== null) {
            // Selecciona la fila en la tabla
            fnLoadDatatableBankTransferSource(rowDataToSelectSource);
        }

        // Busca la fila que contiene el ID especificado
        let rowDataToSelectTarget = null;
        dataTableBankTargetTransfer.rows().every(function (index, element) {
            var rowData = this.data();
            if (rowData.id === bankTarget) {
                rowDataToSelectTarget = index; // Asigna el índice de la fila directamente
                return false; // Termina la iteración una vez que se encuentra la fila
            }
        });

        // Verifica si se encontró la fila
        if (rowDataToSelectTarget !== null) {
            // Selecciona la fila en la tabla
            fnLoadDatatableBankTransferTarget(rowDataToSelectTarget);
        }

        //[...selectBankSourceTransfer.options].forEach((opt) => {
        //    if (opt.value == bankSource) {
        //        opt.selected = true;
        //        return;
        //    }
        //});
        //[...selectBankTargetTransfer.options].forEach((opt) => {
        //    if (opt.value == bankTarget) {
        //        opt.selected = true;
        //        return;
        //    }
        //});
        document.querySelector("#infoModalTransfer").innerHTML = tableRowLabelTransfer.value;
        $('#modalCreateTransfer').modal('show');
    }
};

function fnLoadDatatableBankTransferSource(index = 0) {
    if (dataTableBankSourceTransfer) dataTableBankSourceTransfer.destroy();
    dataTableBankSourceTransfer = new DataTable("#tblBankSourceTransfer", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/admin/bank/GetAll`,
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
            "complete": async function () {
                indexDataTableBankSourceTransfer = index;
                inputBankSourceTransfer.value = await fnGetBankId(dataTableBankSourceTransfer);
                // Selecciona la fila deseada
                var selectedRow = dataTableBankSourceTransfer.row(index);

                // Deselecciona todas las filas
                dataTableBankSourceTransfer.rows().deselect();

                // Selecciona la fila deseada
                selectedRow.select();

                // Obtiene el nodo HTML de la fila seleccionada
                var selectedRowNode = selectedRow.node();

                $(selectedRowNode).addClass('bg-success bg-opacity-75 bg-gradient');
            } 
        },
        "columns": [
            {
                data: null, "width":
                    "15%", orderable: false,
                "className": "text-center align-middle",
                "render": (data) => {
                    if (data.logoUrl != "" && data.logoUrl != null) {
                        return `<span class="m-0 p-0"><img src="${data.logoUrl
                            }" class="img-thumbnail img-fluid" style="width: 40px; height: 40px; margin-right: 10px;" /></span> `;
                    }
                    return "";
                }
            },
            {
                data: 'code',
                orderable: false,
                "width": "85%"
            }
        ],
        "searching": false,
        "paging": false,
        "select": {
            info: false,
            items: 'row',
            style: 'single',
            blurable: true,
            className: 'bg-info bg-opacity-75 bg-gradient'
        },
        "language": {
            info: "",
            infoEmpty: "",
            zeroRecords: "",
            processing: "Procesando...",
            loadingRecords: "Cargando...",
            emptyTable: ""
        }
    });
}
function fnLoadDatatableBankTransferTarget(index = 0) {
    if (dataTableBankTargetTransfer) dataTableBankTargetTransfer.destroy();
    dataTableBankTargetTransfer = new DataTable("#tblBankTargetTransfer", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/admin/bank/GetAll`,
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
            "complete": async function () {
                indexDataTableBankTargetTransfer = index;
                inputBankTargetTransfer.value = await fnGetBankId(dataTableBankTargetTransfer);
                // Selecciona la fila deseada
                var selectedRow = dataTableBankTargetTransfer.row(index);

                // Deselecciona todas las filas
                dataTableBankTargetTransfer.rows().deselect();

                // Selecciona la fila deseada
                selectedRow.select();

                // Obtiene el nodo HTML de la fila seleccionada
                var selectedRowNode = selectedRow.node();

                $(selectedRowNode).addClass('bg-success bg-opacity-75 bg-gradient');
            } 
        },
        "columns": [
            {
                data: null, "width":
                    "15%", orderable: false,
                "className": "text-center align-middle",
                "render": (data) => {
                    if (data.logoUrl != "" && data.logoUrl != null) {
                        return `<span class="m-0 p-0"><img src="${data.logoUrl
                            }" class="img-thumbnail img-fluid" style="width: 40px; height: 40px; margin-right: 10px;" /></span> `;
                    }
                    return "";
                }
            },
            {
                data: 'code',
                orderable: false,
                "width": "85%"
            }
        ],
        "searching": false,
        "paging": false,
        "select": {
            info: false,
            items: 'row',
            style: 'single',
            blurable: true,
            className: 'bg-info bg-opacity-75 bg-gradient'
        },
        "language": {
            info: "",
            infoEmpty: "",
            zeroRecords: "",
            processing: "Procesando...",
            loadingRecords: "Cargando...",
            emptyTable: ""
        }
    });
}
function fnLoadDatatableBankDeposit(index = 0) {
    if (dataTableBankSourceDeposit) dataTableBankSourceDeposit.destroy();
    dataTableBankSourceDeposit = new DataTable("#tblBankSourceDeposit", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/admin/bank/GetAll`,
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
            "complete": async function () {
                dataTableBankSourceDeposit.row(index).select();
                inputBankSourceDeposit.value = await fnGetBankId(dataTableBankSourceDeposit);
                inputBankTargetDeposit.value = inputBankSourceDeposit.value;
            }
        },
        "columns": [
            {
                data: null, "width":
                    "15%", orderable: false,
                "className": "text-center align-middle",
                "render": (data) => {
                    if (data.logoUrl != "" && data.logoUrl != null) {
                        return `<span class="m-0 p-0"><img src="${data.logoUrl
                            }" class="img-thumbnail img-fluid" style="width: 40px; height: 40px; margin-right: 10px;" /></span> `;
                    }
                    return "";
                }
            },
            {
                data: 'code',
                orderable: false,
                "width": "85%"
            }
        ],
        "searching": false,
        "paging": false,
        "select": {
            info: false,
            items: 'row',
            style: 'single',
            blurable: true,
            className: 'bg-info bg-opacity-75 bg-gradient'
        },
        "language": {
            info: "",
            infoEmpty: "",
            zeroRecords: "",
            processing: "Procesando...",
            loadingRecords: "Cargando...",
            emptyTable: ""
        }
    });
}
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
            info: "",
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
            let amount = 0;
            data.forEach((item) => {
                total += item.amountDetail;
            });
            if (typeNumeral == QuotationType.Buy) {
                pending = fnparseFloat(amountHeader) - total;
            } else if (typeNumeral == QuotationType.Sell) {
                amount = fnparseFloat(amountHeader) * fnparseFloat(TCHeader.value);
                pending = (amount.toFixed(decimalTransa)) - (total);
            }

            if (typeNumeral != QuotationType.Transfer) {
                if (pending == 0) {
                    document.querySelector("#btnCreateDetailDeposit").hidden = true;
                } else {
                    document.querySelector("#btnCreateDetailDeposit").hidden = false;
                }

                if (typeNumeral == QuotationType.Sell) {
                    if (currencyTypeDeposit == CurrencyType.Base) {
                        tableRowLabelDeposit.innerHTML =
                            `Depositar: ${formatterAmount().format(fnparseFloat(amount))}  -  Pendiente: ${formatterAmount().format(pending)}`;
                        tableRowLabelDeposit.value =
                            `Depositar: ${formatterAmount().format(fnparseFloat(amount))}  -  Pendiente: ${formatterAmount().format(pending)}`;
                    }
                } else {

                    tableRowLabelDeposit.innerHTML =
                        `Depositar: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)}`;
                    tableRowLabelDeposit.value =
                        `Depositar: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)}`;
                }

            } else {
                tableRowLabelDeposit.innerHTML = `TRC: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)}`;
                tableRowLabelDeposit.value = `TRC: ${formatterAmount().format(fnparseFloat(amountHeader))}  -  Pendiente: ${formatterAmount().format(pending)}`;
            }

            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });

    if (typeNumeral == QuotationType.Transfer) {
        dataTableDeposit.column(3).visible(false);
    }
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
            info: "",
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
            let total = 0, pending = 0;
            data.forEach((item) => {
                total += item.amountDetail;
            });
            if (typeNumeral == QuotationType.Buy) {
                amount = fnparseFloat(amountHeader) * fnparseFloat(TCHeader.value);
                pending = (amount.toFixed(decimalTransa)) - (total);
            } else if (typeNumeral == QuotationType.Sell) {
                if (currencyTypeDeposit == CurrencyType.Base) {
                    amount = (fnparseFloat(amountHeader) / fnparseFloat(TCHeader.value));
                    pending = amount.toFixed(decimalTransa) - total;
                } else {
                    pending = fnparseFloat(amountHeader) - total;
                }
            }

            if (typeNumeral != QuotationType.Transfer) {
                if (pending == 0) {
                    document.querySelector("#btnCreateDetailTransfer").hidden = true;
                } else {
                    document.querySelector("#btnCreateDetailTransfer").hidden = false;
                }

                tableRowLabelTransfer.innerHTML =
                    `Transferir: ${formatterAmount().format(fnparseFloat(amountExchangeDetail.value))}  -  Pendiente: ${formatterAmount().format(pending)
                    }`;
                tableRowLabelTransfer.value =
                    `Transferir: ${formatterAmount().format(fnparseFloat(amountExchangeDetail.value))}  -  Pendiente: ${formatterAmount().format(pending)
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
        dataTableTransfer.column(4).visible(false);
    }
}
