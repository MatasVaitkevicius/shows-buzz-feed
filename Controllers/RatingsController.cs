using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Mappings.Rating;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RatingsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("all/{id}")]
        public async Task<RatingListViewModel> GetAllRatings(int id)
        {
            try
            {
                var model = new RatingListViewModel()
                {
                    Ratings = _mapper.Map<List<RatingViewModel>>(_context.Ratings.Where(e => e.UserId == id).Include(e => e.UserSeenFilm).Include(e => e.UserSeenFilm.Film).Include(e => e.UserSeenTvShow).Include(e => e.UserSeenTvShow.TvShow))
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        [HttpGet("{id}")]
        public async Task<RatingViewModel> GetRating(int id)
        {
            var dto = _mapper.Map<RatingViewModel>(await _context.Ratings.FirstOrDefaultAsync(e => e.Id == id));

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateRatingCommand command)
        {

            try
            {
                var entity = new Rating
                {
                    DateRated = DateTime.Now,
                    Comment = command.Comment,
                    Rate = command.Rate,
                    UserId = command.UserId,
                    UserSeenFilmId = command.UserSeenFilmId,
                    UserSeenTvShowId = command.UserSeenTvShowId,

                };
                _context.Ratings.Add(entity);

                await _context.SaveChangesAsync();

                return Ok(entity.Id);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
