using shows_buzz_feed.Mappings.UserSeenFilm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.User
{
    public class UserViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserSeenFilmViewModel> UserSeenFilms { get; set; }
    }
}
