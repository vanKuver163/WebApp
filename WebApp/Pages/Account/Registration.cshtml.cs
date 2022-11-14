using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data;
using WebApp.Model;

namespace WebApp.Pages.Account
{
    public class RegistrationModel : PageModel
    {
        private readonly AppDbContext _db;

        [BindProperty]
        new public User User { get; set; }
        [BindProperty]
        public string passConfirm { get; set; }
        [BindProperty]
        public IEnumerable<User> Users { get; set; }
        public RegistrationModel(AppDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {        
        }
        public async Task<IActionResult> OnPostAsync()
        {            
            Users = _db.User;
            if (passConfirm != User.Password) return Page();
            if (Users != null)
            {
                foreach (var us in Users)
                {
                    if (us.Login == User.Login || User.Login=="admin") return Page();
                }
            }
            await _db.User.AddAsync(User);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Account/Login");
        }
    }
}
