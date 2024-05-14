﻿let inputDateTransa, firstCurrency, inputAmountTransa, inputExchangeRateBuyTransa, inputExchangeRateSellTransa,
    inputExchangeRateOfficialTransa, inputAmountCost, inputAmountRevenue, currencyType, currencyTypeDeposit, currencyTypeTransfer, currenciesTransa, currenciesDeposit, currenciesTransfer,
    typeNumerals, inputCurrencyTransa, inputCurrencyDeposit, inputCurrencyTransfer, inputTypeNumeral, elementsBuy, elementsTransfer, elementsSell, divCurrencyTransfer, divCurrencyDeposit,
    divAmountExchange, divCommission, divCurrencyTransa;
let inputsFormatTransa, inputsFormatExchange;
let containerMain, selectCustomer, selectBankAccountTarget, selectBankAccountSource;
let elementsHiddenBuySell, divExchangeRateVariation, inputExchangeRateVariation, inputCommission;
let selectBusinessExecutive;

//Variables para los contenedores de los botones
let btnSave, btnNext, btnClosed, btnReClosed;
//Bton de salvar o siguiente
let btnSaveNext = document.querySelector("#btnSaveNext");

//Variables para saber si hay que redirigir al home o al detalle y mostrar el mensaje al crear
let redirectHome = false, redirectDetail = true, showMessages = true;
let isClosed = false, isReClosed = false, isClosedElement;

