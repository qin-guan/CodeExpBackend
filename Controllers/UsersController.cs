﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CodeExpBackend.Data;
using CodeExpBackend.DTOs.Users;
using CodeExpBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeExpBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UsersController(ApplicationDbContext dbContext, IMapper mapper, ILogger<UsersController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var user = await _dbContext.Users.AddAsync(new User(createUserRequest.Name));
                await _dbContext.SaveChangesAsync();

                return Ok(_mapper.Map<UserResponse>(user.Entity));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Fatal error while creating new user");
                return Problem();
            }
        }
    }
}