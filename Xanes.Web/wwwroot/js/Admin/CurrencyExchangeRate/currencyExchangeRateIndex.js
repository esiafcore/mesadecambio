const fnupdateLinkParameter = (selectedRadio) => {
    const radioValue = parseInt(selectedRadio.value);
    window.location.href = `${window.location.origin}/Admin/CurrencyExchangeRate?currencyType=${radioValue}`;
};