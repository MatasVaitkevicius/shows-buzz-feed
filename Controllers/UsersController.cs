using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shows_buzz_feed.Data;
using shows_buzz_feed.Helpers;
using shows_buzz_feed.Mappings.User;
using shows_buzz_feed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<UserListViewModel> GetAllUsers()
        {
            try
            {
                var model = new UserListViewModel()
                {
                    Users = _mapper.Map<List<UserViewModel>>(_context.Users)
                };

                return model;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        [HttpGet("{id}")]
        public async Task<UserViewModel> GetUser(int id)
        {
            var dto = _mapper.Map<UserViewModel>(await _context.Users.FirstOrDefaultAsync(e => e.Id == id));

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateUserCommand command)
        {
            var entity = new User
            {
                Name = command.Name
            };

            _context.Users.Add(entity);

            await _context.SaveChangesAsync();

            return Ok(entity.Id);
        }
    }
}
