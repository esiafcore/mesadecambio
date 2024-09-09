namespace Xanes.Models.Shared;
public class TransaccionResponse
{
    public string NumberTransa { get; set; } = string.Empty;
    public Guid TipoId { get; set; } 
    public Guid SubtipoId { get; set; }

}