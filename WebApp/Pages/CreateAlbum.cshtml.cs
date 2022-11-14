using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data;
using WebApp.Model;

namespace WebApp.Pages
{
    public class CreateAlbumModel : PageModel
    {
        private readonly AppDbContext _db;

        public Album Album { get; set; }

        public CreateAlbumModel(AppDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(Album album)
        {
            if (ModelState.IsValid)
            {
                await _db.Album.AddAsync(album);
                await _db.SaveChangesAsync();
                TempData["Success"] = "The album has been created!";
                return RedirectToPage("/Settings");
            }
            return Page();
        }
    }
}
