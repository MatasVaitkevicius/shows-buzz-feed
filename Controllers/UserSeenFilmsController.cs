using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Mappings.UserSeenFilm;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSeenFilmsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserSeenFilmsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<UserSeenFilmListViewModel> GetAllUserSeenFilms(int id)
        {
            try
            {
                var model = new UserSeenFilmListViewModel()
                {
                    UserSeenFilms = _mapper.Map<List<UserSeenFilmViewModel>>(_context.UserSeenFilms.Where(e => e.UserId == id).Include(e => e.Film).Include(e => e.User))
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateUserSeenFilmCommand command)
        {
            try
            {
                var entity = new UserSeenFilm
                {
                    FilmId = command.FilmId,
                    UserId = command.UserId,
                    CreatedAt = DateTime.Today,
                };

                _context.UserSeenFilms.Add(entity);

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
            var result = await _context.UserSeenFilms.FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
