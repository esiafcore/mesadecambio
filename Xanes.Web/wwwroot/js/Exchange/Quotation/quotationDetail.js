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
let indexDataTableBankSourceTransfer = 0, indexDataTableBankTargetTransfer = 0, indexDataTableBankSourceDeposit = 0, divExchangeRateVariation, inputExchangeRateVariation, btnQuotationClosed;
let isPendingDeposit = true, isPendingTransfer = true, pendingTransfer, pendingDeposit;
let totalDeposit, totalTransfer;
let btnAdjustmentTransfer, btnAdjustmentDeposit;


//Variable para el color de fondo seleccionado en la tablas del detalle
let bgRow = 'selectedinfo';
document.addEventListener("DOMContentLoaded", async function () {
    btnAdjustmentTransfer = document.querySelector("#btnAdjustmentTransfer");
    btnAdjustmentDeposit = document.querySelector("#btnAdjustmentDeposit");
    currencies = document.querySelectorAll(".currenciesTransa");
    currenciesDeposit = document.querySelectorAll(".currenciesDeposit");
    currenciesTransfer = document.querySelectorAll(".currenciesTransfer");
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
    divExchangeRateVariation = document.querySelector("#divExchangeRateVariation");
    inputExchangeRateVariation = document.querySelector("#exchangeRateVariation");
    btnQuotationClosed = document.querySelector("#btnQuotationClosed");

    let selectBusinessExecutive = document.querySelector("#selectBusinessExecutive");
    $(selectBusinessExecutive).select2(select2Options);
    //Obtenemos el tipo de cambio en base a la fecha
    await fnTCByDate();

    //Funcion para obtener el valor de las monedas seleccionadas
    fnGetCurrencyValues();

    //Funcion que formatea los inputs transaccionales y inputs de T/Cambio
    fnFormatInputValues();

    typeNumerals.forEach((item) => {
        if (item.checked) {
            typeNumeral = item.value;
        }
    });

    fnInitializeSelectCustomer();

    //Setear enfoque en el search input
    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });

    //Evento del cambio en la fecha, para obtener el tc/Oficial
    inputDateTransa.addEventListener('change', async () => {
        await fnTCByDate();
    });

    //Si el usuario cambio el monto o el tc transaccional, calculamos nuevamente
    document.querySelectorAll("#amountTransa, #exchangeRateSellTransa, #exchangeRateBuyTransa").forEach((item) => item.addEventListener("change", () => {
        fnCalculateRevenueCost();
    }));

    await fnLoadInputsByType(typeNumeral);
    fnCalculateRevenueCost();
    await fnGetCustomer();


    $(selectBankAccountSource).select2(select2Options);
    $(selectBankAccountTarget).select2(select2Options);

    fnLoadDatatableDeposit();
    fnLoadDatatableTransfer();
    fnLoadDatatableBankDeposit();
    fnLoadDatatableBankTransferSource();
    fnLoadDatatableBankTransferTarget();
    fnChangeSelectDataTableDetails();

    // Evento enviar form para crear
    const formDetailDeposit = document.getElementById("formDetailDeposit");
    formDetailDeposit.addEventListener("submit", fnCreateDetailFormSubmit);
    fnEnableTooltip();

    // Evento enviar form para crear
    const formDetailTransfer = document.getElementById("formDetailTransfer");
    formDetailTransfer.addEventListener("submit", fnCreateDetailFormSubmit);

    // Evento enviar form para actualizar la cabecera
    const formUpdateHeader = document.getElementById("formUpdateHeader");
    formUpdateHeader.addEventListener("submit", fnCreateDetailFormSubmit);
    fnEnableTooltip();


    btnAdjustmentDeposit.addEventListener("click", fnAdjustmentExchange);
    btnAdjustmentTransfer.addEventListener("click", fnAdjustmentExchange);

});

const fnInitializeSelectCustomer = () => {
    if (selectCustomer) {
        $(selectCustomer).select2({
            language: customMessagesSelect,
            allowClear: true,
            placeholder: ACJS.PlaceHolderSelect,
            theme: "bootstrap-5",
            selectionCssClass: "select2--small",
            dropdownCssClass: "select2--small",
            dropdownParent: $('#modalUpdateHeader'),
            width: '100%'
        });
    }
};

