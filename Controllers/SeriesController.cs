using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Series;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SeriesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<SeriesListViewModel> GetAllSeries()
        {
            try
            {
                var model = new SeriesListViewModel()
                {
                    Series = _mapper.Map<List<SeriesViewModel>>(_context.Series)
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        [HttpGet("{id}")]
        public async Task<SeriesViewModel> GetSeries(int id)
        {
            var dto = _mapper.Map<SeriesViewModel>(await _context.Series.FirstOrDefaultAsync(e => e.Id == id));

            return dto;
        }
    }
}
