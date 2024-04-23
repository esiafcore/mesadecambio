let inputDateTransa;
document.addEventListener("DOMContentLoaded", async () => {
    inputDateTransa = document.querySelector("#dateTransa");
    $("#selectCustomer").select2(select2Options);

    //Setear enfoque en el search input
    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });

    await TCByDate();

    inputDateTransa.addEventListener('change', async () => {
        await TCByDate();
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


const TCByDate = async () => {
    let url = `/Admin/CurrencyExchangeRate/GetCurrencyExchangeRate?date=${inputDateTransa.value}`;

    try {

        const response = await fetch(url, {
            method: "POST"
        });

        const jsonResponse = await response.json();

        let inputExchangeRateOfficialTransa = document.querySelector("#exchangeRateOfficialTransa");

        if (jsonResponse.isSuccess) {
            inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyForeign?.officialRate ?? 1;
            inputExchangeRateOfficialTransa.value = jsonResponse.data.currencyAdditional?.officialRate ?? 1;
        } else {
            inputExchangeRateOfficialTransa.value = 1;
        }

    } catch (error) {
        alert(error);
    }
};