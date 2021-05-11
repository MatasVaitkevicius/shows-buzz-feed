using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Quiz;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public QuizController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<QuizListViewModel> GetAllQuizes()
        {
            try
            {
                var model = new QuizListViewModel()
                {
                    Quizes = _mapper.Map<List<QuizViewModel>>(_context.Quiz)
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        [HttpGet("{id}")]
        public async Task<QuizViewModel> GetQuiz(int id)
        {
            var dto = _mapper.Map<QuizViewModel>(await _context.Quiz.FirstOrDefaultAsync(e => e.Id == id));

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateQuizCommand command)
        {
            var entity = new Quiz
            {
                Name = command.Name,
                DateCreated = command.DateCreated,
                TimesCompleted = command.TimesCompleted,
                UserId = command.UserId,
            };

            _context.Quiz.Add(entity);

            await _context.SaveChangesAsync();

            return Ok(entity.Id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put([FromBody] UpdateQuizCommand command)
        {
            var entity = await _context.Quiz.FirstOrDefaultAsync(e => e.Id == command.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Quiz), command.Id);
            }

            entity.Name = command.Name;
            entity.DateCreated = command.DateCreated;
            entity.TimesCompleted = command.TimesCompleted;
            entity.UserId = command.UserId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Quiz.FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
