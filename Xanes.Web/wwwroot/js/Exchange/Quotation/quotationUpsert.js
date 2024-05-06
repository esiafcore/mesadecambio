let inputDateTransa, firstCurrency, inputAmountTransa, inputExchangeRateBuyTransa, inputExchangeRateSellTransa,
    inputExchangeRateOfficialTransa, inputAmountCost, inputAmountRevenue, currencyType, currencyTypeDeposit, currencyTypeTransfer, currenciesTransa, currenciesDeposit, currenciesTransfer,
    typeNumerals, inputCurrencyTransa, inputCurrencyDeposit, inputCurrencyTransfer, inputTypeNumeral, elementsBuy, elementsTransfer, elementsSell, divCurrencyTransfer, divCurrencyDeposit,
    divAmountExchange, divCommission, divCurrencyTransa;
let inputsFormatTransa, inputsFormatExchange;
let containerMain, selectCustomer, selectBankAccountTarget, selectBankAccountSource;

document.addEventListener("DOMContentLoaded", async () => {
    inputDateTransa = document.querySelector("#dateTransa");
    inputAmountTransa = document.querySelector("#amountTransa");
    firstCurrency = document.querySelector("#currencyTransaType_radio_2");
    inputExchangeRateBuyTransa = document.querySelector("#exchangeRateBuyTransa");
    inputExchangeRateSellTransa = document.querySelector("#exchangeRateSellTransa");
    inputExchangeRateOfficialTransa = document.querySelector("#exchangeRateOfficialTransa");
    inputAmountCost = document.querySelector("#amountCost");
    inputAmountRevenue = document.querySelector("#amountRevenue");
    currenciesTransa = document.querySelectorAll(".currenciesTransa");
    currenciesDeposit = document.querySelectorAll(".currenciesTransfer");
    currenciesTransfer = document.querySelectorAll(".currenciesDeposit");
    typeNumerals = document.querySelectorAll(".typeNumerals");
    inputCurrencyTransa = document.querySelector("#currencyTransaType");
    inputCurrencyDeposit = document.querySelector("#currencyDepositType");
    inputCurrencyTransfer = document.querySelector("#currencyTransferType");
    inputTypeNumeral = document.querySelector("#typeNumeral");
    elementsBuy = document.querySelectorAll(".typeBuy");
    elementsTransfer = document.querySelectorAll(".typeTransfer");
    elementsSell = document.querySelectorAll(".typeSell");
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
    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid col-md-12 col-xxl-10 col-11 m-1";
    inputsFormatTransa.forEach((item) => {
        item.value = formatterAmount().format(fnparseFloat(item.value));

        item.addEventListener("change", () => {
            item.value = formatterAmount().format(fnparseFloat(item.value));
            fnCalculateRevenueCost();
        });
    });

    inputsFormatExchange.forEach((item) => {
        item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value));

        item.addEventListener("change", () => {
            item.value = formatterAmount(decimalExchange).format(fnparseFloat(item.value));
            fnCalculateRevenueCost();
        });
    });

    await fnGetBankAccounts();

    //Setear enfoque en el search input
    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });

    currencyType = firstCurrency.value;
    inputCurrencyTransa.value = parseInt(currencyType);
    //Por defecto la moneda es dolar
    currencyTransaType_onClick(firstCurrency);

    //Obtenemos el tipo de cambio en base a la fecha
    await fnTCByDate();

    inputDateTransa.addEventListener('change', async () => {
        await fnTCByDate();
    });

    currenciesTransa.forEach((item) => {
        item.addEventListener("change", () => {
            currencyType = item.value;
            inputCurrencyTransa.value = parseInt(currencyType);
        });
    });

    currenciesDeposit.forEach((item) => {
        if (item.checked) {
            inputCurrencyDeposit.value = parseInt(item.value);
            currencyTypeDeposit = parseInt(item.value);
        }
        item.addEventListener("change", () => {
            inputCurrencyDeposit.value = parseInt(item.value);
        });
    });

    currenciesTransfer.forEach((item) => {
        if (item.checked) {
            inputCurrencyTransfer.value = parseInt(item.value);
            currencyTypeTransfer = parseInt(item.value);
        }
        item.addEventListener("change", () => {
            inputCurrencyTransfer.value = parseInt(item.value);
            currencyTypeTransfer = parseInt(item.value);
        });
    });

    typeNumerals.forEach((item) => {
        if (item.checked) inputTypeNumeral.value = parseInt(item.value);
        item.addEventListener("change", () => {
            inputTypeNumeral.value = parseInt(item.value);
            fnLoadInputsByType(item.value);
        });
    });

    fnLoadInputsByType(inputTypeNumeral.value);
});

const fnGetBankAccounts = async () => {
    $(selectBankAccountSource).select2(select2Options);
    $(selectBankAccountTarget).select2(select2Options);

    $(selectBankAccountSource).on('select2:select', async function (e) {
        let url = `/Exchange/Quotation/GetBankAccountTarget?idSource=${e.params.data.id}`;

        try {

            const response = await fetch(url, {
                method: "POST"
            });

            const jsonResponse = await response.json();

            if (jsonResponse.isSuccess) {
                selectBankAccountTarget.innerHTML = "";
                // Agregar options
                let option = document.createElement("option");
                option.value = "";
                option.text = "--Select Cta Destino--";
                option.disabled = true;
                option.selected = true;

                selectBankAccountTarget.insertBefore(option, selectBankAccountTarget.firstChild);

                jsonResponse.data.forEach((item) => {
                    let option = document.createElement("option");
                    option.value = item.value;
                    option.text = item.text;
                    selectBankAccountTarget.appendChild(option);
                });

                $(selectBankAccountTarget).select2(select2Options);

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
    } else {
        divAmountExchange.hidden = true;
        divCurrencyTransa.hidden = true;
        divCommission.hidden = false;
        divCurrencyTransfer.hidden = true;
        divCurrencyDeposit.hidden = true;
        elementsBuy.forEach((item) => item.hidden = true);
        elementsSell.forEach((item) => item.hidden = true);
        elementsTransfer.forEach((item) => item.hidden = false);
        fnChangeCustomers(true);
    }
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
        document.getElementById(radnamecurDeposit + CurrencyType.Base).checked = true;
        curTransferForeignDiv.style.display = styleHide;
        curTransferBaseDiv.style.display = styleShowInline;
        curTransferAdditionalDiv.style.display = styleHide;
        document.getElementById(radnamecurTransfer + CurrencyType.Base).checked = true;
    }
    else if (currentValue == CurrencyType.Additional) {
        curDepositAdditionalDiv.style.display = styleHide;
        curDepositForeignDiv.style.display = styleShowInline;
        curDepositBaseDiv.style.display = styleShowInline;
        document.getElementById(radnamecurDeposit + CurrencyType.Foreign).checked = true;
        curTransferAdditionalDiv.style.display = styleHide;
        curTransferForeignDiv.style.display = styleShowInline;
        curTransferBaseDiv.style.display = styleShowInline;
        document.getElementById(radnamecurTransfer + CurrencyType.Foreign).checked = true;
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