const fnCreateDetailFormSubmit = async (event) => {

    try {
        fntoggleLoading();

        event.preventDefault();
        const formObject = event.currentTarget;

        const url = `${formObject.action}`;
        const formData = new FormData(formObject);
        //for (let [key, value] of formData.entries()) {
        //    console.log(`${key}: ${value}`);
        //}
        const response = await fetch(url, {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            const errorMessage = await response.json();
            Swal.fire({
                icon: 'error',
                text: errorMessage.errorMessages
            });
        } else {
            const jsonResponse = await response.json();
            if (!jsonResponse.isSuccess) {
                Swal.fire({
                    icon: 'error',
                    text: jsonResponse.errorMessages
                });
            } else {
                if (jsonResponse.urlRedirect) {
                    window.location.href = jsonResponse.urlRedirect;
                }
            }
        }

    } catch (error) {
        Swal.fire({
            icon: 'error',
            text: error
        });

    } finally {
        fntoggleLoading();
    }
}

const fnChangeSelectDataTableDetails = () => {
    dataTableBankSourceDeposit.on('select', async function (e, dt, type, indexes) {
        if (type === 'row') {
            inputBankSourceDeposit.value = await fnGetBankId(dataTableBankSourceDeposit);
            inputBankTargetDeposit.value = inputBankSourceDeposit.value;

            let selectedRowAfter = dataTableBankSourceDeposit.row(indexDataTableBankSourceDeposit);
            let selectedRowNodeAfter = selectedRowAfter.node();
            $(selectedRowNodeAfter).removeClass(bgRow);

            indexDataTableBankSourceDeposit = indexes;

            // Selecciona la fila deseada
            let selectedRow = dataTableBankSourceDeposit.row(indexes);
            let selectedRowNode = selectedRow.node();
            $(selectedRowNode).addClass(bgRow);
        }
    });
    dataTableBankSourceTransfer.on('select', async function (e, dt, type, indexes) {
        if (type === 'row') {
            inputBankSourceTransfer.value = await fnGetBankId(dataTableBankSourceTransfer);
            let selectedRowAfter = dataTableBankSourceTransfer.row(indexDataTableBankSourceTransfer);
            let selectedRowNodeAfter = selectedRowAfter.node();
            $(selectedRowNodeAfter).removeClass(bgRow);

            indexDataTableBankSourceTransfer = indexes;

            // Selecciona la fila deseada
            let selectedRow = dataTableBankSourceTransfer.row(indexes);
            let selectedRowNode = selectedRow.node();
            $(selectedRowNode).addClass(bgRow);
        }
    });
    dataTableBankTargetTransfer.on('select', async function (e, dt, type, indexes) {
        if (type === 'row') {
            inputBankTargetTransfer.value = await fnGetBankId(dataTableBankTargetTransfer);
            let selectedRowAfter = dataTableBankTargetTransfer.row(indexDataTableBankTargetTransfer);
            let selectedRowNodeAfter = selectedRowAfter.node();
            $(selectedRowNodeAfter).removeClass(bgRow);

            indexDataTableBankTargetTransfer = indexes;

            // Selecciona la fila deseada
            let selectedRow = dataTableBankTargetTransfer.row(indexes);
            let selectedRowNode = selectedRow.node();
            $(selectedRowNode).addClass(bgRow);
        }
    });
};

const fnFormatInputValues = () => {
    inputsFormatTransa.forEach((item) => {
        item.value = formatterAmount().format(fnparseFloat(item.value));

        item.addEventListener("change", (event) => {
            if (event.target === amountDeposit || event.target === amountTransfer) {
                item.value = formatterAmount().format(fnparseFloat(item.value, true));
            } else {
                item.value = formatterAmount().format(fnparseFloat(item.value));
            }
        });
    });

    inputsFormatExchange.forEach((item) => {
        item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value));

        item.addEventListener("change", () => {
            item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value));
        });
    });
};

