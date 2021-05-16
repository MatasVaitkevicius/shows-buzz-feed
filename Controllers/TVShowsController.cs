using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.TVShows;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TVShowsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TVShowsController(ApplicationDbContext context, IMapper mapper)
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
        public async Task<TVShowsViewModel> GetTVShows(int id)
        {
            var dto = _mapper.Map<TVShowsViewModel>(await _context.TVShows.FirstOrDefaultAsync(e => e.Id == id));

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateTvShowCommand command)
        {
            var entity = new TVShows
            {
                Name = command.Name,
                Genre = command.Genre,
                Director = command.Director,
                ReleaseYear = command.ReleaseYear
            };

            _context.TVShows.Add(entity);

            await _context.SaveChangesAsync();

            return Ok(entity.Id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put([FromBody] UpdateTVShowCommand command)
        {
            var entity = await _context.TVShows.FirstOrDefaultAsync(e => e.Id == command.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TVShows), command.Id);
            }

            entity.Name = command.Name;
            entity.Genre = command.Genre;
            entity.Director = command.Director;
            entity.ReleaseYear = command.ReleaseYear;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTVShow(int id)
        {
            var result = await _context.TVShows.FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
