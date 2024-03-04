using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xanes.Pages.Data;
using Xanes.Pages.Models;

namespace Xanes.Pages.Pages.Banks;

[BindProperties]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;
    public Bank Bank { get; set; }
    public DeleteModel(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
        _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
    }
    public void OnGet(int? id)
    {
        Bank = new Bank();
        if ((id != null) && (id != 0))
        {
            Bank = _db.Banks.Find(id);
        }
    }

    public IActionResult OnPost()
    {
        Bank obj = _db.Banks.Find(Bank.Id);
        if (obj == null)
        {
            return NotFound();
        }
        _db.Banks.Remove(obj);
        _db.SaveChanges();
        return RedirectToPage("Index");
    }

}
