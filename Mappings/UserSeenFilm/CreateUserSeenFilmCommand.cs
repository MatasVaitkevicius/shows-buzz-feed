using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using shows_buzz_feed.Models;

namespace shows_buzz_feed.Mappings.UserSeenFilm
{
    public class CreateUserSeenFilmCommand
    {
        [Key]
        public int Id { get; set; }
        public int FilmId { get; set; }
        // TODO
        public int UserId { get; set; }
        public int RouteId { get; set; }

        public DateTime RouteDate { get; set; }
    }
}
