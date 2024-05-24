using System.Text.Json.Serialization;

namespace Xanes.Models.Dtos;

public class CustomerLegacyDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("personType")]
    public string PersonType { get; set; } = string.Empty;

    [JsonPropertyName("sectorCategoryCode")]
    public string SectorCategoryCode { get; set; } = string.Empty;

    [JsonPropertyName("identificationNumber")]
    public string IdentificationNumber { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("secondName")]
    public string? SecondName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("secondSurname")]
    public string? SecondSurname { get; set; } = string.Empty;

    [JsonPropertyName("businessName")]
    public string BusinessName { get; set; } = string.Empty;

    [JsonPropertyName("commercialName")]
    public string CommercialName { get; set; } = string.Empty;

    [JsonPropertyName("businessAddress")]
    public string BusinessAddress { get; set; } = string.Empty;

    [JsonPropertyName("isBank")]
    public bool IsBank { get; set; }

    [JsonPropertyName("isSystemRow")]
    public bool IsSystemRow { get; set; }
}