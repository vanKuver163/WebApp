using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Model;

namespace WebApp.Pages
{
    [Authorize]
    public class UserCollectionModel : PageModel
    {
        private readonly AppDbContext _db;        
       
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<UserCollection> UserCollections { get; set; }



        public UserCollectionModel( AppDbContext db)
        {           
            _db = db;  
        }

        public void OnGet()
        {          
           int idUser = (from u in _db.User where u.Name == User.Identity.Name select u.Id).FirstOrDefault();          
           UserCollections = _db.UserCollection;
           Albums = from a in _db.Album from p in _db.UserCollection where p.AlbumId == a.Id && p.UserId == idUser select a;
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            UserCollections = _db.UserCollection;
            int idUser = (from u in _db.User where u.Name == User.Identity.Name select u.Id).FirstOrDefault();            
            UserCollection userCol = (from u in _db.UserCollection where u.AlbumId == id && u.UserId == idUser select u).FirstOrDefault();
            _db.UserCollection.Remove(userCol);
            await _db.SaveChangesAsync();
            TempData["Success"] = "The album has been deleted!";
            return RedirectToPage("/UserCollection");
        }

        public async Task<IActionResult> OnPostUserReorderAsync(int[] id)
        {
            
            foreach (var albumId in id)
            {
                Album album = await _db.Album.FindAsync(albumId);
                _db.Update(album);
                await _db.SaveChangesAsync();                
            }
            Response.StatusCode = 200;
            return new JsonResult(new { message = "Ok" });
        }



       
    }
}
