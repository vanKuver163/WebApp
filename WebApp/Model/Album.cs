using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Model
{
    public class Album
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NameArtist { get; set; }
        [Required]
        public string NameAlbum { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string Genres { get; set; }       
        [Required]
        public string Website { get; set; }
        [Required]
        public string ImageAddress { get; set; }
    }
}
