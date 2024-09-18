namespace Xanes.Models.Dtos.eSiafN4;

public class TransaccionesBcoDtoStatus
{
    public Guid TransaBcoUid { get; set; }
    public string TransaBcoFullName { get; set; }
    public string TransaBcoEstado { get; set; }
    public Guid TransaAsiUid { get; set; }
    public string TransaAsiFullName { get; set; }
    public string TransaAsiEstado { get; set; }
    public Guid? TransaAsiAnuladoUid { get; set; }
    public string? TransaAsiAnuladoFullName { get; set; }
    public string? TransaAsiAnuladoEstado { get; set; }
}