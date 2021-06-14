using System;
using System.ComponentModel.DataAnnotations;

namespace CodeExpBackend.DTOs.Quizzes
{
    public class CreateQuizRequest
    {
        [Required] public Guid RequestorId { get; set; }
        [Required] public string Name { get; set; }
    }
}