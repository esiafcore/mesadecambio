using System.Text.Json.Serialization;

namespace Xanes.Models.Dtos;

public class CredencialesUsuarioDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}