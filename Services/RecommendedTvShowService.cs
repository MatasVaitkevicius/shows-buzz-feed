using Newtonsoft.Json;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.TVShows;
using shows_buzz_feed.Mappings.UserSeenTvShow;
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
    public class RecommendedTvShowService
    {
        private readonly HttpClient client;
        private string baseUrl;

        public RecommendedTvShowService(HttpClient client)
        {
            this.client = client;
            baseUrl = "https://localhost:44347";
        }

        public async Task<TVShowsListViewModel> GetTvShowsAsync()
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/RecommendedTvShow/");
                return JsonConvert.DeserializeObject<TVShowsListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<UserSeenTvShowListViewModel> GetUserSeenTvShowsAsync(int userId)
        {
            try
            {
                var json = await client.GetStringAsync($"{baseUrl}/api/RecommendedTvShow/{userId}");
                return JsonConvert.DeserializeObject<UserSeenTvShowListViewModel>(json);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message;
                throw;
            }
        }

        public async Task<(TVShowsViewModel, string)> GetRecommededTvShowAsync(int userId)
        {
            TVShowsListViewModel tvShows;
            UserSeenTvShowListViewModel userSeenTvShows;
            (TVShowsViewModel, string) tvShowAndMessage;

            TVShowsListViewModel newTvShows = new TVShowsListViewModel();
            TVShowsListViewModel oldTvShows = new TVShowsListViewModel();

            tvShows = await GetTvShowsAsync();
            userSeenTvShows = await GetUserSeenTvShowsAsync(userId);
            tvShowAndMessage.Item1 = null;
            tvShowAndMessage.Item2 = "No information is given";

            if (tvShows.TVShows.Count == 0)
            {
                tvShowAndMessage.Item2 = "No tv shows in data base.";
                return tvShowAndMessage;
            }

            if (userSeenTvShows.UserSeenTvShows.Count == 0)
            {
                tvShowAndMessage.Item2 = "Tv show watching history is empty. Showing random tv show!";
                tvShowAndMessage.Item1 = findUnreleasedTvShow(tvShows);
                return tvShowAndMessage;
            }

            oldTvShows.TVShows = tvShows.TVShows.Where(x => x.ReleaseYear <= DateTime.Now.Year).ToList();
            newTvShows.TVShows = tvShows.TVShows.Where(x => x.ReleaseYear > DateTime.Now.Year).ToList();

            bool areAllTvShowsSeen = checkIfAllTvShowsAreSeen(oldTvShows, userSeenTvShows);

            if (areAllTvShowsSeen == false)
            {
                tvShowAndMessage.Item2 = "Not all released tv shows are seen!";
                TVShowsListViewModel tvShowsWithFavoriteGenre = new TVShowsListViewModel();
                TVShowsListViewModel tvShowsWithFavoriteDirector = new TVShowsListViewModel();
                TVShowsListViewModel tvShowsWithFavoriteYears = new TVShowsListViewModel();
                Parallel.Invoke(
                     () => tvShowsWithFavoriteGenre = LaunchBranchGenre(userSeenTvShows, tvShows),
                     () => tvShowsWithFavoriteDirector = LaunchBranchDirector(userSeenTvShows, tvShows),
                     () => tvShowsWithFavoriteYears = LaunchBranchYear(userSeenTvShows, tvShows)
                     );

                TVShowsListViewModel foundTvShows = new TVShowsListViewModel();
                foundTvShows = CombineFoundTvShows(tvShowsWithFavoriteGenre, tvShowsWithFavoriteDirector, tvShowsWithFavoriteYears);
                foundTvShows = RemoveAlreadySeenTvShows(foundTvShows, userSeenTvShows);
                if (foundTvShows.TVShows.Count > 0)
                {
                    tvShowAndMessage.Item1 = findMostFrequentTvShows(foundTvShows);
                }
                else
                {
                    tvShowAndMessage.Item2 = "All recommended tv shows are seen! No unreleased tv shows to show!";
                    tvShowAndMessage.Item1 = findUnreleasedTvShow(newTvShows);
                }
            }
            else
            {
                tvShowAndMessage.Item2 = "All tv shows are seen! No unreleased tv shows to show!";
                //if (tvShows.)
                tvShowAndMessage.Item1 = findUnreleasedTvShow(newTvShows);
            }
            return tvShowAndMessage;
        }
        public bool checkIfAllTvShowsAreSeen(TVShowsListViewModel tvShows, UserSeenTvShowListViewModel userSeenTvShows)
        {
            if (tvShows.TVShows.Count == userSeenTvShows.UserSeenTvShows.Count)
            {
                return true;
            }
            return false;
        }
        public TVShowsListViewModel LaunchBranchGenre(UserSeenTvShowListViewModel userSeenTvShows, TVShowsListViewModel tvShows)
        {
            TVShowsListViewModel tvShowsWithFavoriteGenre = new TVShowsListViewModel();
            List<string> favoriteGenre = findFavoriteGenre(userSeenTvShows, tvShows);
            tvShowsWithFavoriteGenre = findtvShowsWithFavoriteGenre(favoriteGenre, tvShows);
            return tvShowsWithFavoriteGenre;
        }
        public TVShowsListViewModel LaunchBranchDirector(UserSeenTvShowListViewModel userSeenTvShows, TVShowsListViewModel tvShows)
        {
            TVShowsListViewModel tvShowsWithFavoriteDirector = new TVShowsListViewModel();
            List<string> favoriteDirector = findFavoriteDirector(userSeenTvShows, tvShows);
            tvShowsWithFavoriteDirector = findtvShowsWithFavoriteDirectors(favoriteDirector, tvShows);
            return tvShowsWithFavoriteDirector;
        }
        public TVShowsListViewModel LaunchBranchYear(UserSeenTvShowListViewModel userSeenTvShows, TVShowsListViewModel tvShows)
        {
            TVShowsListViewModel tvShowsWithFavoriteYears = new TVShowsListViewModel();
            List<int> favoriteYears = findFavoriteYears(userSeenTvShows, tvShows);
            tvShowsWithFavoriteYears = findtvShowsWithFavoriteYears(favoriteYears, tvShows);
            return tvShowsWithFavoriteYears;
        }



        public List<string> findFavoriteGenre(UserSeenTvShowListViewModel userSeenTvShows, TVShowsListViewModel tvShows)
        {
            List<string> favoriteGenres = new List<string>();
            var seenTvShowsWithGenres = (from objA in tvShows.TVShows
                                       join objB in userSeenTvShows.UserSeenTvShows on objA.Id equals objB.TvShowId
                                       select objA).ToList();
            var pairs = seenTvShowsWithGenres.GroupBy(value => value.Genre).OrderByDescending(group => group.Count());
            int modeCount = pairs.First().Count();
            favoriteGenres = pairs.Where(pair => pair.Count() == modeCount).Select(pair => pair.Key).ToList();
            return favoriteGenres;
        }

        public TVShowsListViewModel findtvShowsWithFavoriteGenre(List<string> favoriteGenres, TVShowsListViewModel tvShows)
        {
            TVShowsListViewModel foundTvShows = new TVShowsListViewModel();
            foundTvShows.TVShows = (from tvShow in tvShows.TVShows
                                where favoriteGenres.Contains(tvShow.Genre) == true
                                select tvShow).ToList();
            return foundTvShows;
        }

        public List<string> findFavoriteDirector(UserSeenTvShowListViewModel userSeenTvShows, TVShowsListViewModel tvShows)
        {
            var seenTvShowsWithDirectors = (from objA in tvShows.TVShows
                                          join objB in userSeenTvShows.UserSeenTvShows on objA.Id equals objB.TvShowId
                                          select objA/*or objB*/).ToList();
            var pairs = seenTvShowsWithDirectors.GroupBy(value => value.Director).OrderByDescending(group => group.Count());
            int modeCount = pairs.First().Count();
            List<string> favoriteDirectors = pairs.Where(pair => pair.Count() == modeCount)
                    .Select(pair => pair.Key)
                    .ToList();

            return favoriteDirectors;
        }
        public TVShowsListViewModel findtvShowsWithFavoriteDirectors(List<string> favoriteDirectors, TVShowsListViewModel tvShows)
        {
            TVShowsListViewModel foundTvShows = new TVShowsListViewModel();

            foundTvShows.TVShows = (from tvShow in tvShows.TVShows
                                where favoriteDirectors.Contains(tvShow.Genre) == true
                                select tvShow).ToList();


            return foundTvShows;
        }

        public List<int> findFavoriteYears(UserSeenTvShowListViewModel userSeenTvShows, TVShowsListViewModel tvShows)
        {
            var seenTvShowsWithGenres = (from objA in tvShows.TVShows
                                       join objB in userSeenTvShows.UserSeenTvShows on objA.Id equals objB.TvShowId
                                       select objA/*or objB*/).ToList();
            var pairs = seenTvShowsWithGenres.GroupBy(value => value.ReleaseYear).OrderByDescending(group => group.Count());
            int modeCount = pairs.First().Count();
            List<int> favoriteYears = pairs.Where(pair => pair.Count() == modeCount)
                    .Select(pair => pair.Key)
                    .ToList();
            return favoriteYears;
        }
        public TVShowsListViewModel findtvShowsWithFavoriteYears(List<int> favoriteYears, TVShowsListViewModel tvShows)
        {
            TVShowsListViewModel foundTvShows = new TVShowsListViewModel();

            foundTvShows.TVShows = (from tvShow in tvShows.TVShows
                                where favoriteYears.Contains(tvShow.ReleaseYear) == true
                                select tvShow).ToList();


            return foundTvShows;
        }


        public TVShowsListViewModel CombineFoundTvShows(TVShowsListViewModel tvShowsWithFavoriteGenre, TVShowsListViewModel tvShowsWithFavoriteDirector, TVShowsListViewModel tvShowsWithFavoriteYears)
        {
            TVShowsListViewModel result = new TVShowsListViewModel();
            result.TVShows = (tvShowsWithFavoriteGenre.TVShows.Concat(tvShowsWithFavoriteDirector.TVShows)).ToList().Concat(tvShowsWithFavoriteYears.TVShows).ToList();
            return result;
        }

        public TVShowsListViewModel RemoveAlreadySeenTvShows(TVShowsListViewModel potentiallyRecommendedTvShows, UserSeenTvShowListViewModel seenTvShows)
        {

            TVShowsListViewModel result = new TVShowsListViewModel();
            result.TVShows = potentiallyRecommendedTvShows.TVShows.Where(tvShow => seenTvShows.UserSeenTvShows.All(seenTvShow => tvShow.Id != seenTvShow.TvShowId)).ToList();
            return result;
        }
        public TVShowsViewModel findMostFrequentTvShows(TVShowsListViewModel tvShows)
        {
            TVShowsViewModel tvShow = null;
            tvShow = tvShows.TVShows.GroupBy(value => value.Id).OrderByDescending(group => group.Count()).SelectMany(group => group).First();
            return tvShow;
        }

        public TVShowsViewModel findUnreleasedTvShow(TVShowsListViewModel tvShows)
        {

            if (tvShows.TVShows.Count() > 0)
            {
                var random = new Random();
                TVShowsViewModel tvShow = null;
                int index = random.Next(tvShows.TVShows.Count);
                tvShow = tvShows.TVShows[index];
                return tvShow;
            }
            else return null;
        }

    }
}
