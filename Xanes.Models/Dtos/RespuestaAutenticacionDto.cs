using System.Text.Json.Serialization;

namespace Xanes.Models.Dtos;

public class RespuestaAutenticacionDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;

    [JsonPropertyName("expiracion")]
    public DateTime Expiracion { get; set; }
}