using Microsoft.EntityFrameworkCore;

namespace Xanes.Web.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opptons): base(opptons)
    {
            
    }
}