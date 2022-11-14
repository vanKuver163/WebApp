using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Model;

namespace WebApp.Pages
{
    [Authorize(Policy="AdminOnly")]
    public class SettingsModel : PageModel
    {
         
        private readonly AppDbContext _db;
        [BindProperty]
        public Album Album { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ArtistCheack { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool AlbumCheack { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchYear { get; set; }
        public List<string> SearchYears { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchGenre { get; set; }
        public List<string> SearchGenres { get; set; }

        public IEnumerable<Album> Albums { get; set; }

        public IEnumerable<UserCollection> UserCollections { get; set; }

        

        public SettingsModel( AppDbContext db)
        {
            
            _db = db;
            SearchYears = GetYear();
            SearchGenres = GetGenre();
            if (_db.Album.Count() == 0)
            {
                InstalDb();
            }

        }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(SearchYear)) Albums = _db.Album.Where(x => x.Year == SearchYear).ToList();
            else if (!string.IsNullOrEmpty(SearchGenre)) Albums = _db.Album.Where(x => x.Genres == SearchGenre).ToList();
            else if (!string.IsNullOrEmpty(SearchGenre) && !string.IsNullOrEmpty(SearchYear)) Albums = _db.Album.Where(x => x.Genres == SearchGenre).Include(x => x.Year == SearchYear).ToList();
            else if (!string.IsNullOrEmpty(Search) && ArtistCheack == true) Albums = _db.Album.Where(x => x.NameArtist == Search).ToList();
            else if (!string.IsNullOrEmpty(Search) && AlbumCheack == true) Albums = _db.Album.Where(x => x.NameAlbum == Search).ToList();
            else Albums = _db.Album;

        }



        public async Task<IActionResult> OnPostAddAsync(int id)
        {
            UserCollections = _db.UserCollection;
            int idUser = (from u in _db.User where u.Name == User.Identity.Name select u.Id).FirstOrDefault();
            if (UserCollections != null)
            {
                foreach (var col in UserCollections)
                {
                    if (col.UserId == idUser && col.AlbumId == id) return RedirectToPage("/Index");
                }
            }
            UserCollection userCol = new UserCollection { AlbumId = id, UserId = idUser };
            await _db.UserCollection.AddAsync(userCol);
            await _db.SaveChangesAsync();
            TempData["Success"] = "The album has been added!";
            return RedirectToPage("/UserCollection");
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Album album = await _db.Album.FindAsync(id);
            if (album == null)
            {
                TempData["Error"] = "The album does not exist!";
            }
            else
            {
                _db.Album.Remove(album);
                await _db.SaveChangesAsync();
                TempData["Error"] = "The album has been deleted!";
            }
            return RedirectToPage("/Settings");
        }


        public void InstalDb()
        {
            using (StreamReader sr = new StreamReader("Info.txt"))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    Album Alb = new Album();
                    string[] subs = line.Split('|');
                    int count = subs.Length;
                    foreach (var sub in subs)
                    {
                        if (count == 6) Alb.NameArtist = sub;
                        else if (count == 5) Alb.NameAlbum = sub;
                        else if (count == 4) Alb.Year = sub;
                        else if (count == 3) Alb.Genres = sub;
                        else if (count == 2) Alb.Website = sub;
                        else if (count == 1) Alb.ImageAddress = sub;
                        count--;
                    }
                    _db.Album.Add(Alb);
                    _db.SaveChanges();
                }
            }
        }

        public List<string> GetYear()
        {
            Albums = _db.Album;
            Albums = Albums.GroupBy(x => x.Year).Select(group => group.First()).ToList();
            List<string> str = new List<string>();
            foreach (var obj in Albums)
            {
                str.Add(obj.Year);
            }
            return str;
        }

        public List<string> GetGenre()
        {
            Albums = _db.Album;
            Albums = Albums.GroupBy(x => x.Genres).Select(group => group.First()).ToList();
            List<string> str = new List<string>();
            foreach (var obj in Albums)
            {
                str.Add(obj.Genres);
            }
            return str;
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
