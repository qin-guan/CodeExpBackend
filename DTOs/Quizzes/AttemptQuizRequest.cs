using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeExpBackend.DTOs.Quizzes
{
    public class AttemptQuizRequest
    {
        [Required] public Guid RequestorId { get; set; }
        [Required] public IEnumerable<QuestionAttemptRequest> QuestionAttempts { get; set; }
    }
}