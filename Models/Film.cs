﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Models
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        public double Length { get; set; }
        public double Budget { get; set; }
        public int ReleaseYear { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public Film()
        {
            UserSeenFilms = new List<UserSeenFilm>();
        }
        public List<UserSeenFilm> UserSeenFilms { get; set; }
    }
}
