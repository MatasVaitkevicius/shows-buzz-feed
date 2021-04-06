using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Models
{
    public class Film
    {
        public int Id { get; set; }
        public double Length { get; set; }
        public double Budget { get; set; }
        public int ReleaseYear { get; set; }
        public string Name { get; set; }
    }
}
