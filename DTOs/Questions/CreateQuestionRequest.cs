using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeExpBackend.DTOs.Questions
{
    public class CreateQuestionRequest
    {
        [Required] public string Title { get; set; }

        [Required]
        [EnumDataType(typeof(QuestionType))]
        public QuestionType QuestionType { get; set; }
        
        // Mcq Question
        public IEnumerable<CreateMcqQuestionChoiceRequest> McqQuestionChoices { get; set; }
        public int Points { get; set; }
    }
}