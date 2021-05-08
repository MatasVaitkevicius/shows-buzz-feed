using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.UserSeenFilm
{
    public class UserSeenFilmViewModel
    {
        [Key]
        public int Id { get; set; }
        public int FilmId { get; set; }
        public string FilmName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
