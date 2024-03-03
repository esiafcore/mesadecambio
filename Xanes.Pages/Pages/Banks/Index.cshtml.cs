using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xanes.Pages.Data;
using Xanes.Pages.Models;

namespace Xanes.Pages.Pages.Banks
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<Bank> BankList { get; set; }
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            BankList = _db.Banks.ToList();
        }
    }
}
