using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Xanes.DataAccess.Shared;

public class UtcVAlueConverter : ValueConverter<DateTime,DateTime>
{

    public UtcVAlueConverter() : 
        base(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    {
        
    }
}