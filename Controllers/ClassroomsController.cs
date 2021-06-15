using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CodeExpBackend.Data;
using CodeExpBackend.DTOs.Classrooms;
using CodeExpBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeExpBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassroomsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ClassroomsController(ApplicationDbContext dbContext, IMapper mapper,
            ILogger<ClassroomsController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassroomResponse>>> ReadClassrooms(
            [FromQuery] [Required] Guid userId
        )
        {
            try
            {
                var user = await _dbContext.Users.Include(u => u.Classrooms).Where(u => u.Id == userId)
                    .SingleOrDefaultAsync();
                if (user == default(User)) return NotFound();

                var classroomResponse = user.Classrooms.Select(c => new ClassroomResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsAdmin = c.AdminUserId == userId
                });

                return Ok(classroomResponse);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while reading classrooms");
                return Problem();
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClassroomResponse>> CreateClassroom(
            [FromBody] CreateClassroomRequest createClassroomRequest)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(createClassroomRequest.RequestorId);
                if (user is null) return NotFound();

                var classroom = _mapper.Map<Classroom>(createClassroomRequest);

                classroom.Users = new List<User> {user};
                classroom.AdminUserId = createClassroomRequest.RequestorId;
                classroom = (await _dbContext.Classrooms.AddAsync(classroom)).Entity;
                await _dbContext.SaveChangesAsync();

                return Ok(new ClassroomResponse
                {
                    Id = classroom.Id,
                    Name = classroom.Name,
                    IsAdmin = true
                });
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while creating classrooms");
                return Problem();
            }
        }
    }
}