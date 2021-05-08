using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.Rating
{
    public class CreateRatingCommand
    {
        public string Comment { get; set; }
        public int Rate { get; set; }
        public DateTime DateRated { get; set; }
    }
}
