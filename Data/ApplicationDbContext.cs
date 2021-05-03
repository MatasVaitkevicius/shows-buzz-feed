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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Film>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Series>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
