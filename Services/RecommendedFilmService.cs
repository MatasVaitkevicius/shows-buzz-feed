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
using System;
using System.Collections.Generic;



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

        public async Task<(FilmViewModel,string)> GetRecommededFilmAsync(int userId)
        {
            FilmListViewModel films;
            UserSeenFilmListViewModel userSeenFilms;
            (FilmViewModel, string) filmAndMessage;
            films = await GetFilmsAsync();
            userSeenFilms = await GetUserSeenFilmsAsync(userId);
            filmAndMessage.Item1 = null;
            filmAndMessage.Item2 = "No information is given";
            List<FilmViewModel> oldFilms;
            List<FilmViewModel> newFilms;

            if (films == null) {
                filmAndMessage.Item2 = "No movies in data base.";
                return filmAndMessage;
            }

            if (userSeenFilms.UserSeenFilms.Count == 0)
            {
                filmAndMessage.Item2 = "Movies watching history is empty. Showing random movie!";
                filmAndMessage.Item1 = findUnreleasedRecommendedMovie(films.Films);
                return filmAndMessage;
            }

            oldFilms = films.Films.Where(x => x.ReleaseYear <= DateTime.Now.Year).ToList();
            newFilms = films.Films.Where(x => x.ReleaseYear > DateTime.Now.Year).ToList();
            List<string> favoriteGenre;
            List<int> favoriteYears;
            List<string> favoriteDirectors;

            bool areAllFilmsSeen = checkIfAllFilmsAreSeen(oldFilms, userSeenFilms);
            if (areAllFilmsSeen == false)
            {
                filmAndMessage.Item2 = "Not all released movies are seen!";
                
                favoriteGenre = findFavoriteGenre(userSeenFilms, films);
                favoriteYears = findFavoriteYears(userSeenFilms, films);
                favoriteDirectors = findFavoriteDirector(userSeenFilms, films);
                List<FilmViewModel> moviesWithFavoriteGenre = findMoviesWithFavoriteGenre(favoriteGenre, films);
                List<FilmViewModel> moviesWithFavoriteDirector = findMoviesWithFavoriteGenre(favoriteDirectors, films);
                List<FilmViewModel> moviesWithFavoriteYears = findMoviesWithFavoriteYears(favoriteYears, films);
                List<FilmViewModel> foundMovies = CombineFoundMovies(moviesWithFavoriteGenre, moviesWithFavoriteDirector, moviesWithFavoriteYears);
                foundMovies = RemoveAlreadySeenMovies(foundMovies, userSeenFilms);
                if (foundMovies.Count > 0)
                {
                    filmAndMessage.Item1 = findMostFrequentMovies(foundMovies);
                }
                else {
                    filmAndMessage.Item2 = "All recommended movies are seen! No unreleased movies to show!";
                    filmAndMessage.Item1 = findUnreleasedRecommendedMovie(newFilms);
                }
            }
            else 
            {
                filmAndMessage.Item2 = "All movies are seen! No unreleased movies to show!";
                //if (films.)
                filmAndMessage.Item1 = findUnreleasedRecommendedMovie(newFilms);
            }
            return filmAndMessage;
        }

        public bool checkIfAllFilmsAreSeen(List<FilmViewModel> films, UserSeenFilmListViewModel userSeenFilms)
        {
            if (films.Count == userSeenFilms.UserSeenFilms.Count)
            {
                return true;
            }
            return false;
        }

        public List<string> findFavoriteGenre(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films) 
        {
            List<string> favoriteGenres = null;
            //var commonUsers = userSeenFilms.UserSeenFilms.Select(a => a.FilmId).Intersect(films.Films.Select(b => b.Id));
            var seenFilmsWithGenres = (from objA in films.Films
                          join objB in userSeenFilms.UserSeenFilms on objA.Id equals objB.FilmId
                          select objA).ToList();

            var pairs=seenFilmsWithGenres.GroupBy(value => value.Genre).OrderByDescending(group => group.Count());

            int modeCount = pairs.First().Count();

            favoriteGenres=pairs.Where(pair => pair.Count() == modeCount)
                    .Select(pair => pair.Key)
                    .ToList();

            return favoriteGenres;
        }

        public List<FilmViewModel> findMoviesWithFavoriteGenre(List<string> favoriteGenres, FilmListViewModel films)
        {
            List<FilmViewModel> foundFilms;
            foundFilms = (from film in films.Films
            where favoriteGenres.Contains(film.Genre) == true
            select film).ToList();


            return foundFilms;
        }
        public List<FilmViewModel> findMoviesWithFavoriteYears(List<int> favoriteYears, FilmListViewModel films)
        {
            List<FilmViewModel> foundFilms;

            foundFilms = (from film in films.Films
                                where favoriteYears.Contains(film.ReleaseYear) == true
                                select film).ToList();


            return foundFilms;
        }

        public List<FilmViewModel> findMoviesWithFavoriteDirectors(List<string> favoriteDirectors, FilmListViewModel films)
        {
            List<FilmViewModel> foundFilms = null;

            foundFilms = (from film in films.Films
                                where favoriteDirectors.Contains(film.Genre) == true
                                select film).ToList();


            return foundFilms;
        }

        public List<int> findFavoriteYears(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films)
        {
            UserSeenFilmListViewModel userSeenFilmsWithGenre = null;
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
        public List<string> findFavoriteDirector(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films)
        {
            //List<string> favoriteGenres = null;

            UserSeenFilmListViewModel userSeenFilmsWithDirector = null;
            //var commonUsers = userSeenFilms.UserSeenFilms.Select(a => a.FilmId).Intersect(films.Films.Select(b => b.Id));
            var seenFilmsWithDirectors = (from objA in films.Films
                                       join objB in userSeenFilms.UserSeenFilms on objA.Id equals objB.FilmId
                                       select objA/*or objB*/).ToList();

            var pairs = seenFilmsWithDirectors.GroupBy(value => value.Director).OrderByDescending(group => group.Count());

            int modeCount = pairs.First().Count();

            List<string> favoriteDirectors = pairs.Where(pair => pair.Count() == modeCount)
                    .Select(pair => pair.Key)
                    .ToList();


            return favoriteDirectors;
        }

        public List<FilmViewModel> CombineFoundMovies(List<FilmViewModel> moviesWithFavoriteGenre, List<FilmViewModel> moviesWithFavoriteDirector, List<FilmViewModel> moviesWithFavoriteYears)
        {
            List<FilmViewModel> result;
            result = (moviesWithFavoriteGenre.Concat(moviesWithFavoriteDirector)).ToList().Concat(moviesWithFavoriteYears).ToList();
            return result;
        }

        public List<FilmViewModel> RemoveAlreadySeenMovies(List<FilmViewModel> potentiallyRecommendedFilms, UserSeenFilmListViewModel seenMovies) {

            List<FilmViewModel> result;
            result = potentiallyRecommendedFilms.Where(film => seenMovies.UserSeenFilms.All(seenFilm => film.Id!=seenFilm.FilmId)).ToList();
            return result;
        }
        public FilmViewModel findMostFrequentMovies(List<FilmViewModel> films)
        {
            FilmViewModel film = null;
            film = films.GroupBy(value => value.Id).OrderByDescending(group => group.Count()).SelectMany(group => group).First();
            return film;
        }

        public FilmViewModel findUnreleasedRecommendedMovie(List<FilmViewModel> films)
        {

            if (films.Count() > 0)
            {
                var random = new Random();
                FilmViewModel film = null;
                int index = random.Next(films.Count);
                film = films[index];
                return film;
            }
            else return null;
        }

    }
}
