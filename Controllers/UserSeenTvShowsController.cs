using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
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
    public class UserSeenTvShowsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserSeenTvShowsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<UserSeenTvShowListViewModel> GetAllUserSeenTvShows(int id)
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

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateUserSeenTvShowCommand command)
        {
            try
            {
                var entity = new UserSeenTvShow
                {
                    TvShowId = command.TvShowId,
                    UserId = command.UserId,
                    CreatedAt = DateTime.Now,
                };

                _context.UserSeenTvShows.Add(entity);

                await _context.SaveChangesAsync();

                return Ok(entity.Id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.UserSeenTvShows.FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
