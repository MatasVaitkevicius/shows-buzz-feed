using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shows_buzz_feed.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateRated { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }
        public UserSeenFilm UserSeenFilm { get; set; }
        public int UserId { get; set; }
        public int? UserSeenFilmId { get; set; }
        public UserSeenTvShow UserSeenTvShow { get; set; }
        public int? UserSeenTvShowId { get; set; }
    }
}