document.addEventListener("DOMContentLoaded", async () => {
    inputDateTransa = document.querySelector("#dateTransa");
    inputAmountTransa = document.querySelector("#amountTransa");
    inputExchangeRateBuyTransa = document.querySelector("#exchangeRateBuyTransa");
    inputExchangeRateSellTransa = document.querySelector("#exchangeRateSellTransa");
    inputExchangeRateOfficialTransa = document.querySelector("#exchangeRateOfficialTransa");
    inputAmountCost = document.querySelector("#amountCost");
    inputCommission = document.querySelector("#commission");
    inputAmountRevenue = document.querySelector("#amountRevenue");
    currenciesTransa = document.querySelectorAll(".currenciesTransa");
    currenciesDeposit = document.querySelectorAll(".currenciesDeposit");
    currenciesTransfer = document.querySelectorAll(".currenciesTransfer");
    typeNumerals = document.querySelectorAll(".typeNumerals");
    inputCurrencyTransa = document.querySelector("#currencyTransaType");
    inputCurrencyDeposit = document.querySelector("#currencyDepositType");
    inputCurrencyTransfer = document.querySelector("#currencyTransferType");
    inputTypeNumeral = document.querySelector("#typeNumeral");
    elementsBuy = document.querySelectorAll(".typeBuy");
    elementsTransfer = document.querySelectorAll(".typeTransfer");
    elementsSell = document.querySelectorAll(".typeSell");
    elementsHiddenBuySell = document.querySelectorAll(".typeBuySellhidden");
    divExchangeRateVariation = document.querySelector("#divExchangeRateVariation");
    inputExchangeRateVariation = document.querySelector("#exchangeRateVariation");
    divCurrencyDeposit = document.querySelector("#divCurrencyDeposit");
    divCurrencyTransfer = document.querySelector("#divCurrencyTransfer");
    inputsFormatTransa = document.querySelectorAll(".decimalTransa");
    inputsFormatExchange = document.querySelectorAll(".decimalTC");
    divAmountExchange = document.querySelector("#divAmountExchange");
    divCommission = document.querySelector("#divCommission");
    divCurrencyTransa = document.querySelector("#divCurrencyTransa");
    selectCustomer = document.querySelector("#selectCustomer");
    selectBankAccountSource = document.querySelector("#selectBankAccountSource");
    selectBankAccountTarget = document.querySelector("#selectBankAccountTarget");

    //Accedemos al contenedor principal y modificamos la clase
    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid col-md-12 col-xxl-10 col-11 m-1";
    selectBusinessExecutive = document.querySelector("#selectBusinessExecutive");
    btnSave = document.querySelector("#btnSaveReturn");
    btnNext = document.querySelector("#btnNext");
    btnClosed = document.querySelector("#btnSaveClosed");
    btnReClosed = document.querySelector("#btnSaveReClosed");
    isClosedElement = document.querySelector("#isClosedElement").value;
    $(selectBusinessExecutive).select2(select2Options);

    var selectedOptionActive = $(selectBusinessExecutive).find(':selected');

    if (selectedOptionActive[0].dataset.loan === "True" || selectedOptionActive[0].dataset.payment === "True") {
        fnHiddenButton(true);
    } else {
        fnHiddenButton(false);
    }

    //Evento para el select de ejecutivo
    $(selectBusinessExecutive).on('select2:select', function (e) {
        // Obtiene la opción seleccionada
        let selectedOption = e.params.data.element;

        if (selectedOption.dataset.loan === "True" || selectedOption.dataset.payment === "True") {
            fnHiddenButton(true);
        } else {
            fnHiddenButton(false);
        }
    });

    //Formateamos todos los inputs de transacciones
    inputsFormatTransa.forEach((item) => {
        item.value = formatterAmount().format(fnparseFloat(item.value));

        item.addEventListener("change", (event) => {
            if (event.target === inputAmountTransa ||
                event.target === inputCommission) {
                item.value = formatterAmount().format(fnparseFloat(item.value, true));
            } else {
                item.value = formatterAmount().format(fnparseFloat(item.value));
            }
            //Funcion para re hacer los calculos
            fnCalculateRevenueCost();
        });
    });

    //Formateamos todos los inputs de tipo de cambio
    inputsFormatExchange.forEach((item) => {
        item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value));

        item.addEventListener("change", (event) => {
            if (event.target === inputExchangeRateSellTransa ||
                event.target === inputExchangeRateBuyTransa) {
                item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value, true));
            } else {
                item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value));
            }
            //Funcion para re hacer los calculos
            fnCalculateRevenueCost();
        });
    });

    //Funcion para el evento de la cuenta bancaria de origen
    await fnGetBankAccounts();

    //Setear enfoque en el search input
    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });

    //Obtenemos el tipo de cambio en base a la fecha
    await fnTCByDate();

    inputDateTransa.addEventListener('change', async () => {
        await fnTCByDate();
    });

    //Monedas de la transaccion
    currenciesTransa.forEach((transa) => {
        if (transa.checked) {
            inputCurrencyTransa.value = parseInt(transa.value);
            currencyType = parseInt(transa.value);
        }

        transa.addEventListener("change", async () => {
            currencyType = parseInt(transa.value);
            inputCurrencyTransa.value = parseInt(currencyType);
            fnCalculateRevenueCost();
            await fnTCByDate();
        });
    });

    //Por defecto la moneda es dolar
    currencyTransaType_onClick(inputCurrencyTransa);

    //Monedas del deposito
    currenciesDeposit.forEach((deposit) => {
        deposit.addEventListener("change", async () => {
            inputCurrencyDeposit.value = parseInt(deposit.value);
            currencyTypeDeposit = parseInt(deposit.value);
            currencyTypeTransfer = currencyTypeDeposit;
            fnCalculateRevenueCost();
            await fnTCByDate();
        });
        if (deposit.checked) {
            inputCurrencyDeposit.value = parseInt(deposit.value);
            currencyTypeDeposit = parseInt(deposit.value);
        }
    });

    //Monedas de la transferencia
    currenciesTransfer.forEach((transfer) => {
        transfer.addEventListener("change", async () => {
            inputCurrencyTransfer.value = parseInt(transfer.value);
            currencyTypeTransfer = parseInt(transfer.value);
            currencyTypeDeposit = currencyTypeTransfer;
            fnCalculateRevenueCost();
            await fnTCByDate();
        });

        if (transfer.checked) {
            inputCurrencyTransfer.value = parseInt(transfer.value);
            currencyTypeTransfer = parseInt(transfer.value);
        }
    });

    //Tipos de cotizacion
    typeNumerals.forEach((item) => {
        if (item.checked) inputTypeNumeral.value = parseInt(item.value);
        item.addEventListener("change", () => {
            inputTypeNumeral.value = parseInt(item.value);
            fnLoadInputsByType(item.value);
        });
    });

    //Funcion para ocultar los elementos en dependencia del tipo de cotizacion
    fnLoadInputsByType(inputTypeNumeral.value);

    // Evento enviar form para crear
    const formCreate = document.getElementById("formUpsert");
    formCreate.addEventListener("submit", fnCreateFormSubmit);
    fnEnableTooltip();
});

