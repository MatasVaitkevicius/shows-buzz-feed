using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.TVShows;
using shows_buzz_feed.Mappings.UserSeenTvShow;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendedTvShowController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RecommendedTvShowController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<TVShowsListViewModel> GetAllTVShows()
        {
            try
            {
                var model = new TVShowsListViewModel()
                {
                    TVShows = _mapper.Map<List<TVShowsViewModel>>(_context.TVShows)
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        [HttpGet("{id}")]
        public async Task<UserSeenTvShowListViewModel> GetAllUserSeenTVShows(int id)
        {
            try
            {
                var model = new UserSeenTvShowListViewModel()
                {
                    UserSeenTvShows = _mapper.Map<List<UserSeenTvShowViewModel>>(_context.UserSeenTvShows.Where(e => e.UserId == id).Include(e => e.TvShow).Include(e => e.User))
                };
                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
