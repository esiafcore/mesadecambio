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