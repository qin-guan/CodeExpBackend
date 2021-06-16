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

                return Ok(questions.Select(q =>
                {
                    QuestionType questionType;

                    if (q.GetType() == typeof(McqQuestion)) questionType = QuestionType.MultipleChoice;
                    else if (q.GetType() == typeof(OpenEndedQuestion)) questionType = QuestionType.OpenEnded;
                    else if (q.GetType() == typeof(ShortAnswerQuestion)) questionType = QuestionType.ShortAnswer;
                    else if (q.GetType() == typeof(InfoSlideQuestion)) questionType = QuestionType.Info;
                    else throw new Exception();

                    return new QuestionResponse
                    {
                        Id = q.Id,
                        Title = q.Title,
                        QuestionType = questionType
                    };
                }));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while reading questions");
                return Problem();
            }
        }

        [HttpGet("{questionId:guid}")]
        public async Task<ActionResult<QuestionResponse>> ReadQuestion(
            [FromRoute] Guid classroomId,
            [FromRoute] Guid quizId,
            [FromRoute] Guid questionId
        )
        {
            try
            {
                var question = await _dbContext.Questions.OfType<McqQuestion>().Include(q => q.McqQuestionChoices)
                    .SingleOrDefaultAsync(q => q.Id == questionId);
                if (question is null) return NotFound();

                return Ok(new QuestionResponse 
                {
                    Id = questionId,
                    Title = question.Title,
                    McqChoices = _mapper.Map<IEnumerable<McqChoiceResponse>>(question.McqQuestionChoices),
                });
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
                            QuizId = quizId,
                            Title = createQuestionRequest.Title,
                            Points = createQuestionRequest.Points,

                            McqQuestionChoices =
                                _mapper.Map<IEnumerable<McqQuestionChoice>>(createQuestionRequest.McqQuestionChoices),
                        };

                        question = (await _dbContext.McqQuestions.AddAsync(mcqQuestion)).Entity;

                        break;
                    case QuestionType.ShortAnswer:
                        var shortAnswer = new ShortAnswerQuestion
                        {
                            QuizId = quizId,
                            Title = createQuestionRequest.Title,
                            Points = createQuestionRequest.Points,

                            Answer = createQuestionRequest.Answer,
                            OneWordOnly = createQuestionRequest.OneWordOnly,
                            NumbersOnly = createQuestionRequest.NumbersOnly,
                        };

                        question = (await _dbContext.ShortAnswerQuestions.AddAsync(shortAnswer)).Entity;

                        break;
                    case QuestionType.OpenEnded:
                        var openEnded = new OpenEndedQuestion
                        {
                            QuizId = quizId,
                            Title = createQuestionRequest.Title,
                            Points = createQuestionRequest.Points,

                            Answer = createQuestionRequest.Answer,
                            PhotoOnly = createQuestionRequest.PhotoOnly,
                            NumbersOnly = createQuestionRequest.NumbersOnly,
                            MinWordRequirement = createQuestionRequest.MinWordRequirement,
                        };

                        question = (await _dbContext.OpenEndedQuestions.AddAsync(openEnded)).Entity;

                        break;
                    case QuestionType.Info:
                        var info = new InfoSlideQuestion
                        {
                            QuizId = quizId,
                            Title = createQuestionRequest.Title,

                            Text = createQuestionRequest.Text
                        };

                        question = (await _dbContext.InfoSlideQuestions.AddAsync(info)).Entity;

                        break;
                    default:
                        return BadRequest();
                }

                await _dbContext.SaveChangesAsync();
                return Ok(new QuestionResponse
                {
                    Id = question.Id,
                    Title = question.Title,
                    QuestionType = createQuestionRequest.QuestionType
                });
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while creating question");
                return Problem();
            }
        }
    }
}