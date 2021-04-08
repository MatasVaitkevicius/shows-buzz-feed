using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Film;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public FilmsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<FilmListViewModel> GetAllFilms()
        {
            try
            {
                var model = new FilmListViewModel()
                {
                    Films = _mapper.Map<List<FilmViewModel>>(_context.Films)
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        [HttpGet("{id}")]
        public async Task<FilmViewModel> GetFilm(int id)
        {
            var dto = _mapper.Map<FilmViewModel>(await _context.Films.FirstOrDefaultAsync(e => e.Id == id));

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateFilmCommand command)
        {
            var entity = new Film
            {
                Length = command.Length,
                Budget = command.Budget,
                ReleaseYear = command.ReleaseYear,
                Name = command.Name,
                Genre = command.Genre,
            };

            _context.Films.Add(entity);

            await _context.SaveChangesAsync();

            return Ok(entity.Id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put([FromBody] UpdateFilmCommand command)
        {
            var entity = await _context.Films.FirstOrDefaultAsync(e => e.Id == command.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Film), command.Id);
            }

            entity.Id = command.Id;
            entity.Budget = command.Budget;
            entity.Length = command.Length;
            entity.ReleaseYear = command.ReleaseYear;
            entity.Name = command.Name;
            entity.Genre = command.Genre;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Films.FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
