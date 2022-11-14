using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Data;
using WebApp.Model;

namespace WebApp.Pages
{
    [Authorize(Policy = "AdminOnly")]
    public class EditAlbumModel : PageModel
    {
        private readonly AppDbContext _db;

        public Album Album { get; set; }
        public EditAlbumModel(AppDbContext db)
        {
            _db = db;
        }


        public IActionResult OnGet(int id)
        {
            Album = _db.Album.Find(id);
            if (Album == null) return RedirectToPage("/Settings");
            else return Page();
        }

        public IActionResult OnPost(Album album)
        {
            if (ModelState.IsValid)
            {
                _db.Album.Update(album);
                _db.SaveChanges();
                TempData["Success"] = "The album has been edited!";
                return RedirectToPage("/Settings");
            }
            return Page();
        }
    }
}
