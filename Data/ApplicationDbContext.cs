using Microsoft.EntityFrameworkCore;
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
        public DbSet<TVShows> TVShows { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSeenFilm> UserSeenFilms { get; set; }
        public DbSet<UserSeenTvShow> UserSeenTvShows { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Quiz> Quiz { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Film>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<TVShows>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Rating>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<User>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<UserSeenFilm>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<UserSeenFilm>()
                .HasOne(e => e.Film)
                .WithMany(i => i.UserSeenFilms)
                .HasForeignKey(e => e.FilmId);
            builder.Entity<UserSeenFilm>()
                .HasOne(e => e.User)
                .WithMany(i => i.UserSeenFilms)
                .HasForeignKey(e => e.UserId);
            builder.Entity<UserSeenTvShow>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<UserSeenTvShow>()
                .HasOne(e => e.TvShow);
            builder.Entity<UserSeenTvShow>()
                .HasOne(e => e.User);
            builder.Entity<Rating>()
                .HasOne(e => e.UserSeenFilm)
                .WithMany(i => i.Ratings)
                .HasForeignKey(e => e.UserSeenFilmId);
            builder.Entity<Rating>()
                .HasOne(e => e.UserSeenTvShow)
                .WithMany(i => i.Ratings)
                .HasForeignKey(e => e.UserSeenTvShowId);
            builder.Entity<Question>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Answer>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Answer>()
                .HasOne(e => e.Question)
                .WithMany(i => i.Answers)
                .HasForeignKey(e => e.QuestionId);
            builder.Entity<Quiz>()
                .Property(e => e.Id);
            builder.Entity<Question>()
                .HasOne(e => e.Quiz)
                .WithMany(i => i.Questions)
                .HasForeignKey(e => e.QuizId);
        }
    }
}
