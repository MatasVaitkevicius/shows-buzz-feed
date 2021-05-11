using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.Rating
{
    public class RatingViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }
        public int UserSeenFilmId { get; set; }
        public string FilmName { get; set; }
        public int UserId { get; set; }
        public DateTime DateRated { get; set; }
    }
}
