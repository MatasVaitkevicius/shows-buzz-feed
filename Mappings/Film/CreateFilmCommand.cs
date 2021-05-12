using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.Film
{
    public class CreateFilmCommand
    {
        public double Length { get; set; }
        public double Budget { get; set; }
        public int ReleaseYear { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
    }
}
