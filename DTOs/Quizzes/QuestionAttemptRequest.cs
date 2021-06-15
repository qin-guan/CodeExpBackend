using System;
using System.ComponentModel.DataAnnotations;

namespace CodeExpBackend.DTOs.Quizzes
{
    public class QuestionAttemptRequest
    {
        [Required] public Guid QuestionId { get; set; }
        public Guid ChoiceId { get; set; }
        public string Answer { get; set; }
    }
}