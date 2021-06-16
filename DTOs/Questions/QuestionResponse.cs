using System;
using System.Collections.Generic;

namespace CodeExpBackend.DTOs.Questions
{
    public class McqChoiceResponse
    {
        public Guid Id { get; set; }
        public string Choice { get; set; }
    }
    public class QuestionResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<McqChoiceResponse> McqChoices { get; set; }
        public QuestionType QuestionType { get; set; }
    }
}