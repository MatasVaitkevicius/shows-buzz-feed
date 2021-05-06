﻿using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }
        public DbSet<Film> Films { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSeenFilm> UserSeenFilms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Film>();
            builder.Entity<Series>();
            builder.Entity<Rating>();
            builder.Entity<User>();
            builder.Entity<UserSeenFilm>()
                .HasOne(e => e.Film)
                .WithMany(i => i.UserSeenFilms)
                .HasForeignKey(e => e.UserId);
            builder.Entity<UserSeenFilm>()
                .HasOne(e => e.User)
                .WithMany(i => i.UserSeenFilms)
                .HasForeignKey(e => e.UserId);
        }
    }
}
