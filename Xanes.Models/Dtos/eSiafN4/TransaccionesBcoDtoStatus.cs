namespace Xanes.Models.Dtos.eSiafN4;

public class TransaccionesBcoDtoStatus
{
    public Guid TransaBcoUid { get; set; }
    public string TransaBcoFullName { get; set; } = null!;  
    public string TransaBcoEstado { get; set; } = null!;    
    public Guid TransaAsiUid { get; set; }
    public string TransaAsiFullName { get; set; } = null!;
    public string TransaAsiEstado { get; set; } = null!;
    public Guid? TransaAsiAnuladoUid { get; set; }
    public string? TransaAsiAnuladoFullName { get; set; }
    public string? TransaAsiAnuladoEstado { get; set; }
}