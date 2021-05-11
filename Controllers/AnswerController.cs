using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.Answer;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AnswerController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<AnswerListViewModel> GetAllAnswers()
        {
            try
            {
                var model = new AnswerListViewModel()
                {
                    Answers = _mapper.Map<List<AnswerViewModel>>(_context.Answer)
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        //[HttpGet("{id}")]
        //public async Task<AnswerViewModel> GetAnswer(int id)
        //{
        //    var dto = _mapper.Map<AnswerViewModel>(await _context.Answer.FirstOrDefaultAsync(e => e.Id == id));

        //    return dto;
        //}

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateAnswerCommand command)
        {
            var entity = new Answer
            {
                Text = command.Text,
                Weight = command.Weight,
                QuestionId = command.QuestionId,
            };

            _context.Answer.Add(entity);

            await _context.SaveChangesAsync();

            return Ok(entity.Id);
        }

        [HttpGet("question-answer/{id}")]
        public async Task<AnswerListViewModel> GetAllAnswer(int id)
        {
            try
            {
                var model = new AnswerListViewModel()
                {
                    Answers = _mapper.Map<List<AnswerViewModel>>(_context.Answer.Where(e => e.QuestionId == id).Include(e => e.Question))
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Answer.FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

    }
}
