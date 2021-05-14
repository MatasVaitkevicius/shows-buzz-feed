using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.UserSeenTvShow
{
    public class UserSeenTvShowViewModel
    {
        [Key]
        public int Id { get; set; }
        public int TvShowId { get; set; }
        public string TvShowName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
