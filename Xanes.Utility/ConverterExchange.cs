using static Xanes.Utility.SD;
namespace Xanes.Utility;
public class ConverterExchange
{
    private readonly ExchangeAmounts _mtos = new ExchangeAmounts();

    public ExchangeAmounts ConverterExchangeTo(CurrencyType currencyType  ,decimal amountExchange
        , decimal exchangeRateForeign  ,decimal exchangeRateAdditional
        , int decimalTrx)
    {

        switch (currencyType)
        {
            case CurrencyType.Base:
                _mtos.AmountBase = amountExchange.RoundTo(decimalTrx);
                //Convertir a las otras monedas
                _mtos.AmountForeign = _mtos.AmountBase
                    .ExchangeTo(CurrencyType.Base
                        , CurrencyType.Foreign
                        , exchangeRateForeign, decimalTrx);
                _mtos.AmountForeign = _mtos.AmountForeign.RoundTo(decimalTrx);

                _mtos.AmountAdditional = _mtos.AmountBase
                    .ExchangeTo(CurrencyType.Base
                        , CurrencyType.Additional
                        , exchangeRateAdditional, decimalTrx);
                _mtos.AmountAdditional = _mtos.AmountAdditional.RoundTo(decimalTrx);
                break;

            case CurrencyType.Foreign:
                _mtos.AmountForeign = amountExchange.RoundTo(decimalTrx);
                //Convertir a las otras monedas
                _mtos.AmountBase = _mtos.AmountForeign
                    .ExchangeTo(CurrencyType.Foreign
                        , CurrencyType.Base
                        , exchangeRateForeign, decimalTrx);
                _mtos.AmountBase = _mtos.AmountBase.RoundTo(decimalTrx);

                _mtos.AmountAdditional = _mtos.AmountBase
                    .ExchangeTo(CurrencyType.Base
                        , CurrencyType.Additional
                        , exchangeRateAdditional, decimalTrx);
                _mtos.AmountAdditional = _mtos.AmountAdditional.RoundTo(decimalTrx);
                break;

            case CurrencyType.Additional:
                _mtos.AmountAdditional = amountExchange.RoundTo(decimalTrx);
                //Convertir a las otras monedas
                _mtos.AmountBase = _mtos.AmountAdditional
                    .ExchangeTo(CurrencyType.Additional
                        , CurrencyType.Base
                        , exchangeRateAdditional, decimalTrx);
                _mtos.AmountBase = _mtos.AmountBase.RoundTo(decimalTrx);

                _mtos.AmountForeign = _mtos.AmountBase
                    .ExchangeTo(CurrencyType.Base
                        , CurrencyType.Foreign
                        , exchangeRateForeign, decimalTrx);
                _mtos.AmountForeign = _mtos.AmountForeign.RoundTo(decimalTrx);
                break;
        }

        return _mtos;
    }

}
