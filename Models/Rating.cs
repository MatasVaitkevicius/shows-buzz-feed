using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace shows_buzz_feed.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateRated { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }
    }
}