const fnGetCurrencyValues = () => {
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
}

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
                option.value = item.id;
                option.text = item.businessName;
                option.setAttribute("data-executive", item.businessExecutiveId);
                selectCustomer.appendChild(option);
            });

            $(selectCustomer).select2(select2Options);
            let options = selectCustomer.getElementsByTagName('option');
            if (options.length > 0) {
                // Selecciona el primer elemento
                selectCustomer.value = options[0].value;
            }

        } else {
            Swal.fire({
                icon: 'error',
                text: jsonResponse.errorMessages
            });
        }

    } catch (error) {
        Swal.fire({
            icon: 'error',
            text: error
        });
    }
};

const fnGetCustomer = async () => {
    try {
        if (selectCustomer) {
            fnInitializeSelectCustomer();
            $(selectCustomer).on('select2:open', async function (e) {

                let searchField = document.querySelector(".select2-search__field");
                searchField.focus();
                searchField.addEventListener('keyup', async function (event) {
                    if (event.key === 'Enter' || event.key === ' ') {
                        // Capturar el valor del campo de búsqueda
                        let searchTerm = searchField.value.replace(/-/g, "").trim();
                        if (searchTerm === "") return;
                        let onlyCompanies;

                        if (typeNumeral == QuotationType.Buy || typeNumeral == QuotationType.Sell) {
                            onlyCompanies = false;
                        } else {
                            onlyCompanies = true;
                        }
                        let url = `/Exchange/Quotation/GetCustomerByContain?search=${searchTerm}&onlyCompanies=${onlyCompanies}`;

                        const response = await fetch(url, {
                            method: "POST"
                        });

                        const jsonData = await response.json();

                        fntoggleLoading();

                        if (jsonData.isSuccess) {
                            selectCustomer.innerHTML = "";
                            // Agregar options
                            let option = document.createElement("option");
                            option.value = "";
                            option.text = ACJS.PlaceHolderSelect;
                            option.disabled = true;
                            option.selected = true;

                            selectCustomer.insertBefore(option, selectCustomer.firstChild);

                            jsonData.data.forEach((item) => {
                                let option = document.createElement("option");
                                option.value = item.id;
                                option.text = item.businessName;
                                option.setAttribute("data-executive", item.businessExecutiveId);
                                selectCustomer.appendChild(option);
                            });

                            fnInitializeSelectCustomer();

                            // Abrir Select2 nuevamente
                            $(selectCustomer).select2('open');
                            searchField.focus();
                            document.querySelector(".select2-search__field").value = searchTerm;
                        }

                        fntoggleLoading();

                        let options = selectCustomer.getElementsByTagName('option');
                        if (options.length > 0) {
                            // Selecciona el primer elemento
                            selectCustomer.value = options[0].value;
                        }

                        $(selectCustomer).on('select2:select', async function (e) {
                            var executiveId = $(e.params.data.element).data('executive');
                            if (executiveId != 0)
                                $(selectBusinessExecutive).val(executiveId).trigger('change');
                        });

                        $(selectCustomer).select2('focus');

                    }
                });
            });
        }

    } catch (error) {
        Swal.fire({
            icon: 'error',
            text: error
        });
    }
};

const fnLoadInputsByType = async (type) => {
    if (type == QuotationType.Buy) {
        divCurrencyTransa.hidden = false;
        divAmountExchange.hidden = false;
        divCommission.hidden = true;
        divCurrencyTransfer.hidden = false;
        divExchangeRateVariation.hidden = false;
        divCurrencyDeposit.hidden = true;
        elementsBuy.forEach((item) => item.hidden = false);
        elementsSell.forEach((item) => item.hidden = true);
        elementsTransfer.forEach((item) => item.hidden = true);
        await fnGetCustomer();
        fnCalculateRevenueCost();

    } else if (type == QuotationType.Sell) {
        divCurrencyTransa.hidden = false;
        divAmountExchange.hidden = false;
        divCommission.hidden = true;
        divCurrencyTransfer.hidden = true;
        divExchangeRateVariation.hidden = false;
        divCurrencyDeposit.hidden = false;
        elementsBuy.forEach((item) => item.hidden = true);
        elementsSell.forEach((item) => item.hidden = false);
        elementsTransfer.forEach((item) => item.hidden = true);
        await fnGetCustomer();
        fnCalculateRevenueCost();

    } else {
        divAmountExchange.hidden = true;
        divCurrencyTransa.hidden = true;
        divCurrencyTransfer.hidden = true;
        divCurrencyDeposit.hidden = true;
        divExchangeRateVariation.hidden = true;
        divCommission.hidden = false;
        elementsBuy.forEach((item) => item.hidden = true);
        elementsSell.forEach((item) => item.hidden = true);
        elementsTransfer.forEach((item) => item.hidden = false);
        await fnGetCustomer();
    }
};
function formatOption(option) {
    if (!option.id) {
        return option.text;
    }

    let thumbnailUrl = option.element.dataset.thumbnail;

    if (thumbnailUrl) {
        let $option = $(
            '<span><img src="' + thumbnailUrl + '" class="img-thumbnail img-fluid" style="width: 80px; height: 40px; margin-right: 10px;" />' + option.text + '</span>'
        );
    } else {
        let $option = $('<span>' + option.text + '</span>');
    }

    return $option;
}

