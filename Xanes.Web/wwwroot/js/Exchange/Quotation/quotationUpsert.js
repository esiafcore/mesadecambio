let inputDateTransa, firstCurrency, inputAmountTransa, inputExchangeRateBuyTransa, inputExchangeRateSellTransa,
    inputExchangeRateOfficialTransa, inputAmountCost, inputAmountRevenue, currencyType, currenciesDestiny, typeNumerals, currenciesOrigin,
    inputCurrencyDestiny, inputCurrencyOrigin, inputTypeNumeral, elementsBuy, elementsSell;

document.addEventListener("DOMContentLoaded", async () => {
    inputDateTransa = document.querySelector("#dateTransa");
    inputAmountTransa = document.querySelector("#amountTransa");
    firstCurrency = document.querySelector("#currencyTransaType_radio_2");
    inputExchangeRateBuyTransa = document.querySelector("#exchangeRateBuyTransa");
    inputExchangeRateSellTransa = document.querySelector("#exchangeRateSellTransa");
    inputExchangeRateOfficialTransa = document.querySelector("#exchangeRateOfficialTransa");
    inputAmountCost = document.querySelector("#amountCost");
    inputAmountRevenue = document.querySelector("#amountRevenue");
    currenciesDestiny = document.querySelectorAll(".currenciesDestiny");
    typeNumerals = document.querySelectorAll(".typeNumerals");
    currenciesOrigin = document.querySelectorAll(".currenciesOrigin");
    inputCurrencyDestiny = document.querySelector("#currencyTransaType");
    inputCurrencyOrigin = document.querySelector("#currencyOriginExchangeType");
    inputTypeNumeral = document.querySelector("#typeNumeral");
    elementsBuy = document.querySelectorAll(".typeBuy");
    elementsSell = document.querySelectorAll(".typeSell");
    let decimalFormat = document.querySelectorAll(".decimalFormat");

    //Aplicar select2
    $("#selectCustomer").select2(select2Options);

    //Setear enfoque en el search input
    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });

    currencyType = firstCurrency.value;
    inputCurrencyDestiny.value = parseInt(currencyType);
    //Por defecto la moneda es dolar
    currencyTransaType_onClick(firstCurrency);

    //Obtenemos el tipo de cambio en base a la fecha
    await fnTCByDate();

    inputDateTransa.addEventListener('change', async () => {
        await fnTCByDate();
    });

    inputAmountTransa.addEventListener("change", () => {
        fnCalculateRevenueCost();
    });

    currenciesDestiny.forEach((item) => {
        item.addEventListener("change", () => {
            currencyType = item.value;
            inputCurrencyDestiny.value = parseInt(currencyType);
        });
    });

    typeNumerals.forEach((item) => {
        if (item.checked) inputTypeNumeral.value = parseInt(item.value);
        item.addEventListener("change", () => {
            inputTypeNumeral.value = parseInt(item.value);
            if (item.value == QuotationType.Buy) {
                elementsBuy.forEach((item) => item.hidden = false);
                elementsSell.forEach((item) => item.hidden = true);

            } else if (item.value == QuotationType.Sell) {
                elementsBuy.forEach((item) => item.hidden = true);
                elementsSell.forEach((item) => item.hidden = false);
            } else {

            }
        });
    });

    currenciesOrigin.forEach((item) => {
        if (item.checked) inputCurrencyOrigin.value = parseInt(item.value);
        item.addEventListener("change", () => {
            inputCurrencyOrigin.value = parseInt(item.value);
        });
    });

    //decimalFormat.forEach((item) => {
    //    item.value = formatterAmount().format(parseFloat(item.value));
    //    item.addEventListener("change", () => {
    //        item.value = formatterAmount().format(item.value);
    //    });
    //});

});

function currencyTransaType_onClick(objElem) {

    let curExchangeBaseDiv = document.getElementById(divnamecurExchange + CurrencyType.Base);
    let curExchangeForeignDiv = document.getElementById(divnamecurExchange + CurrencyType.Foreign);
    let curExchangeAdditionalDiv = document.getElementById(divnamecurExchange + CurrencyType.Additional);
    let currentValue = Number(objElem.value);

    if (currentValue == CurrencyType.Foreign) {
        curExchangeForeignDiv.style.display = styleHide;
        curExchangeBaseDiv.style.display = styleShowInline;
        curExchangeAdditionalDiv.style.display = styleShowInline;
        document.getElementById(radnamecurExchange + CurrencyType.Base).checked = true;
    }
    else if (currentValue == CurrencyType.Additional) {
        curExchangeAdditionalDiv.style.display = styleHide;
        curExchangeForeignDiv.style.display = styleShowInline;
        curExchangeBaseDiv.style.display = styleShowInline;
        document.getElementById(radnamecurExchange + CurrencyType.Foreign).checked = true;
    }

    return true;
}

//Funcion para calcular el costo
const fnCalculateRevenueCost = () => {

    let exchangeRateBuyTransa = parseFloat(inputExchangeRateBuyTransa.value);
    let exchangeRateSellTransa = parseFloat(inputExchangeRateSellTransa.value);
    let exchangeRateOfficialTransa = parseFloat(inputExchangeRateOfficialTransa.value);
    let amountTransa = fnparseFloat(inputAmountTransa.value);
    let amountCost = 0, amountRevenue = 0;

    if (inputTypeNumeral.value == QuotationType.Buy) {
        if (exchangeRateBuyTransa > exchangeRateOfficialTransa) {
            amountCost = (exchangeRateBuyTransa - exchangeRateOfficialTransa) * amountTransa;
        } else {
            amountRevenue = (exchangeRateOfficialTransa - exchangeRateBuyTransa) * amountTransa;
        }
    } else if (inputTypeNumeral.value == QuotationType.Sell) {
        if (exchangeRateSellTransa > exchangeRateOfficialTransa) {
            amountRevenue = (exchangeRateSellTransa - exchangeRateOfficialTransa) * amountTransa;
        } else {
            amountCost = (exchangeRateOfficialTransa - exchangeRateSellTransa) * amountTransa;
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