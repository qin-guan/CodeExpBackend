using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CodeExpBackend.Data;
using CodeExpBackend.DTOs.Questions;
using CodeExpBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeExpBackend.Controllers
{
    [ApiController]
    [Route("Classrooms/{classroomId:guid}/Quizzes/{quizId:guid}/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public QuestionsController(ApplicationDbContext dbContext, IMapper mapper, ILogger<QuestionsController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionResponse>>> ReadQuestions(
            [FromRoute] Guid classroomId,
            [FromRoute] Guid quizId
        )
        {
            try
            {
                var questions = await _dbContext.Questions.Where(q => q.QuizId == quizId).ToListAsync();
                return Ok(_mapper.Map<IEnumerable<QuestionResponse>>(questions));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while reading questions");
                return Problem();
            }
        }

        [HttpPost]
        public async Task<ActionResult<QuestionResponse>> CreateQuestion(
            [FromRoute] Guid classroomId,
            [FromRoute] Guid quizId,
            [FromBody] CreateQuestionRequest createQuestionRequest
        )
        {
            try
            {
                Question question;
                switch (createQuestionRequest.QuestionType)
                {
                    case QuestionType.MultipleChoice:
                        if (createQuestionRequest.McqQuestionChoices is null)
                            return BadRequest();

                        var mcqQuestion = new McqQuestion
                        {
                            McqQuestionChoices =
                                _mapper.Map<IEnumerable<McqQuestionChoice>>(createQuestionRequest.McqQuestionChoices),
                            QuizId = quizId,
                            Title = createQuestionRequest.Title,
                            Points = createQuestionRequest.Points
                        };

                        question = (await _dbContext.McqQuestions.AddAsync(mcqQuestion)).Entity;

                        break;
                    default:
                        return Problem();
                }

                await _dbContext.SaveChangesAsync();
                return Ok(_mapper.Map<QuestionResponse>(question));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while creating question");
                return Problem();
            }
        }
    }
}