//Funcion para calcular el costo
const fnCalculateRevenueCost = () => {
    let divRevenue = document.querySelector("#divRevenue");
    let divCost = document.querySelector("#divCost");
    let amountExchange = document.querySelector("#amountExchange");
    let exchangeRateSellTransa = 0;
    let exchangeRateOfficialTransa = fnparseFloat(inputExchangeRateOfficialTransa.value);
    let amountTransa = fnparseFloat(inputAmountTransa.value);
    let amountCost = 0, amountRevenue = 0;
    let exchangeRateBuyTransa = 0;
    let exchangeRateVariation = fnparseFloat(inputExchangeRateVariation.value);

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
        exchangeRateVariation = exchangeRateOfficialTransa - exchangeRateBuyTransa;
        inputExchangeRateVariation.value = formatterAmount(decimalExchange).format(exchangeRateVariation);
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
                amountExchange.value = formatterAmount().format(amountTransa * exchangeRateSellTransa);
            } else if (currencyType == CurrencyType.Additional) {
                if (currencyTypeDeposit == CurrencyType.Base) {
                    amountExchange.value = formatterAmount().format(amountTransa * exchangeRateSellTransa);
                } else {
                    amountExchange.value = formatterAmount().format(amountTransa / exchangeRateSellTransa);
                }
            }
        }
        exchangeRateVariation = exchangeRateSellTransa - exchangeRateOfficialTransa;
        inputExchangeRateVariation.value = formatterAmount(decimalExchange).format(exchangeRateVariation);
    }
    inputExchangeRateSellTransa.value = formatterAmount(decimalExchange).format(exchangeRateSellTransa);
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

    } catch (e) {
        Swal.fire({
            icon: 'error',
            title: "Error en la conexión",
            text: e
        });
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
    fnmakeModalDraggable("modalUpdateHeader");
};

const fnShowModalDeposit = async () => {
    fnClearModalDeposit();
    document.querySelector("#staticBackdropLabelDeposit").innerHTML = "Nuevo Deposito";
    document.querySelector("#infoModalDeposit").innerHTML = tableRowLabelDeposit.value;
    await fnSelectTableSourceDeposit();
    //fnLoadDatatableBankDeposit();
    // Mostrar el modal
    $('#modalCreateDeposit').modal('show');
};

//Funcion para obtener el banco seleccionado en base al datatable
const fnGetBankId = async (table) => {
    let dataRow = table.rows({ selected: true }).data().toArray();
    let id;
    dataRow.forEach(async (item) => {
        id = item.id;
    });
    return id;
};

//Funcion para mostrar el modal de transferencia
const fnShowModalTransfer = () => {
    fnClearModalTransfer();
    document.querySelector("#staticBackdropLabelTransfer").innerHTML = "Nueva Transferencia";
    document.querySelector("#infoModalTransfer").innerHTML = tableRowLabelTransfer.value;
    fnLoadDatatableBankTransferSource();
    fnLoadDatatableBankTransferTarget();
    $('#modalCreateTransfer').modal('show');
};

//Funcion para eliminar los detalles
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
            //toastr.success(jsonResponse.successMessages);
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

