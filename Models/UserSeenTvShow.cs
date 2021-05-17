using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Models
{
    public class UserSeenTvShow
    {
        public int Id { get; set; }
        public int TvShowId { get; set; }
        public TVShows TvShow { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserSeenTvShow()
        {
            Ratings = new List<Rating>();
        }
        public List<Rating>? Ratings { get; set; }
    }
}
