using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.UserSeenFilm
{
    public class CreateUserSeenFilmModel
    {
        [Required]
        public string FilmId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
