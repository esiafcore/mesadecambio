using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Xanes.Web.Models;

public class Bank
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal BankingCommissionPercentage { get; set; }
    public Guid? BankAccountExcludeUId { get; set; }
    public bool IsCompany { get; set; }
    public int OrderPriority { get; set; }
    public string LogoBank { get; set; } = null!;
}