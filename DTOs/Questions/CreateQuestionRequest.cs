using System.ComponentModel.DataAnnotations;

namespace CodeExpBackend.DTOs.Questions
{
    public class CreateQuestionRequest
    {
        [Required] public string Title { get; set; }
        [Required] public QuestionType QuestionType { get; set; }
    }
}