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
        // Short Answer Question
        public bool OneWordOnly { get; set; }
        public bool NumbersOnly { get; set; }
        public string Answer { get; set; }
        // Open Ended Question 
        public bool PhotoOnly { get; set; }
        public int MinWordRequirement { get; set; }
        // Info Slide Question
        public string Text { get; set; }
        public int Points { get; set; }
    }
}