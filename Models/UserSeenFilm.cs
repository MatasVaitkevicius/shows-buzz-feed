using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Models
{
    public class UserSeenFilm
    {
        [Key]
        public int Id { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public int UserId { get; set;  }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserSeenFilm()
        {
            Ratings = new List<Rating>();
        }
        public List<Rating> Ratings { get; set; }
    }
}
