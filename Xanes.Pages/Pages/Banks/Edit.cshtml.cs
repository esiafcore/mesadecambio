using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xanes.Pages.Data;
using Xanes.Pages.Models;

namespace Xanes.Pages.Pages.Banks;

[BindProperties]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly int _companyId;
    public Bank Bank { get; set; }

    public EditModel(ApplicationDbContext db, IConfiguration configuration)
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
        if (ModelState.IsValid)
        {
            _db.Banks.Update(Bank);
            _db.SaveChanges();
            TempData["success"] = "Bank updated successfully";
            return RedirectToPage("Index");
        }
        return Page();
    }
}
