namespace Xanes.Models.Dtos.XanesN4;

public class CustomerLegacyDto
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string IdentificationTypeCode { get; set; } = string.Empty;

    public string PersonType { get; set; } = string.Empty;

    public string SectorCategoryCode { get; set; } = string.Empty;

    public string IdentificationNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string? SecondName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? SecondSurname { get; set; } = string.Empty;

    public string BusinessName { get; set; } = string.Empty;

    public string CommercialName { get; set; } = string.Empty;

    public string BusinessAddress { get; set; } = string.Empty;

    public bool IsBank { get; set; }

    public bool IsSystemRow { get; set; }

    public int TotalQuotations { get; set; }
}