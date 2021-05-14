﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using shows_buzz_feed.Models;

namespace shows_buzz_feed.Mappings.UserSeenTvShow
{
    public class CreateUserSeenTvShowCommand
    {
        public int TvShowId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
