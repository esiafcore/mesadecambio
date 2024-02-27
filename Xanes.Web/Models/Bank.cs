using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Xanes.Web.Models;

public class Bank
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public string Code { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    public decimal BankingCommissionPercentage { get; set; }
    public Guid? BankAccountExcludeUId { get; set; }
    [Required]
    public bool IsCompany { get; set; }
    public int OrderPriority { get; set; }
    public string LogoBank { get; set; } = null!;
}