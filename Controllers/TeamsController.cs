using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CodeExpBackend.Data;
using CodeExpBackend.DTOs.Teams;
using CodeExpBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeExpBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController: ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TeamsController(ApplicationDbContext dbContext, IMapper mapper, ILogger<TeamsController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<TeamResponse>> CreateTeam(
            [FromBody] CreateTeamRequest createTeamRequest
        )
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(createTeamRequest.RequestorId);
                if (user is null) return NotFound();
                
                var team = await _dbContext.Teams.AddAsync(new Team
                {
                    Name = createTeamRequest.Name,
                    Code = new Random().Next(10000, 99999),
                    Users = new List<User> {user}
                });
                await _dbContext.SaveChangesAsync();

                return Ok(_mapper.Map<TeamResponse>(team.Entity));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while creating team");
                return Problem();
            }
        }

        [HttpGet("{joinCode:int}")]
        public async Task<ActionResult<TeamResponse>> JoinTeam(
            [FromBody] Guid requestorId, [FromRoute] int joinCode)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(requestorId);
                if (user is null) return NotFound();

                var team = await _dbContext.Teams.Include(t => t.Users).SingleOrDefaultAsync(t => t.Code == joinCode);
                if (team == default(Team)) return NotFound();

                team.Users.Add(user);
                _dbContext.Teams.Update(team);
                await _dbContext.SaveChangesAsync();

                return Ok(_mapper.Map<TeamResponse>(team));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while joining team");
                return Problem();
            }
        }

        [HttpGet("Leaderboard")]
        public async Task<ActionResult<IEnumerable<TeamResponse>>> ReadTeamsLeaderboard()
        {
            try
            {
                var teams = await _dbContext.Teams.OrderByDescending(t => t.Points).Take(3).ToListAsync();

                return Ok(_mapper.Map<IEnumerable<TeamResponse>>(teams));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while reading team leaderboard");
                return Problem();
            }
        }
    }
}