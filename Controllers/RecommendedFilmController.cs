using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Film;
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
    public class RecommendedFilmController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RecommendedFilmController(ApplicationDbContext context, IMapper mapper)
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
    }
}