//Funcion para cerrar la cotizacion
const fnClosed = async (id, isReclosed = false) => {

    try {
        fntoggleLoading();

        let url = `/exchange/quotation/Closed?id=${id}&isReclosed=${isReclosed}`;

        const response = await fetch(url, {
            method: 'POST'
        });

        const jsonResponse = await response.json();
        if (!jsonResponse.isSuccess) {

            if (jsonResponse.titleMessages == "") {
                titulo = "Error";
            }

            await fnShowModalMessages(jsonResponse, jsonResponse.titleMessages);
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
    } finally {
        fntoggleLoading();
    }
};

const fnupdateRow = async (id, amount, bankSource, bankTarget, quotationDetailType) => {
    if (quotationDetailType == QuotationDetailType.Deposit) {
        document.querySelector("#staticBackdropLabelDeposit").innerHTML = "Actualizar Deposito";
        idDetailDeposit.value = id;

        let rowDataToSelect = null;
        dataTableBankSourceDeposit.rows().every(function (index, element) {
            let rowData = this.data();
            if (rowData.id === bankSource) {
                rowDataToSelect = index;
                return false;
            }
        });

        // Verifica si se encontró la fila
        if (rowDataToSelect !== null) {
            // Selecciona la fila en la tabla
            await fnSelectTableSourceDeposit(rowDataToSelect);
        }

        amountDeposit.value = formatterAmount().format(amount);
        document.querySelector("#infoModalDeposit").innerHTML = tableRowLabelDeposit.value;
        $('#modalCreateDeposit').modal('show');
    } else {
        document.querySelector("#staticBackdropLabelTransfer").innerHTML = "Actualizar Transferencia";
        idDetailTransfer.value = id;
        amountTransfer.value = formatterAmount().format(amount);

        let rowDataToSelectSource = null;
        dataTableBankSourceTransfer.rows().every(function (index, element) {
            let rowData = this.data();
            if (rowData.id === bankSource) {
                rowDataToSelectSource = index; // Asigna el índice de la fila 
                return false;
            }
        });

        // Verifica si se encontró la fila
        if (rowDataToSelectSource !== null) {
            // Selecciona la fila en la tabla
            fnLoadDatatableBankTransferSource(rowDataToSelectSource);
        }

        let rowDataToSelectTarget = null;
        dataTableBankTargetTransfer.rows().every(function (index, element) {
            let rowData = this.data();
            if (rowData.id === bankTarget) {
                rowDataToSelectTarget = index;
                return false;
            }
        });

        // Verifica si se encontró la fila
        if (rowDataToSelectTarget !== null) {
            // Selecciona la fila en la tabla
            fnLoadDatatableBankTransferTarget(rowDataToSelectTarget);
        }

        document.querySelector("#infoModalTransfer").innerHTML = tableRowLabelTransfer.value;
        $('#modalCreateTransfer').modal('show');
    }
};

//Funcion para verificar si se puede cerrar o no
const fnVerificateIsPending = () => {
    if (isPendingDeposit == false && isPendingTransfer == false) {
        btnQuotationClosed.hidden = false;
    } else {
        btnQuotationClosed.hidden = true;
    }
};

//Funcion para asignar el pendiente al monto del detalle en deposito
const fnBtnAddPendingDeposit = () => {
    amountDeposit.value = formatterAmount().format(fnparseFloat(pendingDeposit));
};

//Funcion para asignar el pendiente al monto del detalle en transferencia
const fnBtnAddPendingTransfer = () => {
    amountTransfer.value = formatterAmount().format(fnparseFloat(pendingTransfer));

};

const fnSelectTableSourceDeposit = async (index = 0) => {

    let selectedRowAfter = dataTableBankSourceDeposit.row(indexDataTableBankSourceDeposit);
    indexDataTableBankSourceDeposit = index;
    let selectedRowNodeAfter = selectedRowAfter.node();
    $(selectedRowNodeAfter).removeClass(bgRow);

    let selectedRow = dataTableBankSourceDeposit.row(index);

    dataTableBankSourceDeposit.rows().deselect();

    // Selecciona la fila deseada
    selectedRow.select();

    let selectedRowNode = selectedRow.node();

    $(selectedRowNode).addClass(bgRow);
};

const fnSelectTableSourceTransfer = async (index = 0) => {

    let selectedRowAfter = dataTableBankSourceTransfer.row(indexDataTableBankSourceTransfer);
    indexDataTableBankSourceTransfer = index;
    let selectedRowNodeAfter = selectedRowAfter.node();
    $(selectedRowNodeAfter).removeClass(bgRow);

    let selectedRow = dataTableBankSourceTransfer.row(index);

    dataTableBankSourceTransfer.rows().deselect();

    // Selecciona la fila deseada
    selectedRow.select();

    let selectedRowNode = selectedRow.node();

    $(selectedRowNode).addClass(bgRow);
};

const fnSelectTableTargetTransfer = async (index = 0) => {

    let selectedRowAfter = dataTableBankTargetTransfer.row(indexDataTableBankTargetTransfer);
    indexDataTableBankTargetTransfer = index;
    let selectedRowNodeAfter = selectedRowAfter.node();
    $(selectedRowNodeAfter).removeClass(bgRow);

    let selectedRow = dataTableBankTargetTransfer.row(index);

    dataTableBankTargetTransfer.rows().deselect();

    // Selecciona la fila deseada
    selectedRow.select();

    let selectedRowNode = selectedRow.node();

    $(selectedRowNode).addClass(bgRow);
};

//*********************** Funciones de los datatables *****************************
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
                await fnSelectTableSourceTransfer(index);
            }
        },
        "columns": [
            {
                data: null, "width":
                    "30%", orderable: false,
                "className": "text-center align-middle",
                "render": (data) => {
                    if (data.logoUrl != "" && data.logoUrl != null) {
                        return `<span class="m-0 p-0"><img src="${data.logoUrl
                            }" class="img-thumbnail img-fluid" style="width: 80px; height: 40px; margin-right: 10px;" /></span> `;
                    }
                    return "";
                }
            },
            {
                data: 'code',
                orderable: false,
                "width": "70%"
            }
        ],
        "searching": false,
        "paging": false,
        "autoWidth": false,
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
                await fnSelectTableTargetTransfer(index);
            }
        },
        "columns": [
            {
                data: null, "width":
                    "30%", orderable: false,
                "className": "text-center align-middle",
                "render": (data) => {
                    if (data.logoUrl != "" && data.logoUrl != null) {
                        return `<span class="m-0 p-0"><img src="${data.logoUrl
                            }" class="img-thumbnail img-fluid" style="width: 80px; height: 40px; margin-right: 10px;" /></span> `;
                    }
                    return "";
                }
            },
            {
                data: 'code',
                orderable: false,
                "width": "70%"
            }
        ],
        "searching": false,
        "paging": false,
        "autoWidth": false,
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
                await fnSelectTableSourceDeposit(index);
            }
        },
        "columns": [
            {
                data: null,
                "width":
                    "30%",
                orderable: false,
                "className": "text-center align-middle",
                "render": (data) => {
                    if (data.logoUrl != "" && data.logoUrl != null) {
                        return `<span class="m-0 p-0"><img src="${data.logoUrl
                            }" class="img-thumbnail img-fluid" style="width: 80px; height: 40px; margin-right: 10px;" /></span> `;
                    }
                    return "";
                }
            },
            {
                data: 'code',
                orderable: false,
                "width": "70%"
            }
        ],
        "searching": false,
        "paging": false,
        "autoWidth": false,
        "ordering": false,
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
                fnVerificateIsPending();
                if (typeNumeral == QuotationType.Sell) {
                    fnVerificateAjustment(pendingDeposit);
                }
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
            },
            {
                data: null, "width": "5%", orderable: false
                , render: (data, type, row) => {
                    return `<div class="btn-group" role="group">        
                        <a class="btn btn-primary py-1 px-3 my-0 mx-1"
                         onclick="fnupdateRow(${data.id}, ${data.amountDetail}, ${data.bankSourceId}, ${data.bankTargetId}, '${QuotationDetailType.Deposit}')"
                           data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a class="btn btn-danger py-1 px-3 my-0 mx-1"
                            onclick="fndeleteRow(${data.id})"
                           data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Eliminar">
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
                isPendingDeposit = false;
            }

            if (typeNumeral != QuotationType.Transport) {
                if (pending.toFixed(decimalTransa) == 0) {
                    document.querySelector("#btnCreateDetailDeposit").hidden = true;
                    isPendingDeposit = false;
                } else {
                    document.querySelector("#btnCreateDetailDeposit").hidden = false;
                    isPendingDeposit = true;
                }

            }

            tableRowLabelDeposit.innerHTML =
                `${label}: ${formatterAmount().format(fnparseFloat(deposit))}  -  Pendiente: ${formatterAmount().format(pending)
                }`;
            tableRowLabelDeposit.value =
                `${label}: ${formatterAmount().format(fnparseFloat(deposit))}  -  Pendiente: ${formatterAmount().format(pending)
                }`;

            pendingDeposit = pending;
            totalDeposit = total;

            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });

    if (typeNumeral == QuotationType.Transport) {
        dataTableDeposit.column(3).visible(false);
    }
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
                fnVerificateIsPending();
                if (typeNumeral == QuotationType.Buy) {
                    fnVerificateAjustment(pendingTransfer);
                }
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
            },
            {
                data: null, "width": "5%", orderable: false
                , render: (data, type, row) => {
                    return `<div class="btn-group" role="group">        
                        <a class="btn btn-primary py-1 px-3 my-0 mx-1"
                            onclick="fnupdateRow(${data.id}, ${data.amountDetail}, ${data.bankSourceId}, ${data.bankTargetId},'${QuotationDetailType.Transfer}')"
                           data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a class="btn btn-danger py-1 px-3 my-0 mx-1"
                            onclick="fndeleteRow(${data.id})"
                           data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Eliminar">
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
                if (pending.toFixed(decimalTransa) == 0) {
                    document.querySelector("#btnCreateDetailTransfer").hidden = true;
                    isPendingTransfer = false;
                } else {
                    document.querySelector("#btnCreateDetailTransfer").hidden = false;
                    isPendingTransfer = true;
                }

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
                isPendingTransfer = false;
            }

            tableRowLabelTransfer.innerHTML =
                `${label}: ${formatterAmount().format(fnparseFloat(transfer))}  -  Pendiente: ${formatterAmount().format(pending)
                }`;
            tableRowLabelTransfer.value =
                `${label}: ${formatterAmount().format(fnparseFloat(transfer))}  -  Pendiente: ${formatterAmount().format(pending)
                }`;
            pendingTransfer = pending;
            totalTransfer = total;
            $(footerCell).html(`${formatterAmount().format(total)}`);
        }
    });

    if (typeNumeral == QuotationType.Transport) {
        dataTableTransfer.column(2).visible(false);
        dataTableTransfer.column(4).visible(false);
    }
}

