using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xanes.Pages.Data;
using Xanes.Pages.Models;

namespace Xanes.Pages.Pages.Banks
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly int _companyId;
        public Bank Bank { get; set; }
        public CreateModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _companyId = _configuration.GetValue<int>("ApplicationSettings:CompanyId");
        }

        public void OnGet()
        {
            //Setear valor por defecto
            Bank = new Bank()
            {
                OrderPriority = 1,
                BankingCommissionPercentage = 0,
                CompanyId = _companyId
            };
        }

        public IActionResult OnPost()
        {
            _db.Banks.Add(Bank);
            _db.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
