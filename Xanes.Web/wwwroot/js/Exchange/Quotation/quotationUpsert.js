let inputDateTransa, firstCurrency, inputAmountTransa, inputExchangeRateBuyTransa,
    inputExchangeRateOfficialTransa, inputAmountCost, currencyType, currencies, typeNumerals, currenciesOrigin,
    inputCurrencyType, inputCurrencyOriginExchangeType, inputTypeNumeral;

document.addEventListener("DOMContentLoaded", async () => {
    inputDateTransa = document.querySelector("#dateTransa");
    inputAmountTransa = document.querySelector("#amountTransa");
    firstCurrency = document.querySelector("#currencyTransaType_radio_2");
    inputExchangeRateBuyTransa = document.querySelector("#exchangeRateBuyTransa");
    inputExchangeRateOfficialTransa = document.querySelector("#exchangeRateOfficialTransa");
    inputAmountCost = document.querySelector("#amountCost");
    currencies = document.querySelectorAll(".currencies");
    typeNumerals = document.querySelectorAll(".typeNumerals");
    currenciesOrigin = document.querySelectorAll(".currenciesOrigin");
    inputCurrencyType = document.querySelector("#currencyTransaType");
    inputCurrencyOriginExchangeType = document.querySelector("#currencyOriginExchangeType");
    inputTypeNumeral = document.querySelector("#typeNumeral");

    //Aplicar select2
    $("#selectCustomer").select2(select2Options);

    //Setear enfoque en el search input
    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });

    currencyType = firstCurrency.value;
    inputCurrencyType.value = parseInt(currencyType);
    //Por defecto la moneda es dolar
    currencyTransaType_onClick(firstCurrency);

    //Obtenemos el tipo de cambio en base a la fecha
    await fnTCByDate();

    inputDateTransa.addEventListener('change', async () => {
        await fnTCByDate();
    });

    inputAmountTransa.addEventListener("change", () => {
        fnCalculateCost();
    });

    currencies.forEach((item) => {
        item.addEventListener("change", () => {
            currencyType = item.value;
            inputCurrencyType.value = parseInt(currencyType);
        });
    });

    typeNumerals.forEach((item) => {
        if (item.checked) inputTypeNumeral.value = parseInt(item.value);
        item.addEventListener("change", () => {
            inputTypeNumeral.value = parseInt(item.value);
        });
    });

    currenciesOrigin.forEach((item) => {
        if (item.checked) inputCurrencyOriginExchangeType.value = parseInt(item.value);
        item.addEventListener("change", () => {
            inputCurrencyOriginExchangeType.value = parseInt(item.value);
        });
    });
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
const fnCalculateCost = () => {

    let exchangeRateBuyTransa = parseFloat(inputExchangeRateBuyTransa.value);
    let exchangeRateOfficialTransa = parseFloat(inputExchangeRateOfficialTransa.value);
    let amountTransa = parseFloat(inputAmountTransa.value);
    let amountCost = 0;

    amountCost = (exchangeRateBuyTransa - exchangeRateOfficialTransa) * amountTransa;

    inputAmountCost.value = amountCost;
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