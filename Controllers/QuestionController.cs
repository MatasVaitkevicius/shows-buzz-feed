using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Question;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public QuestionController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<QuestionListViewModel> GetAllQuestions()
        {
            try
            {
                var model = new QuestionListViewModel()
                {
                    Questions = _mapper.Map<List<QuestionViewModel>>(_context.Question)
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        [HttpGet("{id}")]
        public async Task<QuestionViewModel> GetQuestion(int id)
        {
            var dto = _mapper.Map<QuestionViewModel>(await _context.Question.FirstOrDefaultAsync(e => e.Id == id));

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateQuestionCommand command)
        {
            var entity = new Question
            {
                Content = command.Content,
                Question_No = command.Question_No,
                Row_No = command.Row_No,
                QuizId = command.QuizId,
            };

            _context.Question.Add(entity);

            await _context.SaveChangesAsync();

            return Ok(entity.Id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put([FromBody] UpdateQuestionCommand command)
        {
            var entity = await _context.Question.FirstOrDefaultAsync(e => e.Id == command.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Question), command.Id);
            }

            entity.Content = command.Content;
            entity.Question_No = command.Question_No;
            entity.Row_No = command.Row_No;
            entity.QuizId = command.QuizId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Question.FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
