using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Film;
using shows_buzz_feed.Mappings.UserSeenFilm;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;



namespace shows_buzz_feed.Services
{
    public class RecommendedFilmService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public RecommendedFilmService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<FilmListViewModel> GetFilmsAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/RecommendedFilm/");
                return JsonConvert.DeserializeObject<FilmListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<UserSeenFilmListViewModel> GetUserSeenFilmsAsync(int userId)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/RecommendedFilm/{userId}");
                return JsonConvert.DeserializeObject<UserSeenFilmListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<FilmListViewModel> GetRecommededFilmAsync(int userId)
        {
            FilmListViewModel films;
            UserSeenFilmListViewModel userSeenFilms;

            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/RecommendedFilm/");
                films = JsonConvert.DeserializeObject<FilmListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }

            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/RecommendedFilm/{userId}");
                userSeenFilms = JsonConvert.DeserializeObject<UserSeenFilmListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }


            bool fakeValue = checkIfAllFilmsAreSeen(films, userSeenFilms);
            List<string> favoriteGenre = findFavoriteGenre(userSeenFilms, films);
            List<int> favoriteYears = findFavoriteYears(userSeenFilms, films);


            return films;
        }

        public bool checkIfAllFilmsAreSeen(FilmListViewModel films, UserSeenFilmListViewModel userSeenFilms)
        {
            if (films.Films.Count == userSeenFilms.UserSeenFilms.Count)
            {
                return true;
            }
            return false;
        }

        public List<string> findFavoriteGenre(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films) 
        {
            //List<string> favoriteGenres = null;

            UserSeenFilmListViewModel userSeenFilmsWithGenre;
            //var commonUsers = userSeenFilms.UserSeenFilms.Select(a => a.FilmId).Intersect(films.Films.Select(b => b.Id));
            var seenFilmsWithGenres = (from objA in films.Films
                          join objB in userSeenFilms.UserSeenFilms on objA.Id equals objB.FilmId
                          select objA/*or objB*/).ToList();

            var pairs =seenFilmsWithGenres.GroupBy(value => value.Genre).OrderByDescending(group => group.Count());

            int modeCount = pairs.First().Count();

            List<string> favoriteGenres =pairs.Where(pair => pair.Count() == modeCount)
                    .Select(pair => pair.Key)
                    .ToList();


            return favoriteGenres;
        }

        public FilmListViewModel findMoviesWithFavoriteGenre(List<string> favoriteGenres, FilmListViewModel films)
        {
            FilmListViewModel foundFilms = null;

            foundFilms.Films = (from film in films.Films
            where favoriteGenres.Contains(film.Genre) == true
            select film).ToList();


            return foundFilms;
        }
        public FilmListViewModel findMoviesWithFavoriteYears(List<string> favoriteYears, FilmListViewModel films)
        {
            FilmListViewModel foundFilms = null;

            foundFilms.Films = (from film in films.Films
                                where favoriteYears.Contains(film.Genre) == true
                                select film).ToList();


            return foundFilms;
        }

        public List<int> findFavoriteYears(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films)
        {
            UserSeenFilmListViewModel userSeenFilmsWithGenre;
            //var commonUsers = userSeenFilms.UserSeenFilms.Select(a => a.FilmId).Intersect(films.Films.Select(b => b.Id));
            var seenFilmsWithGenres = (from objA in films.Films
                                       join objB in userSeenFilms.UserSeenFilms on objA.Id equals objB.FilmId
                                       select objA/*or objB*/).ToList();

            var pairs = seenFilmsWithGenres.GroupBy(value => value.ReleaseYear).OrderByDescending(group => group.Count());

            int modeCount = pairs.First().Count();

            List<int> favoriteYears = pairs.Where(pair => pair.Count() == modeCount)
                    .Select(pair => pair.Key)
                    .ToList();
            return favoriteYears;
        }

    }
}
