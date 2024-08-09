namespace Xanes.Utility;
public class ExchangeAmounts
{
    public decimal AmountBase { get; set; } = 0M;
    public decimal AmountForeign { get; set; } = 0M;
    public decimal AmountAdditional { get; set; } = 0M;

    public void SetInit()
    {
        AmountBase = 0; 
        AmountForeign = 0; 
        AmountAdditional = 0;
    }
}
