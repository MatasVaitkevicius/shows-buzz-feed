using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public User()
        {
            UserSeenFilms = new List<UserSeenFilm>();
        }
        public List<UserSeenFilm> UserSeenFilms { get; set; }
        public string Name { get; set; }

    }
}
