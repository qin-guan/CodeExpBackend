using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CodeExpBackend.Data;
using CodeExpBackend.DTOs.Quizzes;
using CodeExpBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeExpBackend.Controllers
{
    [ApiController]
    [Route("Classrooms/{classroomId:guid}/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public QuizzesController(ApplicationDbContext dbContext, IMapper mapper, ILogger<QuizzesController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizResponse>>> ReadQuizzes(
            [FromRoute] Guid classroomId
        )
        {
            try
            {
                var quizzes = await _dbContext.Quizzes.Where(q => q.ClassroomId == classroomId).ToListAsync();

                return Ok(_mapper.Map<IEnumerable<QuizResponse>>(quizzes));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while reading quizzes");
                return Problem();
            }
        }

        [HttpPost]
        public async Task<ActionResult<QuizResponse>> CreateQuiz(
            [FromRoute] Guid classroomId,
            [FromBody] CreateQuizRequest createQuizRequest
        )
        {
            try
            {
                var quiz = _mapper.Map<Quiz>(createQuizRequest);

                quiz.ClassroomId = classroomId;
                quiz = (await _dbContext.Quizzes.AddAsync(quiz)).Entity;
                await _dbContext.SaveChangesAsync();

                return Ok(_mapper.Map<QuizResponse>(quiz));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while creating quiz");
                return Problem();
            }
        }
        
        [HttpGet("{quizId:guid}/Toggle")]
        public async Task<ActionResult> StartQuiz(
            [FromRoute] Guid classroomId,
            [FromRoute] Guid quizId
        )
        {
            try
            {
                var quiz = await _dbContext.Quizzes.FindAsync(quizId);
                if (quiz is null) return NotFound();

                quiz.Live = !quiz.Live;
                _dbContext.Update(quiz);
                await _dbContext.SaveChangesAsync();

                return Ok(_mapper.Map<QuizResponse>(quiz));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while starting/stopping quiz");
                return Problem();
            }
        }
        
        [HttpPost("{quizId:guid}/Attempt")]
        public async Task<ActionResult<int>> AttemptQuiz(
            [FromRoute] Guid classroomId,
            [FromRoute] Guid quizId,
            [FromBody] AttemptQuizRequest attemptQuizRequest
        )
        {
            try
            {
                var user = await _dbContext.Users.Include(u => u.Team).SingleOrDefaultAsync(u => u.Id == attemptQuizRequest.RequestorId);
                if (user is null) return NotFound();
                
                var points = 0;
                
                var mcqs = await _dbContext.McqQuestions.Where(q => q.QuizId == quizId).Include(c => c.McqQuestionChoices).ToListAsync();
                mcqs.ForEach(mcq =>
                {
                    var choices = mcq.McqQuestionChoices.Where(c => c.IsAnswer).Select(c => c.Id);
                    if (choices.All(c => attemptQuizRequest.QuestionAttempts.Any(a => a.ChoiceId == c)))
                    {
                        points += mcq.Points;
                    }
                });

                var shortAns = await _dbContext.ShortAnswerQuestions.Where(q => q.QuizId == quizId).ToListAsync();
                shortAns.ForEach(q =>
                {
                    var attempt = attemptQuizRequest.QuestionAttempts.SingleOrDefault(a => a.QuestionId == q.Id);
                    if (attempt == default(QuestionAttemptRequest)) return;
                    
                    if (attempt.Answer == q.Answer) points += q.Points;
                });
                
                var openEnded = await _dbContext.OpenEndedQuestions.Where(q => q.QuizId == quizId).ToListAsync();
                openEnded.ForEach(q =>
                {
                    var attempt = attemptQuizRequest.QuestionAttempts.SingleOrDefault(a => a.QuestionId == q.Id);
                    if (attempt == default(QuestionAttemptRequest)) return;
                    
                    if (attempt.Answer == q.Answer) points += q.Points;
                });

                user.Points += points;
                
                if (user.Team is not null)
                {
                    user.Team.Points += points;
                }
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return Ok(points);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Fatal error while attempting quiz");
                return Problem();
            }
        }
    }
}