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

            FilmListViewModel newFilms = new FilmListViewModel();
            FilmListViewModel oldFilms = new FilmListViewModel();

            films = await GetFilmsAsync();
            userSeenFilms = await GetUserSeenFilmsAsync(userId);
            filmAndMessage.Item1 = null;
            filmAndMessage.Item2 = "No information is given";

            if (films.Films.Count == 0) {
                filmAndMessage.Item2 = "No movies in data base.";
                return filmAndMessage;
            }

            if (userSeenFilms.UserSeenFilms.Count == 0)
            {
                filmAndMessage.Item2 = "Movies watching history is empty. Showing random movie!";
                filmAndMessage.Item1 = findUnreleasedFilm(films);
                return filmAndMessage;
            }

            oldFilms.Films = films.Films.Where(x => x.ReleaseYear <= DateTime.Now.Year).ToList();
            newFilms.Films = films.Films.Where(x => x.ReleaseYear > DateTime.Now.Year).ToList();

            bool areAllFilmsSeen = checkIfAllFilmsAreSeen(oldFilms, userSeenFilms);

            if (areAllFilmsSeen == false)
            {
                filmAndMessage.Item2 = "Not all released movies are seen!";
                FilmListViewModel filmsWithFavoriteGenre = new FilmListViewModel();
                FilmListViewModel filmsWithFavoriteDirector = new FilmListViewModel();
                FilmListViewModel filmsWithFavoriteYears = new FilmListViewModel();
                Parallel.Invoke(
                     () => filmsWithFavoriteGenre = LaunchBranchGenre(userSeenFilms, films),
                     () => filmsWithFavoriteDirector = LaunchBranchDirector(userSeenFilms, films),
                     () => filmsWithFavoriteYears = LaunchBranchYear(userSeenFilms, films)
                     );

                FilmListViewModel foundFilms = new FilmListViewModel();
                foundFilms = CombineFoundFilms(filmsWithFavoriteGenre, filmsWithFavoriteDirector, filmsWithFavoriteYears);
                foundFilms = RemoveAlreadySeenFilms(foundFilms, userSeenFilms);
                if (foundFilms.Films.Count > 0)
                {
                    filmAndMessage.Item1 = findMostFrequentFilms(foundFilms);
                }
                else {
                    filmAndMessage.Item2 = "All recommended movies are seen! No unreleased movies to show!";
                    filmAndMessage.Item1 = findUnreleasedFilm(newFilms);
                }
            }
            else 
            {
                filmAndMessage.Item2 = "All movies are seen! No unreleased movies to show!";
                //if (films.)
                filmAndMessage.Item1 = findUnreleasedFilm(newFilms);
            }
            return filmAndMessage;
        }

        //public bool checkIfTvShowsListIsEmpty(FilmListViewModel films) {
        //    if (films.Films.Count == 0)
        //    {
        //        return true;
        //    }
        //    else return false;
        //}
        //public bool checkIfSeenTVShowsListIsEmpty(UserSeenFilmListViewModel films)
        //{
        //    if (films.UserSeenFilms.Count == 0)
        //    {
        //        return true;
        //    }
        //    else return false;
        //}

        public bool checkIfAllFilmsAreSeen(FilmListViewModel films, UserSeenFilmListViewModel userSeenFilms)
        {
            if (films.Films.Count == userSeenFilms.UserSeenFilms.Count)
            {
                return true;
            }
            return false;
        }
        public FilmListViewModel LaunchBranchGenre(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films) {
            FilmListViewModel filmsWithFavoriteGenre = new FilmListViewModel();
            List<string> favoriteGenre = findFavoriteGenre(userSeenFilms, films);
            filmsWithFavoriteGenre = findFilmsWithFavoriteGenre(favoriteGenre, films);
            return filmsWithFavoriteGenre;
        }
        public FilmListViewModel LaunchBranchDirector(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films)
        {
            FilmListViewModel filmsWithFavoriteDirector = new FilmListViewModel();
            List<string> favoriteDirector = findFavoriteDirector(userSeenFilms, films);
            filmsWithFavoriteDirector = findFilmsWithFavoriteDirectors(favoriteDirector, films);
            return filmsWithFavoriteDirector;
        }
        public FilmListViewModel LaunchBranchYear(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films)
        {
            FilmListViewModel filmsWithFavoriteYears = new FilmListViewModel();
            List<int> favoriteYears = findFavoriteYears(userSeenFilms, films);
            filmsWithFavoriteYears = findFilmsWithFavoriteYears(favoriteYears, films);
            return filmsWithFavoriteYears;
        }



        public List<string> findFavoriteGenre(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films) 
        {
            List<string> favoriteGenres = new List<string>();
            var seenFilmsWithGenres = (from objA in films.Films
                          join objB in userSeenFilms.UserSeenFilms on objA.Id equals objB.FilmId
                          select objA).ToList();
            var pairs=seenFilmsWithGenres.GroupBy(value => value.Genre).OrderByDescending(group => group.Count());
            int modeCount = pairs.First().Count();
            favoriteGenres=pairs.Where(pair => pair.Count() == modeCount).Select(pair => pair.Key).ToList();
            return favoriteGenres;
        }

        public FilmListViewModel findFilmsWithFavoriteGenre(List<string> favoriteGenres, FilmListViewModel films)
        {
            FilmListViewModel foundFilms = new FilmListViewModel();
            foundFilms.Films = (from film in films.Films
            where favoriteGenres.Contains(film.Genre) == true
            select film).ToList();
            return foundFilms;
        }

        public List<string> findFavoriteDirector(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films)
        {
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
        public FilmListViewModel findFilmsWithFavoriteDirectors(List<string> favoriteDirectors, FilmListViewModel films)
        {
            FilmListViewModel foundFilms = new FilmListViewModel();

            foundFilms.Films = (from film in films.Films
                                where favoriteDirectors.Contains(film.Director) == true
                                select film).ToList();


            return foundFilms;
        }

        public List<int> findFavoriteYears(UserSeenFilmListViewModel userSeenFilms, FilmListViewModel films)
        {
            var seenFilmsWithDirectors = (from objA in films.Films
                                       join objB in userSeenFilms.UserSeenFilms on objA.Id equals objB.FilmId
                                       select objA/*or objB*/).ToList();
            var pairs = seenFilmsWithDirectors.GroupBy(value => value.ReleaseYear).OrderByDescending(group => group.Count());
            int modeCount = pairs.First().Count();
            List<int> favoriteYears = pairs.Where(pair => pair.Count() == modeCount)
                    .Select(pair => pair.Key)
                    .ToList();
            return favoriteYears;
        }
        public FilmListViewModel findFilmsWithFavoriteYears(List<int> favoriteYears, FilmListViewModel films)
        {
            FilmListViewModel foundFilms = new FilmListViewModel();

            foundFilms.Films = (from film in films.Films
                          where favoriteYears.Contains(film.ReleaseYear) == true
                          select film).ToList();


            return foundFilms;
        }


        public FilmListViewModel CombineFoundFilms(FilmListViewModel filmsWithFavoriteGenre, FilmListViewModel filmsWithFavoriteDirector, FilmListViewModel filmsWithFavoriteYears)
        {
            FilmListViewModel result = new FilmListViewModel();
            result.Films = (filmsWithFavoriteGenre.Films.Concat(filmsWithFavoriteDirector.Films)).ToList().Concat(filmsWithFavoriteYears.Films).ToList();
            return result;
        }

        public FilmListViewModel RemoveAlreadySeenFilms(FilmListViewModel potentiallyRecommendedFilms, UserSeenFilmListViewModel seenFilms) {

            FilmListViewModel result = new FilmListViewModel();
            result.Films = potentiallyRecommendedFilms.Films.Where(film => seenFilms.UserSeenFilms.All(seenFilm => film.Id!=seenFilm.FilmId)).ToList();
            return result;
        }
        public FilmViewModel findMostFrequentFilms(FilmListViewModel films)
        {
            FilmViewModel film = null;
            film = films.Films.GroupBy(value => value.Id).OrderByDescending(group => group.Count()).SelectMany(group => group).First();
            return film;
        }

        public FilmViewModel findUnreleasedFilm(FilmListViewModel films)
        {

            if (films.Films.Count() > 0)
            {
                var random = new Random();
                FilmViewModel film = null;
                int index = random.Next(films.Films.Count);
                film = films.Films[index];
                return film;
            }
            else return null;
        }

    }
}