const fnSaveReturn = () => {
    redirectHome = true;
    redirectDetail = false;
    showMessages = true;
    isClosed = false;
    isReClosed = false;
    btnSaveNext.click();
}

const fnSaveNext = () => {
    redirectHome = false;
    redirectDetail = true;
    showMessages = true;
    isClosed = false;
    isReClosed = false;
    btnSaveNext.click();
}

const fnSaveClosed = () => {
    redirectHome = false;
    redirectDetail = false;
    showMessages = false;
    isClosed = true;
    btnSaveNext.click();
}

const fnSaveReClosed = () => {
    redirectHome = false;
    redirectDetail = false;
    showMessages = false;
    isClosed = false;
    isReClosed = true;
    btnSaveNext.click();
}

const fnCreateFormSubmit = async (event) => {

    try {
        event.preventDefault();
        const formObject = event.currentTarget;

        const url = `${formObject.action}?redirectHome=${redirectHome}&redirectDetail=${redirectDetail}&showMessages=${showMessages}`;
        const formData = new FormData(formObject);

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

                if (isClosed) {
                    await fnClosed(jsonResponse.data.id);
                }else if (isReClosed) {
                    await fnReClosed(jsonResponse.data.id);
                }
            }
        }

    } catch (error) {
        Swal.fire({
            icon: 'error',
            text: error
        });

    }
}

const fnHiddenButton = (isExecutiveLoanOrPayment) => {

    if (isExecutiveLoanOrPayment) {
        btnNext.hidden = true;
        if (isClosedElement == "True") {
            btnClosed.hidden = true;
            btnReClosed.hidden = false;
        } else {
            btnClosed.hidden = false;
            btnReClosed.hidden = true;
        }
        btnSave.hidden = false;
    } else {
        btnNext.hidden = false;
        btnClosed.hidden = true;
        btnReClosed.hidden = true;
        btnSave.hidden = true;
    }
};

