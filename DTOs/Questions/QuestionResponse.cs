using System;

namespace CodeExpBackend.DTOs.Questions
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public QuestionType QuestionType { get; set; }
    }
}