const fnVerificateAjustment = (pendiente) => {

    if (Math.abs(pendiente) <= variationMaxDeposit) {
        if (typeNumeral == QuotationType.Sell && pendiente != 0) {
            btnAdjustmentDeposit.hidden = false;
        } else {
            btnAdjustmentDeposit.hidden = true;
        }
        if (typeNumeral == QuotationType.Buy && pendiente != 0) {
            btnAdjustmentTransfer.hidden = false;
        } else {
            btnAdjustmentTransfer.hidden = true;
        }
    }
};

const fnAdjustmentExchange = async () => {
    let total = typeNumeral == QuotationType.Buy ? fnparseFloat(totalTransfer) : fnparseFloat(totalDeposit);
    let TCNew = total / fnparseFloat(amountHeader);
    let result = await Swal.fire({
        title: `&#191;Está seguro de ajustar la transacción?`,
        html: `T/C Actual: ${formatterAmount(decimalExchange).format(TCHeader.value)} - Nuevo T/C: ${formatterAmount(decimalExchangeFull).format(TCNew)}`,
        icon: "warning",
        showCancelButton: true,
        reverseButtons: true,
        focusConfirm: false,
        confirmButtonText: ButtonsText.Adjustment,
        cancelButtonText: ButtonsText.Cancel,
        customClass: {
            confirmButton: "btn btn-info px-3 mx-2",
            cancelButton: "btn btn-danger px-3 mx-2"
        },
        buttonsStyling: false
    });

    if (!result.isConfirmed) {
        return;
    }



    let url = `/Exchange/Quotation/AdjustmentExchange?parentId=${parentId}`;
    try {
        const response = await fetch(url, {
            method: "POST"
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
    }
    catch (error) {
        Swal.fire({
            icon: 'error',
            text: error
        });

    }



};