const fnClosed = async (id) => {

    try {

        let url = `/exchange/quotation/Closed?id=${id}`;

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

const fnReClosed = async (id) => {

    try {

        let url = `/exchange/quotation/ReClosed?id=${id}`;

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

const fnGetBankAccounts = async () => {

    selectBankAccountSource.addEventListener("change", async () => {

        let url = `/Exchange/Quotation/GetBankAccountTarget?idSource=${selectBankAccountSource.value}`;

        try {

            const response = await fetch(url, {
                method: "POST"
            });

            const jsonResponse = await response.json();

            if (jsonResponse.isSuccess) {
                selectBankAccountTarget.innerHTML = "";
                //// Agregar options
                //let option = document.createElement("option");
                //option.value = "";
                //option.text = "--Select Cuenta Destino--";
                //option.disabled = true;
                //option.selected = true;

                //selectBankAccountTarget.insertBefore(option, selectBankAccountTarget.firstChild);

                jsonResponse.data.forEach((item) => {

                    if (item.value != selectBankAccountSource.value) {
                        let option = document.createElement("option");
                        option.value = item.value;
                        option.text = item.text;
                        selectBankAccountTarget.appendChild(option);
                    }
                });

                //$(selectBankAccountTarget).select2(select2Options);

            } else {
                alert(jsonResponse.errorMessages);
            }

        } catch (error) {
            alert(error);
        }
    });
};

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
            $(selectCustomer).select2('focus');

        } else {
            alert(jsonResponse.errorMessages);
        }

    } catch (error) {
        alert(error);
    }
};

const fnLoadOptionSelectBusiness = (type) => {
    if (type != QuotationType.Transfer) {
        $(selectBusinessExecutive).find('option').each(function () {
            if ($(this).data('loan') === "True" || $(this).data('payment') === "True") {
                $(this).prop('disabled', false);
            }
        });
    } else {
        $(selectBusinessExecutive).find('option').each(function () {
            if ($(this).data('loan') === "True" || $(this).data('payment') === "True") {
                $(this).prop('disabled', true);
            }
        });
    }
};

//Funcion para ocultar o mostrar los elementos en base al tipo
const fnLoadInputsByType = (type) => {
    if (type == QuotationType.Buy) {
        divCurrencyTransa.hidden = false;
        divAmountExchange.hidden = false;
        divCommission.hidden = true;
        divCurrencyTransfer.hidden = false;
        divCurrencyDeposit.hidden = true;
        elementsBuy.forEach((item) => item.hidden = false);
        elementsSell.forEach((item) => item.hidden = true);
        divExchangeRateVariation.hidden = false;
        elementsHiddenBuySell.forEach((item) => {
            item.className = "";
            item.classList.add("row", "col-xl-4", "d-none", "d-xl-block", "typeBuySell");
        });
        elementsTransfer.forEach((item) => item.hidden = true);
        fnChangeCustomers(false);
    } else if (type == QuotationType.Sell) {
        divCurrencyTransa.hidden = false;
        divAmountExchange.hidden = false;
        divCommission.hidden = true;
        divExchangeRateVariation.hidden = false;
        divCurrencyTransfer.hidden = true;
        divCurrencyDeposit.hidden = false;
        elementsBuy.forEach((item) => item.hidden = true);
        elementsSell.forEach((item) => item.hidden = false);
        elementsHiddenBuySell.forEach((item) => {
            item.className = "";
            item.classList.add("row", "col-xl-4", "d-none", "d-xl-block", "typeBuySell");
        });
        elementsTransfer.forEach((item) => item.hidden = true);
        fnChangeCustomers(false);
    } else {
        divAmountExchange.hidden = true;
        divCurrencyTransa.hidden = true;
        divCommission.hidden = false;
        divCurrencyTransfer.hidden = true;
        divExchangeRateVariation.hidden = true;
        divCurrencyDeposit.hidden = true;
        elementsHiddenBuySell.forEach((item) => {
            item.className = "";
            item.classList.add("row", "col-xl-4", "d-none", "typeBuySell");
        });
        elementsBuy.forEach((item) => item.hidden = true);
        elementsSell.forEach((item) => item.hidden = true);
        elementsTransfer.forEach((item) => item.hidden = false);
        fnChangeCustomers(true);
    }
    fnLoadOptionSelectBusiness(type);
    fnCalculateRevenueCost();
};
function currencyTransaType_onClick(objElem) {

    let curDepositBaseDiv = document.getElementById(divnamecurDeposit + CurrencyType.Base);
    let curDepositForeignDiv = document.getElementById(divnamecurDeposit + CurrencyType.Foreign);
    let curDepositAdditionalDiv = document.getElementById(divnamecurDeposit + CurrencyType.Additional);
    let curTransferBaseDiv = document.getElementById(divnamecurTransfer + CurrencyType.Base);
    let curTransferForeignDiv = document.getElementById(divnamecurTransfer + CurrencyType.Foreign);
    let curTransferAdditionalDiv = document.getElementById(divnamecurTransfer + CurrencyType.Additional);
    let currentValue = Number(objElem.value);

    if (currentValue == CurrencyType.Foreign) {
        curDepositForeignDiv.style.display = styleHide;
        curDepositBaseDiv.style.display = styleShowInline;
        curDepositAdditionalDiv.style.display = styleHide;
        //document.getElementById(radnamecurDeposit + CurrencyType.Base).checked = true;
        curTransferForeignDiv.style.display = styleHide;
        curTransferBaseDiv.style.display = styleShowInline;
        curTransferAdditionalDiv.style.display = styleHide;
        //document.getElementById(radnamecurTransfer + CurrencyType.Base).checked = true;
    }
    else if (currentValue == CurrencyType.Additional) {
        curDepositAdditionalDiv.style.display = styleHide;
        curDepositForeignDiv.style.display = styleShowInline;
        curDepositBaseDiv.style.display = styleShowInline;
        //document.getElementById(radnamecurDeposit + CurrencyType.Base).checked = true;
        curTransferAdditionalDiv.style.display = styleHide;
        curTransferForeignDiv.style.display = styleShowInline;
        curTransferBaseDiv.style.display = styleShowInline;
        //document.getElementById(radnamecurTransfer + CurrencyType.Base).checked = true;
    }

    return true;
}

//Funcion para calcular el costo
const fnCalculateRevenueCost = () => {
    let divRevenue = document.querySelector("#divRevenue");
    let divCost = document.querySelector("#divCost");
    let amountExchange = document.querySelector("#amountExchange");
    let exchangeRateBuyTransa = fnparseFloat(inputExchangeRateBuyTransa.value);
    let exchangeRateSellTransa = fnparseFloat(inputExchangeRateSellTransa.value);
    let exchangeRateOfficialTransa = fnparseFloat(inputExchangeRateOfficialTransa.value);
    let exchangeRateVariation = fnparseFloat(inputExchangeRateVariation.value);
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
        exchangeRateVariation = exchangeRateOfficialTransa - exchangeRateBuyTransa;
        inputExchangeRateVariation.value = formatterAmount(decimalExchange).format(exchangeRateVariation);
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
                    amountExchange.value = formatterAmount().format(amountTransa * exchangeRateSellTransa);
                }
            }
        }

        exchangeRateVariation = exchangeRateSellTransa - exchangeRateOfficialTransa;
        inputExchangeRateVariation.value = formatterAmount(decimalExchange).format(exchangeRateVariation);
    }
    inputExchangeRateBuyTransa.value = formatterAmount(decimalExchange).format(exchangeRateBuyTransa);
    inputExchangeRateSellTransa.value = formatterAmount(decimalExchange).format(exchangeRateSellTransa);
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

            if (inputTypeNumeral.value == QuotationType.Buy) {
                if (currencyType == CurrencyType.Foreign) {
                    inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyForeign?.officialRate ?? 1;
                } else if (currencyType == CurrencyType.Additional) {
                    if (currencyTypeTransfer == CurrencyType.Base) {
                        if (jsonResponse.data.currencyAdditional?.officialRate) {
                            inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyAdditional?.officialRate ?? 1;
                        } else {
                            inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyForeign?.officialRate ?? 1;
                        }
                    } else {
                        inputExchangeRateOfficialTransa.value = 1;
                    }
                }
            } else if (inputTypeNumeral.value == QuotationType.Sell) {
                if (currencyType == CurrencyType.Foreign) {
                    inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyForeign?.officialRate ?? 1;
                } else if (currencyType == CurrencyType.Additional) {
                    if (currencyTypeDeposit == CurrencyType.Base) {
                        if (jsonResponse.data.currencyAdditional?.officialRate) {
                            inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyAdditional?.officialRate ?? 1;
                        } else {
                            inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyForeign?.officialRate ?? 1;
                        }
                    } else {
                        inputExchangeRateOfficialTransa.value = 1;
                    }
                }
            } else {
                inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyForeign?.officialRate ?? 1;
            }
        } else {
            inputExchangeRateOfficialTransa.value = 1;
        }

        inputExchangeRateOfficialTransa.value =
            formatterAmount(decimalExchange).format(fnparseFloat(inputExchangeRateOfficialTransa.value));

    } catch (error) {
        alert(error);
    }
};