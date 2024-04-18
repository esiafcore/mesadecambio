function currencyTransaType_onClick(objElem) {


    let curExchangeBaseDiv = document.getElementById(divnamecurExchange + CurrencyType.Base);
    let curExchangeForeignDiv = document.getElementById(divnamecurExchange + CurrencyType.Foreign);
    let curExchangeAdditionalDiv = document.getElementById(divnamecurExchange + CurrencyType.Additional);
    let curTransaValue = Number(objElem.value);

    if (curTransaValue == CurrencyType.Foreign) {
        curExchangeForeignDiv.style.display = styleHide;
        curExchangeBaseDiv.style.display = styleShow;
        curExchangeAdditionalDiv.style.display = styleShow;
        document.getElementById(radnamecurExchange + CurrencyType.Base).checked = true;
    }
    else if (curTransaValue == CurrencyType.Additional) {
        curExchangeAdditionalDiv.style.display = styleHide;
        curExchangeForeignDiv.style.display = styleShow;
        curExchangeBaseDiv.style.display = styleShow;
        document.getElementById(radnamecurExchange + CurrencyType.Foreign).checked = true;
    }

    return true;
}